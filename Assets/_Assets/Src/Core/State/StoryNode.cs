using System;

/// <summary>
/// Represents the logic of the story: the story is a succession of StoryNodes that get dynamically created and "stacked"
/// depending on the choices of the player.
/// </summary>
[Serializable]
public abstract class StoryNode {

    [NonSerialized] GameState state;

    public void _Inject(GameState state) {
        this.state = state;
    }

    #region STATE

    /// <summary> Gets the system of the given type. </summary>
    protected X GetSystem<X>() where X : StorySystem, new() {
        return state.GetSystem<X>();
    }
    /// <summary> Gets the system of the given type with a specific id (the id is used to identify what models to modify). </summary>
    protected X GetSystem<X>(string id) where X : StorySystem, new() {
        return state.GetSystem<X>(id);
    }

    /// <summary> Gets the model (part of the state) of the given type, creates it if it doesn't exist. </summary>
    protected X GetModel<X>() where X : StoryModel, new() {
        return state.GetModel<X>();
    }
    /// <summary> Gets the model (part of the state) of the given type with that specific id, creates it if it doesn't exist. </summary>
    protected X GetModel<X>(string id) where X : StoryModel, new() {
        return state.GetModel<X>(id);
    }

    #endregion

    #region HELPERS

    /// <summary> Creates a "Pop" StoryAction, meaning the current StoryNode will be removed from the story stack. </summary>
    protected StoryAction Pop(int _) {
        return StoryAction.Pop();
    }
    /// <summary> Creates a "Pop" StoryAction, meaning the current StoryNode will be removed from the story stack. </summary>
    protected StoryAction Pop() {
        return StoryAction.Pop();
    }

    /// <summary> 
    /// Creates a "Push" StoryAction, meaning it will add a StoryNode on the stack. 
    /// The callback provided will be called when the StoryNode is popped.
    /// </summary>
    protected StoryAction Push(StoryNode node, Func<StoryAction> popCallback, StoryStatus status = null) {
        return StoryAction.Push(node, popCallback, status);
    }
    /// <summary> 
    /// Creates a "Push" StoryAction, meaning it will add a StoryNode on the stack. 
    /// The callback provided will be called when the StoryNode is popped.
    /// </summary>
    protected StoryAction Push<X>(Func<StoryAction> popCallback, StoryStatus status = null) where X : StoryNode, new() {
        return StoryAction.Push(new X(), popCallback, status);
    }

    /// <summary>
    /// Creates a "Show" StoryAction, more specifically a text StoryAction: 
    /// used to show a screen of text without any decisions
    /// </summary>
    protected StoryAction Text(string description, Func<int, StoryAction> choiceCallback, StoryStatus status = null, params string[] effects) {
        return StoryAction.Show(new TextStoryModel {
            description = description,
            effects = effects
        }, choiceCallback, status);
    }
    
    /// <summary>
    /// Creates a "Show" StoryAction, more specifically a text StoryAction: 
    /// used to show a screen of text without any decisions
    /// </summary>
    protected StoryAction Text(string description, Func<int, StoryAction> choiceCallback, params string[] effects) {
        return StoryAction.Show(new TextStoryModel {
            description = description,
            effects = effects
        }, choiceCallback);
    }
    
    /// <summary>
    /// Creates a "Show" StoryAction, more specifically a choice StoryAction: 
    /// used to show a choice between up to four choices.
    /// </summary>
    protected StoryAction Choice(string description, string choiceDescription, Func<int, StoryAction> choiceCallback, StoryStatus status = null, params string[] effects) {
        return StoryAction.Show(new ChoiceStoryModel {
            description = description,
            choiceDescription = choiceDescription,

            effects = effects
        }, choiceCallback, status);
    }
    /// <summary>
    /// Creates a "Show" StoryAction, more specifically a choice StoryAction: 
    /// used to show a choice between up to four choices.
    /// </summary>
    protected StoryAction Choice(string description, Func<int, StoryAction> choiceCallback, StoryStatus status = null, params string[] effects) {
        return StoryAction.Show(new ChoiceStoryModel {
            description = description,

            effects = effects
        }, choiceCallback, status);
    }
    /// <summary>
    /// Creates a "Show" StoryAction, more specifically a choice StoryAction: 
    /// used to show a choice between up to four choices.
    /// </summary>
    protected StoryAction Choice(string description, Func<int, StoryAction> choiceCallback, params string[] effects) {
        return StoryAction.Show(new ChoiceStoryModel {
            description = description,

            effects = effects
        }, choiceCallback);
    }

    /// <summary>
    /// Creates a "End" StoryAction, used to signal the end of the given Game.
    /// </summary>
    protected StoryAction EndStory() {
        return null;
    }
    /// <summary>
    /// Creates a "End" StoryAction, used to signal the end of the given Game.
    /// </summary>
    protected StoryAction EndStory(int _) {
        return null;
    }

    #endregion

    /// <summary>
    /// Called as the StoryNode gets added to the story stack.
    /// </summary>
    public abstract StoryAction OnEnter();
}
