using Cysharp.Threading.Tasks;
using LitMotion.Animation;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace NUtilities.Popup
{
    [RequireComponent(typeof(Canvas), typeof(GraphicRaycaster))]
    public class Popup : MonoBehaviour
    {
        [Header("Content")]
        public Image backgroundImage;
        public CanvasGroup canvasGroup;
        
        [Header("Animator")]
        public Animator animator;
        public string animatorOpenState = "Open";
        public string animatorCloseState = "Close";
        
        [Header("LitMotion")]
        public LitMotionAnimation openAnimation;
        public LitMotionAnimation closeAnimation;
        
        [Header("Events")]
        public UnityEvent<object> OnOpen;
        public UnityEvent<object> OnHide;
        
        protected Canvas canvas;
        protected GraphicRaycaster raycaster;
        protected UniTaskCompletionSource<object> onCompletedTask;
        
        private int _openStateHash;
        private int _closeStateHash;

        protected virtual void Awake()
        {
            // cache animator trigger hashes
            _openStateHash = Animator.StringToHash(animatorOpenState);
            _closeStateHash = Animator.StringToHash(animatorCloseState);
            
            // get components
            canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();
            animator = animator ?? GetComponent<Animator>();
            if (animator != null)
            {
                animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            }
            
            // disable canvas and raycaster by default
            canvas.enabled = false;
            raycaster.enabled = false;
        }

        [ContextMenu("Show Popup")]
        public void Show()
        {
            Show(null);
        }

        public void Show(object dataInput, UniTaskCompletionSource<object> completeTask = null)
        {
            // check if the popup is already open
            if (canvas.enabled)
            {
                Debug.LogWarning($"Popup {gameObject.name} is already open");
                completeTask?.TrySetResult(null);
                return;
            }
            
            // invoke the onOpen event
            OnOpen?.Invoke(dataInput);
            gameObject.SendMessage("OnOpenPopup", dataInput, SendMessageOptions.DontRequireReceiver);
            
            // enable canvas and raycaster
            canvas.enabled = true;
            raycaster.enabled = true;
            onCompletedTask = completeTask;
            if (canvasGroup != null) canvasGroup.interactable = false;
            
            // play animation
            if (animator != null)
            {
                animator.Play(_openStateHash);
            }
            else if (openAnimation != null)
            {
                openAnimation.Play();
            }
            else
            {
                ShowCompleted();
            }
        }

        [ContextMenu("Hide Popup")]
        public void Hide()
        {
            Hide(null);
        }

        public void Hide(object result)
        {
            // invoke the onHide event
            OnHide?.Invoke(result);
            gameObject.SendMessage("OnHidePopup", result, SendMessageOptions.DontRequireReceiver);
            onCompletedTask?.TrySetResult(result);
            
            // disable canvasGroup
            if (canvasGroup != null) canvasGroup.interactable = false;
            
            // play animation
            if (animator != null)
            {
                animator.Play(_closeStateHash);
            }
            else if (closeAnimation != null)
            {
                closeAnimation.Play();
            }
            else
            {
                HideCompleted();
            }
        }

        public void HideCompleted()
        {
            Debug.Log("Popup Hide Completed: " + gameObject.name);
            canvas.enabled = false;
            raycaster.enabled = false;
            
            if (openAnimation != null) openAnimation.Stop();
            if (closeAnimation != null) UniTask.NextFrame().ContinueWith(() => closeAnimation.Stop()).Forget();
        }

        public void ShowCompleted()
        {
            Debug.Log("Popup Show Completed: " + gameObject.name);
            if (canvasGroup != null) canvasGroup.interactable = true;
        }
    }
}