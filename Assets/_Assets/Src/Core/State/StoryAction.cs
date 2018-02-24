using System;

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
}
