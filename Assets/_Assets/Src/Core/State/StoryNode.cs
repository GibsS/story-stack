using System;

[Serializable]
public abstract class StoryNode {

    [NonSerialized] GameState state;

    public void _Inject(GameState state) {
        this.state = state;
    }

    #region STATE

    protected X GetSystem<X>() where X : StorySystem, new() {
        return state.GetSystem<X>();
    }
    protected X GetSystem<X>(string id) where X : StorySystem, new() {
        return state.GetSystem<X>(id);
    }

    protected X GetModel<X>() where X : StoryModel, new() {
        return state.GetModel<X>();
    }
    protected X GetModel<X>(string id) where X : StoryModel, new() {
        return state.GetModel<X>(id);
    }

    #endregion

    #region HELPERS

    protected StoryAction Pop(int choice) {
        return StoryAction.Pop();
    }
    protected StoryAction Pop() {
        return StoryAction.Pop();
    }
    
    protected StoryAction Push(StoryNode node, Func<StoryAction> popCallback, StoryStatus status = null) {
        return StoryAction.Push(node, popCallback, status);
    }
    protected StoryAction Push<X>(Func<StoryAction> popCallback, StoryStatus status = null) where X : StoryNode, new() {
        return StoryAction.Push(new X(), popCallback, status);
    }

    protected StoryAction Text(string description, Func<int, StoryAction> choiceCallback, StoryStatus status = null, params string[] effects) {
        return StoryAction.Show(new TextStoryModel {
            description = description,
            effects = effects
        }, choiceCallback, status);
    }

    protected StoryAction Text(string description, Func<int, StoryAction> choiceCallback, params string[] effects) {
        return StoryAction.Show(new TextStoryModel {
            description = description,
            effects = effects
        }, choiceCallback);
    }

    protected StoryAction Choice(string description, string choiceDescription, Func<int, StoryAction> choiceCallback, StoryStatus status = null, params string[] effects) {
        return StoryAction.Show(new ChoiceStoryModel {
            description = description,
            choiceDescription = choiceDescription,

            effects = effects
        }, choiceCallback, status);
    }
    protected StoryAction Choice(string description, Func<int, StoryAction> choiceCallback, StoryStatus status = null, params string[] effects) {
        return StoryAction.Show(new ChoiceStoryModel {
            description = description,

            effects = effects
        }, choiceCallback, status);
    }
    protected StoryAction Choice(string description, Func<int, StoryAction> choiceCallback, params string[] effects) {
        return StoryAction.Show(new ChoiceStoryModel {
            description = description,

            effects = effects
        }, choiceCallback);
    }

    protected StoryAction EndStory() {
        return null;
    }
    protected StoryAction EndStory(int choice) {
        return null;
    }

    #endregion

    public abstract StoryAction OnEnter();
}
