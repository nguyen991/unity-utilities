using LitMotion;
using LitMotion.Extensions;
using NUtilities.Pool;
using TileGame.Game.Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TileGame.Game.Controller
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Tile : MonoBehaviour
    {
        public enum TileState
        {
            Under,
            Over,
            Holding,
            Removing,
        }

        public SpriteRenderer iconRenderer;
        public SpriteRenderer maskRenderer;

        public UnityAction<Tile> OnTileSelected;

        public TileData data { get; private set; } = new();

        private BoxCollider2D _collider2D;

        private TileState _state;
        public TileState State
        {
            get => _state;
            set
            {
                _state = value;
                switch (value)
                {
                    case TileState.Under:
                        maskRenderer.enabled = true;
                        Touchable = true;
                        break;
                    case TileState.Over:
                        maskRenderer.enabled = false;
                        Touchable = true;
                        break;
                    case TileState.Holding:
                        maskRenderer.enabled = false;
                        Touchable = false;
                        break;
                    case TileState.Removing:
                        maskRenderer.enabled = false;
                        Touchable = false;
                        break;
                }
            }
        }

        public bool Touchable
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
            State = TileState.Over;

            // set debug text
            GetComponentInChildren<TextMeshPro>().text = data.id.ToString();
        }

        public void Shake()
        {
            LMotion
                .Shake.Create(Vector3.zero, GameConst.ShakeVector, 0.15f)
                .BindToLocalEulerAngles(transform);
        }

        public MotionHandle Release()
        {
            return LMotion
                .Create(Vector3.one, Vector3.zero, 0.25f)
                .WithEase(Ease.InBack)
                .WithOnComplete(() => GetComponent<PoolObject>().Release())
                .BindToLocalScale(transform);
        }
    }
}
