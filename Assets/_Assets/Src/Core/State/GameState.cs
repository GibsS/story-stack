using System.Collections.Generic;

using UnityEngine;

namespace StoryStack.Core {

    public class GameState {

        public List<StoryModel> storyModels;
        public Dictionary<string, StoryModel> idToStoryModels;

        StoryNodeStack storyStack;

        SaveModel previousState;

        public GameState() {
            storyModels = new List<StoryModel>();
            idToStoryModels = new Dictionary<string, StoryModel>();

            storyStack = new StoryNodeStack(this);
        }

        public Tuple<StoryViewModel, StoryStatusModel> Start(StoryNode node) {
            StoryViewModel storyModel = storyStack.Start(node);
            if (storyModel != null) {
                storyModel.Validate();
            }

            StoryStatusModel statusModel = storyStack.GetStatus();
            if (statusModel != null) {
                statusModel.Validate();
            }

            return new Tuple<StoryViewModel, StoryStatusModel>(storyModel, statusModel);
        }
        public Tuple<StoryViewModel, StoryStatusModel> MakeChoice(int choice) {
            previousState = _Save();
            previousState.choice = choice;

            StoryViewModel storyModel = storyStack.MakeChoice(choice);
            if (storyModel != null) {
                storyModel.Validate();
            }

            StoryStatusModel statusModel = storyStack.GetStatus();
            if (statusModel != null) {
                statusModel.Validate();
            }

            return new Tuple<StoryViewModel, StoryStatusModel>(storyModel, statusModel);
        }

        public X GetSystem<X>() where X : StorySystem, new() {
            X system = new X();

            system._Inject(this, null);

            return system;
        }
        public X GetSystem<X>(string id) where X : StorySystem, new() {
            X system = new X();

            system._Inject(this, id);

            return system;
        }

        public X GetModel<X>() where X : StoryModel, new() {
            StoryModel model = storyModels.Find(m => m is X);

            if (model == null) {
                model = new X();

                storyModels.Add(model);

                if (!model.GetType().IsSerializable) {
                    Debug.LogWarning("[StoryNodeStack] StoryModel sub types need to be serializable but " + model.GetType().Name + " isn't.");
                }
            }

            return model as X;
        }
        public X GetModel<X>(string id) where X : StoryModel, new() {
            StoryModel model;

            idToStoryModels.TryGetValue(id, out model);

            if (model == null) {
                model = new X();

                idToStoryModels[id] = model;

                if (!model.GetType().IsSerializable) {
                    Debug.LogWarning("[StoryNodeStack] StoryModel sub types need to be serializable but " + model.GetType().Name + " isn't.");
                }
            }

            return model as X;
        }

        SaveModel _Save() {
            Dictionary<string, StoryModel> saveIdToStories = new Dictionary<string, StoryModel>();

            if (idToStoryModels != null) {
                foreach (KeyValuePair<string, StoryModel> pair in idToStoryModels) {
                    saveIdToStories[pair.Key] = pair.Value.Clone();
                }
            }

            List<StoryNode> saveStoryStack = new List<StoryNode>(storyStack.stack);
            List<StoryStatus> saveStatusStack = new List<StoryStatus>(storyStack.status);
            List<string> savePopCallbackStack = new List<string>(storyStack.popCallbacks);

            saveStoryStack.Reverse();
            saveStatusStack.Reverse();
            savePopCallbackStack.Reverse();

            return new SaveModel {
                storyStack = saveStoryStack,
                statusStack = saveStatusStack,
                popCallbackStack = savePopCallbackStack,

                choiceCallback = storyStack.choiceCallback,

                storyModels = storyModels == null ? null : storyModels.ConvertAll(m => m.Clone()),
                idToStoryModels = saveIdToStories
            };
        }
        public SaveModel CreateSave() {
            return previousState;
        }

        public Tuple<StoryViewModel, StoryStatusModel> LoadSave(SaveModel save) {
            storyStack = new StoryNodeStack(this);

            storyStack.stack = new Stack<StoryNode>(save.storyStack);
            storyStack.status = new Stack<StoryStatus>(save.statusStack);
            storyStack.popCallbacks = new Stack<string>(save.popCallbackStack);

            storyStack.choiceCallback = save.choiceCallback;

            idToStoryModels = new Dictionary<string, StoryModel>();

            if (save.idToStoryModels != null) {
                foreach (KeyValuePair<string, StoryModel> pair in save.idToStoryModels) {
                    idToStoryModels[pair.Key] = pair.Value.Clone();
                }
            }

            storyModels = save.storyModels == null ? null : save.storyModels.ConvertAll(s => s.Clone());

            // INJECTION
            foreach (var s in save.storyStack) s._Inject(this);
            foreach (var s in save.statusStack) if (s != null) s._Inject(this);

            return MakeChoice(save.choice);
        }
    }
}