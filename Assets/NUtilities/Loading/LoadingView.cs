using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using LitMotion.Animation;
using NUtilities.Helper.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NUtilities.Loading
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public class LoadingView : MonoBehaviour
    {
        [SerializeField]
        private float delayOnComplete = 0.15f;

        [SerializeField]
        private bool autoHide = true;

        [Header("Animator")]
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private string animatorOpenState = "Open";

        [SerializeField]
        private string animatorCloseState = "Close";

        [Header("LitMotion")]
        [SerializeField]
        private LitMotionAnimation loadingAnimation;

        [SerializeField]
        private LitMotionAnimation closeAnimation;

        [Header("Progress")]
        [SerializeField]
        private SlicedFilledImage progressBar;

        [SerializeField]
        private TextMeshProUGUI progressText;

        private Canvas _canvas;
        private GraphicRaycaster _raycaster;
        private MotionHandle _progressMotion;
        public bool allowHide { get; set; } = true;

        void Start()
        {
            _canvas = GetComponent<Canvas>();
            _raycaster = GetComponent<GraphicRaycaster>();
            _canvas.enabled = false;
            _raycaster.enabled = false;
        }

        public void StartLoading()
        {
            if (progressBar)
                progressBar.fillAmount = 0f;

            if (progressText)
                progressText.text = "";

            if (animator && !string.IsNullOrEmpty(animatorOpenState))
                animator.Play(animatorOpenState);

            loadingAnimation?.Play();

            // show canvas and raycaster
            _canvas.enabled = true;
            _raycaster.enabled = true;
            allowHide = autoHide;
        }

        public void OnProgress(float progress)
        {
            if (progressBar)
            {
                if (
                    _progressMotion != null
                    && (_progressMotion.IsActive() || _progressMotion.IsPlaying())
                )
                {
                    _progressMotion.TryCancel();
                }
                _progressMotion = LMotion
                    .Create(progressBar.fillAmount, progress, 0.15f)
                    .WithEase(Ease.OutSine)
                    .Bind(v => progressBar.fillAmount = v)
                    .AddTo(this);
            }

            progressText?.SetText($"{progress * 100:0.00}%");
        }

        public async UniTask OnComplete()
        {
            progressText?.SetText("100%");

            // play close animation
            loadingAnimation?.Stop();
            closeAnimation?.Play();
            if (animator && !string.IsNullOrEmpty(animatorCloseState))
            {
                animator.Play(animatorCloseState);
            }

            if (progressBar)
            {
                await LMotion
                    .Create(progressBar.fillAmount, 1f, delayOnComplete)
                    .WithEase(Ease.OutSine)
                    .Bind(v => progressBar.fillAmount = v)
                    .AddTo(this);
            }
            else
            {
                await UniTask.Delay(TimeSpan.FromSeconds(delayOnComplete));
            }

            // hide the canvas and raycaster
            await UniTask.WaitUntil(() => allowHide);
            _canvas.enabled = false;
            _raycaster.enabled = false;
        }
    }
}
