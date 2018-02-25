using UnityEngine;

namespace StoryStack.Views {

    public class StoryView : MonoBehaviour {

        public CanvasGroup canvasGroup;

        ValueAnimator fadeAnimator;

        public virtual void Initialize() {
            fadeAnimator = ValueAnimator.Create(gameObject, 1);
        }

        protected virtual void Update() {
            if (fadeAnimator.isAnimating) {
                canvasGroup.alpha = fadeAnimator.value;
            }
        }

        public void Show(bool animate) {
            if (!animate) {
                fadeAnimator.ClearQueue();
                fadeAnimator.SetValue(1);
                canvasGroup.alpha = 1;
            } else {
                fadeAnimator.ClearQueue();
                fadeAnimator.QueueAnimation(1, 0.5f);
            }
        }
        public void Hide(bool animate) {
            if (!animate) {
                fadeAnimator.ClearQueue();
                fadeAnimator.SetValue(0);
                canvasGroup.alpha = 0;
            } else {
                fadeAnimator.ClearQueue();
                fadeAnimator.QueueAnimation(0, 0.5f);
            }
        }

        public void Destroy() {
            Destroy(gameObject);
        }
    }
}