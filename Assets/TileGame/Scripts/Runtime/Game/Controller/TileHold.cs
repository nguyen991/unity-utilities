using System;
using System.Collections.Generic;
using System.Linq;
using LitMotion;
using LitMotion.Extensions;
using NUtilities.Helper.Device;
using NUtilities.Pool;
using TileGame.Game.Manager;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace TileGame.Game.Controller
{
    public class TileHold : MonoBehaviour
    {
        public GameObject trailPrefab;
        public Transform container;
        public UnityAction onArrangeComplete;

        [SerializeField]
        private float spacing = 1f;

        private GameManager _gameManager;
        private PoolSystem _poolSystem;
        private MotionHandle _arrangeCompleteHandle;
        private List<MotionHandle> _arrangeHandles = new List<MotionHandle>();

        private void Start()
        {
            // scale to fit width
            var containWidth = spacing * GameConst.MaxHoldingSize;
            var screenWidth = Device.GetScreenWidth();
            var scale = screenWidth > containWidth ? 1 : screenWidth / containWidth;
            container.transform.localScale = new Vector3(scale, scale, 1);
        }

        [Inject]
        public void Inject(GameManager gameManager, PoolSystem poolSystem)
        {
            _gameManager = gameManager;
            _poolSystem = poolSystem;
        }

        public void AddTile(Tile newTile)
        {
            // change parent
            newTile.transform.SetParent(container, true);
            newTile.transform.localPosition += new Vector3(0, 0, -2f);

            // arrange
            ArrangeHolingTiles();
        }

        public void AddTiles(IEnumerable<Tile> newTiles)
        {
            // change parent
            foreach (var tile in newTiles)
            {
                tile.transform.SetParent(container, true);
                tile.transform.localPosition += new Vector3(0, 0, -2f);
            }

            // arrange
            ArrangeHolingTiles();
        }

        public void ArrangeHolingTiles()
        {
            // move tile to container
            var index = -1;
            _arrangeHandles.ForEach(handle =>
            {
                if (handle.IsPlaying())
                    handle.Cancel();
            });
            _arrangeHandles = _gameManager
                .GameModel.holdingTiles.Select(tile =>
                {
                    index++;
                    ;

                    return LSequence
                        .Create()
                        .Append(
                            LMotion
                                .Create(
                                    tile.transform.localPosition,
                                    new Vector3(
                                        spacing * (index - (GameConst.MaxHoldingSize - 1) / 2f),
                                        0,
                                        0
                                    ),
                                    0.15f
                                )
                                .WithEase(Ease.InSine)
                                .BindToLocalPosition(tile.transform)
                        )
                        .Join(
                            LMotion
                                .Create(tile.transform.localScale, Vector3.one, 0.15f)
                                .WithEase(Ease.InSine)
                                .BindToLocalScale(tile.transform)
                        )
                        .Run();
                })
                .ToList();

            // wait for move completed
            if (_arrangeCompleteHandle.IsPlaying())
                _arrangeCompleteHandle.Cancel();
            _arrangeCompleteHandle = LMotion
                .Create(0, 1, 0.15f)
                .WithOnComplete(() => onArrangeComplete())
                .RunWithoutBinding();
        }
    }
}
