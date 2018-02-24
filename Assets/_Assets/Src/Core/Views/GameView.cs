using UnityEngine;
using System;
using System.Collections;

public class GameView : MonoBehaviour {

    public const int TIME_OUT_INPUT = -1;
    public const int NEXT_INPUT = -2;

    public event Action<int> onInput;

    public GameObject storyContainer;

    public StatusView statusView;

    StoryView storyView;

    IEnumerator switchAnimation;

    float lastChoice = 0;

    public void Initialize() {
        statusView.Initialize();

        statusView.onClick += HandleClickStatus;
    }

    public void ShowStory(Tuple<StoryViewModel, StoryStatusModel> story, bool fade) {
        if(switchAnimation != null) {
            StopCoroutine(switchAnimation);

            switchAnimation = null;
        }

        if(fade) {
            switchAnimation = SwitchStoryAnimation(story.Item1, story.Item2);

            StartCoroutine(switchAnimation);
        } else {
            if (storyView != null) {
                storyView.Destroy();
            }

            storyView = CreateStoryView(story.Item1);
            storyView.transform.SetParent(storyContainer.transform, false);

            statusView.HalfReveal(false);
            if (story.Item2 != null) {
                statusView.UpdateStatus(story.Item2);
            }
        }
    }

    IEnumerator SwitchStoryAnimation(StoryViewModel story, StoryStatusModel status) {
        if(storyView != null) {
            storyView.Hide(true);

            statusView.HalfReveal(true);

            yield return new WaitForSeconds(0.5f);

            storyView.Destroy();
        }

        statusView.UpdateStatus(status);

        storyView = CreateStoryView(story);
        storyView.transform.SetParent(storyContainer.transform, false);

        storyView.Hide(false);
        storyView.Show(true);

        switchAnimation = null;
    }

    StoryView CreateStoryView(StoryViewModel story) {
        if(story is TextStoryModel) {
            GameObject go = Instantiate(PrefabStore.store.textStoryPrefab);
            TextStoryView view = go.GetComponent<TextStoryView>();

            view.Initialize();
            view.Setup(story as TextStoryModel);

            view.onClick += () => {
                if (onInput != null) onInput(NEXT_INPUT);
            };

            return view;
        } else {
            GameObject go = Instantiate(PrefabStore.store.choiceStoryPrefab);
            ChoiceStoryView view = go.GetComponent<ChoiceStoryView>();

            view.Initialize();
            view.Setup(story as ChoiceStoryModel);

            view.onPick += a => {
                if (onInput != null) onInput(a);
            };
            view.onTimeOut += () => {
                if (onInput != null) onInput(TIME_OUT_INPUT);
            };

            return view;
        }
    }

    void HandleClickStatus() {
        if(statusView.position == UIPosition.HALF_REVEALED) {
            statusView.Reveal(true);
        } else if(statusView.position == UIPosition.REVEALED) {
            statusView.HalfReveal(true);
        }
    }
}
