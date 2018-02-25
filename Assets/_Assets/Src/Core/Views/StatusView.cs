using UnityEngine;
using UnityEngine.UI;

using System;
using System.Linq;
using System.Collections;

namespace StoryStack.Views {

    public enum UIPosition {
        HIDDEN,
        HALF_REVEALED,
        REVEALED
    }

    public class StatusView : MonoBehaviour {

        public event Action onClick;

        public Button button;
        RectTransform rect;

        public GameObject statusEntryContainer;

        public GameObject statsContainer;

        public GameObject statTemplate;
        public GameObject statEntryTemplate;
        public GameObject statusEntryTemplate;

        public UIPosition position { get; private set; }

        ValueAnimator positionAnimator;

        public void Initialize() {
            position = UIPosition.HALF_REVEALED;

            rect = GetComponent<RectTransform>();

            button.onClick.AddListener(() => {
                if (onClick != null) onClick();
            });

            positionAnimator = ValueAnimator.Create(gameObject);
        }
        void Update() {
            if (positionAnimator.isAnimating) {
                rect.anchoredPosition = new Vector2(0, positionAnimator.value);
            }
        }

        public void UpdateStatus(StoryStatusModel model) {
            // Clean up
            foreach (Transform t in statusEntryContainer.transform) {
                if (t.gameObject != statusEntryTemplate && t.gameObject != statEntryTemplate) {
                    Destroy(t.gameObject);
                }
            }

            foreach (Transform t in statsContainer.transform) {
                if (t.gameObject != statTemplate && model.stats.First(m => m.id == t.gameObject.GetComponent<StatView>().id) == null) {
                    Destroy(t.gameObject);
                }
            }

            statTemplate.gameObject.SetActive(false);
            statEntryTemplate.gameObject.SetActive(false);
            statusEntryTemplate.gameObject.SetActive(false);

            // Setup
            if (model.stats != null) {
                foreach (StoryStatusStatModel stat in model.stats) {
                    StatView statView = null;

                    foreach (Transform t in statsContainer.transform) {
                        statView = t.GetComponent<StatView>();

                        if (statView.id == stat.id) {
                            break;
                        } else {
                            statView = null;
                        }
                    }

                    if (statView != null) {
                        statView.UpdateValue(stat.quantity);
                    } else {
                        GameObject go = Instantiate(statTemplate);
                        go.transform.SetParent(statsContainer.transform, false);
                        go.SetActive(true);

                        go.GetComponent<StatView>().Setup(stat);
                    }
                }
            }

            if (model.entries != null) {
                foreach (StoryStatusEntryModel entry in model.entries) {
                    GameObject go = Instantiate(statusEntryTemplate);
                    go.transform.SetParent(statusEntryContainer.transform, false);
                    go.SetActive(true);

                    go.GetComponent<StatusEntryView>().Setup(entry);
                }
            }

            if (model.stats != null) {
                foreach (StoryStatusStatModel stat in model.stats) {
                    GameObject go = Instantiate(statEntryTemplate);
                    go.transform.SetParent(statusEntryContainer.transform, false);
                    go.SetActive(true);

                    go.GetComponent<StatEntryView>().Setup(stat);
                }
            }
        }

        public void Reveal(bool animated) {
            float target = -statusEntryContainer.GetComponent<RectTransform>().rect.height;

            ChangePosition(target, animated);

            position = UIPosition.REVEALED;
        }
        public void HalfReveal(bool animated) {
            ChangePosition(0, animated);

            position = UIPosition.HALF_REVEALED;
        }
        public void Hide(bool animated) {
            float target = statsContainer.GetComponent<RectTransform>().rect.height;

            ChangePosition(target, animated);

            position = UIPosition.HIDDEN;
        }

        void ChangePosition(float newPosition, bool animated) {
            if (animated) {
                positionAnimator.ClearQueue();
                positionAnimator.QueueAnimation(newPosition, 0.2f);
            } else {
                positionAnimator.ClearQueue();
                positionAnimator.SetValue(newPosition);

                rect.anchoredPosition = new Vector2(0, positionAnimator.value);
            }
        }
    }
}