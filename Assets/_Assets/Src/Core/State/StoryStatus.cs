[System.Serializable]
public abstract class StoryStatus {

    [System.NonSerialized] GameState state;

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

    #region HELPER

    public StoryStatusModel CreateModel() {
        return new StoryStatusModel();
    }

    #endregion

    public abstract StoryStatusModel GetStatus();
}
