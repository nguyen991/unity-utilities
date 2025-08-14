using System;
using LitMotion;
using LitMotion.Extensions;
using TileGame.Game.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TileGame.Game.Controller
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Tile : MonoBehaviour
    {
        public SpriteRenderer iconRenderer;
        public SpriteRenderer maskRenderer;
        public UnityAction<Tile> OnTileSelected;

        public TileData data { get; } = new();
        
        private BoxCollider2D _collider2D;

        public bool selectable
        {
            get => _collider2D.enabled;
            set => _collider2D.enabled = value;
        }

        private void Awake()
        {
            _collider2D = GetComponent<BoxCollider2D>();
        }

        private void OnMouseDown()
        {
            OnTileSelected.Invoke(this);
        }

        public void SetData(int id, int layer, Vector2 index)
        {
            data.id = id;
            data.layer = layer;
            data.index = index;
            selectable = true;
            
            // set debug text
            GetComponentInChildren<TextMeshPro>().text = data.id.ToString();
        }

        public void Shake()
        {
            LMotion.Shake.Create(Vector3.zero, Vector3.one * 20f, 0.15f).BindToLocalEulerAngles(transform);
        }

        public void SetOverlay(bool value)
        {
            maskRenderer.enabled = value;
        }
    }
}
