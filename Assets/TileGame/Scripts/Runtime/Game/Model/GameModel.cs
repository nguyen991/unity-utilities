using System;
using System.Collections.Generic;
using System.Linq;
using TileGame.Game.Controller;
using UnityEngine;

namespace TileGame.Game.Model
{
    public class GameModel
    {
        public List<Tile> tiles;
        public List<Tile> selectedTiles;

        public GameModel()
        {
            tiles = new List<Tile>();
            selectedTiles = new List<Tile>();
        }
        
        public bool IsOnTop(Tile tile)
        {
            return tiles.All(other => !other.selectable || other.data.layer <= tile.data.layer ||
                                      (Math.Abs(other.data.index.x) - Math.Abs(tile.data.index.x)) >= 1 ||
                                      (Math.Abs(other.data.index.y) - Math.Abs(tile.data.index.y)) >= 1);
        }

        public void AddSelectTile(Tile tile)
        {
            if (!tiles.Contains(tile) || selectedTiles.Contains(tile))
            {
                return;
            }
            
            // find last index
            var lastIndex = selectedTiles.FindLastIndex(t => t.data.id == tile.data.id);
            if (lastIndex < 0 || lastIndex >= selectedTiles.Count - 1)
            {
                selectedTiles.Add(tile);
            }
            else
            {
                selectedTiles.Insert(lastIndex + 1, tile);
            }
                
            // update overlay tiles
            tile.selectable = false;
            UpdateOverlayTiles();
        }
        
        public void UpdateOverlayTiles()
        {
            tiles.ForEach(tile => tile.SetOverlay(!IsOnTop(tile)));
        }
    }
}