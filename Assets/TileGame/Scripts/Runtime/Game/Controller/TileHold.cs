using System;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using NUtilities.Pool;
using TileGame.Game.Manager;
using UnityEngine;
using UnityEngine.Pool;
using VContainer;

namespace TileGame.Game.Controller
{
    public class TileHold : MonoBehaviour
    {
        public GameObject trailPrefab;
        public Transform container;
        [SerializeField] private int maxSize = 9;
        [SerializeField] private float spacing = 1f;

        private GameManager _gameManager;
        private PoolSystem _poolSystem;

        [Inject]
        public void Inject(GameManager gameManager, PoolSystem poolSystem)
        {
            _gameManager = gameManager;
            _poolSystem = poolSystem;
        }
        
        public void AddTile(Tile tile)
        {
            // change parent
            tile.transform.SetParent(container, true);
            
            // move tile to container
            var index = 0;
            _gameManager.GameModel.selectedTiles.ForEach(t =>
            {
                LMotion.Create(tile.transform.localPosition, new Vector3(spacing * (index - (maxSize - 1) / 2f), 0, 0), 0.15f)
                    .WithEase(Ease.InSine)
                    .BindToLocalPosition(tile.transform);
                index++;
            });
        }
    }
}