using UnityEngine;

public class Main : MonoBehaviour {

    public GameView mainView;

    public PrefabStore store;

    GameState state;
    
    void Start() {
        store.Initialize();

        state = new GameState();

        mainView.Initialize();

        mainView.onInput += HandleInput;
        mainView.ShowStory(state.Start(new TestStoryNode()), true);
    }

    void HandleInput(int choice) {
        mainView.ShowStory(state.MakeChoice(choice), true);
    }
}
