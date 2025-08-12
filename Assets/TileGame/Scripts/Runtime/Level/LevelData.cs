using System.Collections.Generic;
using UnityEngine;

namespace TileGame.Level
{
    [System.Serializable]
    public class LevelData
    {
        public List<TileLayer> layers;
    }

    [System.Serializable]
    public class TileLayer
    {
        public int index;
        public List<Vector2> stones;
    }
}