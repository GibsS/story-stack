using UnityEngine;

public class Main : MonoBehaviour {

    public GameView mainView;

    public PrefabStore store;

    GameState state;

    bool end;
    
    void Start() {
        store.Initialize();

        state = new GameState();

        mainView.Initialize();

        mainView.onInput += HandleInput;

        var storyModel = state.Start(new TestStoryNode());
        mainView.ShowStory(storyModel, true && !storyModel.Item1.preventFade);
    }

    void HandleInput(int choice) {
        if (!end) {
            var model = state.MakeChoice(choice);

            if (model.Item1 == null) {
                end = true;
                mainView.ShowStory(
                    new Tuple<StoryViewModel, StoryStatusModel>(new TextStoryModel { description = "The end." }, null), 
                    true
                );
            } else {
                mainView.ShowStory(model, true && !model.Item1.preventFade);
            }
        }
    }
}
