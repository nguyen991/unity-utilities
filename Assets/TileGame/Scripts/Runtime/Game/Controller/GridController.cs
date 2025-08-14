using System;
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
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Vector3 tileSize;
        [SerializeField] private Transform container;
        
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
            levelData.layers.ForEach(layer =>
            {
                layer.stones.ForEach(tilePos =>
                {
                    var tile = _poolSystem.Get(tilePrefab, container).GetComponent<Tile>();
                    tile.OnTileSelected = OnSelectedTile;
                    tile.transform.localPosition = new Vector3(tilePos.x * tileSize.x, tilePos.y * tileSize.y, layer.index * tileSize.z);
                    tile.SetData(0, layer.index, tilePos);
                    _gameManager.GameModel.tiles.Add(tile);
                });
            });
        }

        private void OnSelectedTile(Tile tile)
        {
            OnTileSelectedEvent.Invoke(tile);
        }
    }
}