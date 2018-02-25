using UnityEngine;

public class Main : MonoBehaviour {

    public GameView mainView;

    public PrefabStore store;

    GameState state;

    bool end;

    SaveModel save;
    
    void Start() {
        store.Initialize();

        mainView.Initialize();

        mainView.onInput += HandleInput;

        StartStory();
    }

    void StartStory() {
        state = new GameState();

        var storyModel = state.Start(new TestStoryNode());
        mainView.ShowStory(storyModel, true && !storyModel.Item1.preventFade);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            save = state.CreateSave();
        }

        if(Input.GetKeyDown(KeyCode.D)) {
            if (save != null) {
                end = false;
                var storyModel = state.LoadSave(save);
                mainView.ShowStory(storyModel, true && !storyModel.Item1.preventFade);
            } else {
                StartStory();
            }
        }
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
