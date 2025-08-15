using System;
using System.Collections.Generic;
using System.Linq;
using TileGame.Game.Controller;

namespace TileGame.Game.Model
{
    public class GameModel
    {
        public int level;
        public List<Tile> tiles;
        public List<Tile> holdingTiles;
        public List<Tile> record;
        public bool isExpandHolding = false;

        public GameModel()
        {
            level = 1;
            tiles = new();
            holdingTiles = new();
            record = new();
        }

        public bool IsOnTop(Tile tile)
        {
            return tiles.All(other =>
                other.data.layer <= tile.data.layer
                || Math.Abs(other.data.index.x - tile.data.index.x) > 0.5
                || Math.Abs(other.data.index.y - tile.data.index.y) > 0.5
            );
        }

        public void AddSelectTile(Tile tile)
        {
            if (!tiles.Contains(tile) || holdingTiles.Contains(tile))
            {
                return;
            }

            // remove tile from tiles
            tiles.Remove(tile);

            // find last index
            var lastIndex = holdingTiles.FindLastIndex(t =>
                t.data.id == tile.data.id && t.State == Tile.TileState.Holding
            );
            if (lastIndex < 0 || lastIndex >= holdingTiles.Count - 1)
            {
                holdingTiles.Add(tile);
            }
            else
            {
                holdingTiles.Insert(lastIndex + 1, tile);
            }

            // add to record
            record.Add(tile);

            // update new tile state
            tile.State = Tile.TileState.Holding;

            // update removing tiles
            CheckHoldingTiles();

            // update overlay tiles
            UpdateOverlayTiles();
        }

        public void CheckHoldingTiles()
        {
            var lastTileId = -1;
            var count = 0;
            for (var i = 0; i < holdingTiles.Count; i++)
            {
                var tile = holdingTiles[i];
                if (tile.State == Tile.TileState.Removing)
                    continue;

                // count tiles with the same id
                if (lastTileId < 0 || lastTileId != tile.data.id)
                {
                    lastTileId = tile.data.id;
                    count = 1;
                }
                else if (lastTileId == tile.data.id)
                {
                    count++;
                }

                // remove tile if count is greater than 2
                if (count > 2)
                {
                    for (var j = i - count + 1; j <= i; j++)
                    {
                        holdingTiles[j].State = Tile.TileState.Removing;
                    }
                }
            }
        }

        public void RemoveHoldingTiles(IEnumerable<Tile> tiles)
        {
            foreach (var tile in tiles)
            {
                holdingTiles.Remove(tile);
                record.Remove(tile);
            }
        }

        public void RemoveHoldingTile(Tile tile)
        {
            holdingTiles.Remove(tile);
            record.Remove(tile);
        }

        public void UpdateOverlayTiles()
        {
            tiles.ForEach(tile =>
                tile.State = IsOnTop(tile) ? Tile.TileState.Over : Tile.TileState.Under
            );
        }

        public bool IsHoldingFull()
        {
            return holdingTiles.Count(t => t.State == Tile.TileState.Holding)
                >= AvailableHoldingSpace;
        }

        public int RemainHoldingSpace()
        {
            return AvailableHoldingSpace
                - holdingTiles.Count(t => t.State == Tile.TileState.Holding);
        }

        public int AvailableHoldingSpace =>
            isExpandHolding ? GameConst.MaxHoldingSize : GameConst.HoldingSize;

        public bool IsCompleted()
        {
            return tiles.Count() == 0 && holdingTiles.Count() == 0;
        }
    }
}
