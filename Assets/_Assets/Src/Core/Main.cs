using UnityEngine;

using StoryStack.Views;
using StoryStack.Core;
using StoryStack;

/// <summary>
/// A boilerplate top level object to manage the link between game logic, saves and views.
/// </summary>
public class Main : MonoBehaviour {

    public GameView mainView;

    public PrefabStore store;

    public bool deleteSave;

    GameState state;

    bool end;
    
    void Start() {
        store.Initialize();

        mainView.Initialize();

        mainView.onInput += HandleInput;

        if(deleteSave) Serializer<SaveModel>.Clear("test");
        
        LoadStory();
    }

    void LoadStory() {
        SaveModel save = Serializer<SaveModel>.Load("test");

        if (save != null) {
            end = false;
            state = new GameState();

            var storyModel = state.LoadSave(save);

            if (storyModel.Item1 == null) {
                end = true;
                mainView.ShowStory(
                    new Tuple<StoryViewModel, StoryStatusModel>(new TextStoryModel { description = "The end." }, null),
                    true
                );
            } else {
                mainView.ShowStory(storyModel, true && !storyModel.Item1.preventFade);
            }
        } else {
            StartStory();
        }
    }
    void SaveStory() {
        SaveModel save = state.CreateSave();

        if (save != null) {
            Serializer<SaveModel>.Save("test", save);
        }
    }

    void StartStory() {
        state = new GameState();

        var storyModel = state.Start(new TestStoryNode());
        mainView.ShowStory(storyModel, true && !storyModel.Item1.preventFade);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            SaveStory();
        }

        if(Input.GetKeyDown(KeyCode.D)) {
            LoadStory();
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

    void OnApplicationQuit() {
        SaveStory();
    }
}
