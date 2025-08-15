using System.Collections.Generic;
using System.Linq;
using NUtilities.Helper.Array;
using TileGame.Game.Controller;
using UnityEngine;

namespace TileGame.Game.Manager
{
    public class BoosterManager
    {
        private readonly GameManager _gameManager;

        public BoosterManager(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void UseBooster(GameConst.BoosterType boosterType)
        {
            switch (boosterType)
            {
                case GameConst.BoosterType.Undo:
                    Undo();
                    break;
                case GameConst.BoosterType.Hint:
                    Hint();
                    break;
                case GameConst.BoosterType.Shuffle:
                    Shuffle();
                    break;
            }
        }

        public bool Undo()
        {
            if (_gameManager.GameModel.record.Count == 0)
                return false;

            // take last tile from record
            var tile = _gameManager.GameModel.record.Last();

            // add them back to tile list
            _gameManager.GameModel.RemoveHoldingTile(tile);
            _gameManager.GameModel.tiles.Add(tile);
            _gameManager.GameController.grid.ArrangeTiles();
            _gameManager.GameController.hold.ArrangeHolingTiles();
            _gameManager.GameModel.UpdateOverlayTiles();

            return true;
        }

        public bool Hint()
        {
            if (_gameManager.GameModel.holdingTiles.Count == 0)
                return false;

            // group holding tiles by count tile id
            var groupedTiles = _gameManager.GameModel.holdingTiles.GroupBy(t => t.data.id);

            // find group with size greater than 2
            var group = groupedTiles.FirstOrDefault(g => g.Count() >= 2);
            group ??= groupedTiles.FirstOrDefault();
            var groupSize = group.Count();

            // check is have enough space
            if (3 - groupSize > _gameManager.GameModel.RemainHoldingSpace())
            {
                return false;
            }

            // find tiles
            var pickTiles = new List<Tile>();
            for (var i = _gameManager.GameModel.tiles.Count - 1; i >= 0; i--)
            {
                if (_gameManager.GameModel.tiles[i].data.id == group.Key)
                {
                    pickTiles.Add(_gameManager.GameModel.tiles[i]);
                    if (pickTiles.Count + groupSize >= 3)
                        break;
                }
            }

            // add pick tiles to holding tiles
            foreach (var tile in pickTiles)
            {
                _gameManager.GameModel.AddSelectTile(tile);
            }
            _gameManager.GameController.hold.AddTiles(pickTiles);

            return true;
        }

        public bool Shuffle()
        {
            if (_gameManager.GameModel.tiles.Count <= 1)
                return false;

            for (var i = _gameManager.GameModel.tiles.Count - 1; i > 0; i--)
            {
                var r = Random.Range(0, i);

                // swap index
                (
                    _gameManager.GameModel.tiles[r].data.index,
                    _gameManager.GameModel.tiles[i].data.index
                ) = (
                    _gameManager.GameModel.tiles[i].data.index,
                    _gameManager.GameModel.tiles[r].data.index
                );

                // swap layer
                (
                    _gameManager.GameModel.tiles[r].data.layer,
                    _gameManager.GameModel.tiles[i].data.layer
                ) = (
                    _gameManager.GameModel.tiles[i].data.layer,
                    _gameManager.GameModel.tiles[r].data.layer
                );
            }

            // arrange tiles
            _gameManager.GameController.grid.ArrangeTiles();
            _gameManager.GameModel.UpdateOverlayTiles();

            return true;
        }
    }
}
