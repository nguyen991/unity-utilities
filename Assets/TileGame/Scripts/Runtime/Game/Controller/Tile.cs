using System;
using UnityEngine;
using UnityEngine.Events;

namespace TileGame.Game.Controller
{
    public class Tile : MonoBehaviour
    {
        public SpriteRenderer iconRenderer;
        public SpriteRenderer maskRenderer;
        
        public UnityAction<Tile> OnTileSelected;

        public void SetTitleData()
        {
            
        }

        private void OnMouseDown()
        {
            OnTileSelected.Invoke(this);
        }
    }
}
