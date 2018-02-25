using System.Collections.Generic;
using System;

namespace StoryStack.Core {

    [Serializable]
    public class SaveModel {

        public List<StoryNode> storyStack;
        public List<StoryStatus> statusStack;
        public List<string> popCallbackStack;

        public string choiceCallback;

        public List<StoryModel> storyModels;
        public Dictionary<string, StoryModel> idToStoryModels;

        public int choice;
    }
}