using UnityEngine;

namespace TileGame.Game
{
    public static class GameConst
    {
        public const int MaxHoldingSize = 7;
        public const int HoldingSize = 6;
        public static readonly Vector3 ShakeVector = new(0, 0, 20f);

        public static class State
        {
            public const string Init = "Init";
            public const string Play = "Play";
            public const string Pause = "Pause";
            public const string End = "End";
        }

        public static class Popup
        {
            public const string Lose = "lose";
            public const string End = "end";
        }

        public static class Scene
        {
            public const string Lobby = "Lobby";
            public const string Game = "Game";
        }

        public enum BoosterType
        {
            Undo,
            Hint,
            Shuffle,
            Skip,
        }
    }
}
