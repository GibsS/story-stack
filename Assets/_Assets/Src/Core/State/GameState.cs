using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState {

    public List<StoryModel> storyModels;
    public Dictionary<string, StoryModel> idToStoryModels;

    StoryNodeStack storyStack;

    public GameState() {
        storyModels = new List<StoryModel>();
        idToStoryModels = new Dictionary<string, StoryModel>();

        storyStack = new StoryNodeStack(this);
    }

    public Tuple<StoryViewModel, StoryStatusModel> Start(StoryNode node) {
        return new Tuple<StoryViewModel, StoryStatusModel>(storyStack.Start(node), storyStack.GetStatus());
    }
    public Tuple<StoryViewModel, StoryStatusModel> MakeChoice(int choice) {
        return new Tuple<StoryViewModel, StoryStatusModel>(storyStack.MakeChoice(choice), storyStack.GetStatus());
    }

    public X GetSystem<X>() where X : StorySystem, new() {
        X system = new X();

        system._Inject(this);

        return system;
    }
    public X GetSystem<X>(string id) where X : StorySystem, new() {
        X system = new X();

        system._Inject(this);

        return system;
    }

    public X GetModel<X>() where X : StoryModel, new() {
        StoryModel model = storyModels.Find(m => m is X);

        if(model == null) {
            model = new X();

            storyModels.Add(model);
        }

        return model as X;
    }
    public X GetModel<X>(string id) where X : StoryModel, new() {
        StoryModel model;

        idToStoryModels.TryGetValue(id, out model);

        if(model == null) {
            model = new X();

            idToStoryModels[id] = model;
        }

        return model as X;
    }
}
