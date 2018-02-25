using StoryStack.Core;

namespace StoryStack {

    public abstract class StorySystem {

        public string id { get; private set; }
        GameState state;

        public void _Inject(GameState state, string id) {
            this.state = state;
            this.id = id;
        }

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
    }
}