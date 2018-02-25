using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public enum StoryActionType {
   PUSH_STORY,
   POP_STORY,
   SHOW_STORY
}

public class StoryAction {

    public StoryActionType type { get; private set; }

    public StoryViewModel story { get; private set; }
    public Func<int, StoryAction> choiceCallback { get; private set; }

    public StoryNode node { get; private set; }
    public Func<StoryAction> popCallback { get; private set; }

    public StoryStatus status { get; private set; }

    public TextStoryModel text { get { return story as TextStoryModel; } }
    public ChoiceStoryModel choice { get { return story as ChoiceStoryModel; } }

    private StoryAction() { }

    public static StoryAction Push(StoryNode node, Func<StoryAction> popCallback, StoryStatus status = null) {
        return new StoryAction {
            type = StoryActionType.PUSH_STORY,
            story = null,
            node = node,
            popCallback = popCallback,
            status = status
        };
    }

    public static StoryAction Pop() {
        return new StoryAction {
            type = StoryActionType.POP_STORY,
            story = null,
            node = null,
            popCallback = null,
            status = null
        };
    }

    public static StoryAction Show(StoryViewModel story, Func<int, StoryAction> choiceCallback, StoryStatus status = null) {
        return new StoryAction {
            type = StoryActionType.SHOW_STORY,
            story = story,
            choiceCallback = choiceCallback,
            node = null,
            popCallback = null,
            status = status
        };
    }

    public void AddEffect(string effect) {
        if(story != null) {
            if(story is TextStoryModel) {
                List<string> effects = (story as TextStoryModel).effects.ToList();

                effects.Add(effect);

                (story as TextStoryModel).effects = effects.ToArray();
            } else if(story is ChoiceStoryModel) {
                List<string> effects = (story as ChoiceStoryModel).effects.ToList();

                effects.Add(effect);

                (story as ChoiceStoryModel).effects = effects.ToArray();
            }
        } else {
            Debug.LogWarning("[StoryAction] Trying to add actions to an action without a story model");
        }
    }

    public ChoiceModel AddChoice(int id, string action, string requirementOrEffect) {
        if (story != null && story is ChoiceStoryModel) {
            return (story as ChoiceStoryModel).AddChoice(id, action, requirementOrEffect);
        } else {
            Debug.LogWarning("[StoryAction] Can only add choices to a choice model");
            return null;
        }
    }

    public ChoiceModel AddChoice(string action, string requirementOrEffect) {
        if (story != null && story is ChoiceStoryModel) {
            return (story as ChoiceStoryModel).AddChoice(action, requirementOrEffect);
        } else {
            Debug.LogWarning("[StoryAction] Can only add choices to a choice model");
            return null;
        }
    }

    public ChoiceModel AddChoice(int id, string action) {
        if (story != null && story is ChoiceStoryModel) {
            return (story as ChoiceStoryModel).AddChoice(action, null);
        } else {
            Debug.LogWarning("[StoryAction] Can only add choices to a choice model");
            return null;
        }
    }
    public ChoiceModel AddChoice(string action) {
        if (story != null && story is ChoiceStoryModel) {
            return (story as ChoiceStoryModel).AddChoice(action, null);
        } else {
            Debug.LogWarning("[StoryAction] Can only add choices to a choice model");
            return null;
        }
    }
}
