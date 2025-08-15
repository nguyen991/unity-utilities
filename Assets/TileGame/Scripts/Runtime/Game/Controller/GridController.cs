using System;
using System.Collections.Generic;
using System.Linq;
using LitMotion;
using LitMotion.Extensions;
using NUtilities.Helper.Array;
using NUtilities.Helper.Device;
using NUtilities.Pool;
using TileGame.Game.Manager;
using TileGame.Level;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace TileGame.Game.Controller
{
    public class GridController : MonoBehaviour
    {
        [SerializeField]
        private GameObject tilePrefab;

        [SerializeField]
        private Vector3 tileSize;

        [SerializeField]
        private Transform container;

        public UnityAction<Tile> OnTileSelectedEvent;

        private GameManager _gameManager;
        private PoolSystem _poolSystem;

        [Inject]
        public void Inject(GameManager gameManager, PoolSystem poolSystem)
        {
            _gameManager = gameManager;
            _poolSystem = poolSystem;
        }

        public void Init(LevelData levelData)
        {
            // calculate tile size
            var tileIds = new List<int>();
            var totalTiles = levelData.layers.Sum(layer => layer.stones.Count);
            if (totalTiles % 3 != 0)
            {
                Debug.LogError(
                    "Total tiles must be a multiple of 3 for the game to work correctly."
                );
                return;
            }

            // random tile ids
            var pairs = totalTiles / 3;
            for (var i = 0; i < pairs; i++)
            {
                var r = UnityEngine.Random.Range(0, 8);
                for (var j = 0; j < 3; j++)
                {
                    tileIds.Add(r);
                }
            }
            tileIds.Shuffle();

            // initialize tiles
            var maxWidth = 0f;
            var index = 0;
            levelData.layers.ForEach(layer =>
            {
                layer.stones.ForEach(tilePos =>
                {
                    var tile = _poolSystem.Get(tilePrefab, container).GetComponent<Tile>();
                    tile.OnTileSelected = OnSelectedTile;
                    tile.transform.localPosition = GetTilePosition(layer.index, tilePos);
                    tile.transform.localScale = Vector3.one;

                    tile.SetData(tileIds[index], layer.index, tilePos);
                    _gameManager.GameModel.tiles.Add(tile);
                    index++;

                    maxWidth = Mathf.Max(maxWidth, Math.Abs(tile.transform.localPosition.x));
                });
            });
            maxWidth = maxWidth * 2f + tileSize.x * 1.5f;

            // scale to fit width
            var screenWidth = Device.GetScreenWidth();
            var scale = screenWidth > maxWidth ? 1 : screenWidth / maxWidth;
            container.transform.localScale = new Vector3(scale, scale, 1);
        }

        public void ArrangeTiles()
        {
            _gameManager.GameModel.tiles.ForEach(tile =>
            {
                // change parent
                tile.State = Tile.TileState.Over;
                tile.Touchable = false;
                tile.transform.SetParent(container, true);

                // move to position
                var position = GetTilePosition(tile.data.layer, tile.data.index);
                LMotion
                    .Create(tile.transform.localPosition, position, 0.15f)
                    .WithEase(Ease.InSine)
                    .WithOnComplete(() => tile.Touchable = true)
                    .BindToLocalPosition(tile.transform);

                // scale to default
                LMotion
                    .Create(tile.transform.localScale, Vector3.one, 0.15f)
                    .WithEase(Ease.InSine)
                    .BindToLocalScale(tile.transform);
            });
        }

        public Vector3 GetTilePosition(int layer, Vector2 index)
        {
            return new Vector3(index.x * tileSize.x, index.y * tileSize.y, layer * tileSize.z);
        }

        private void OnSelectedTile(Tile tile)
        {
            OnTileSelectedEvent.Invoke(tile);
        }
    }
}
