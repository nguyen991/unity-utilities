using System;
using System.Collections.Generic;
using TileGame.Level;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace TileGame.Game.Controller
{
    public class GridController : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private Vector3 tileSize;
        
        public UnityAction<Tile> OnTileSelectedEvent;
        
        private IObjectPool<Tile> _tilePool;
        private List<Tile> _availableTiles;
        private List<Tile> _selectedTiles;

        private void Awake()
        {
            _tilePool = new ObjectPool<Tile>(CreateTile, actionOnGet: GetTile, actionOnRelease: ReleaseTile);
            _availableTiles = new List<Tile>();
            _selectedTiles = new List<Tile>();
        }

        public void Init(LevelData levelData)
        {
            _availableTiles.ForEach(tile => _tilePool.Release(tile));
            _selectedTiles.ForEach(tile => _tilePool.Release(tile));
            levelData.layers.ForEach(layer =>
            {
                layer.stones.ForEach(tilePos =>
                {
                    var tile = _tilePool.Get();
                    tile.transform.localPosition = tilePos;
                });
            });
        }

        private Tile CreateTile()
        {
            var tile = Instantiate(tilePrefab, transform).GetComponent<Tile>();
            tile.transform.SetParent(transform);
            tile.OnTileSelected = OnSelectedTile;
            return tile;
        }

        private void OnSelectedTile(Tile tile)
        {
            OnTileSelectedEvent.Invoke(tile);
        }

        private void GetTile(Tile tile)
        {
            tile.transform.SetParent(transform);
            tile.gameObject.SetActive(true);
        }
        
        private void ReleaseTile(Tile tile)
        {
            tile.gameObject.SetActive(false);
        }
    }
}