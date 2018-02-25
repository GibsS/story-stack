using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace StoryStack {

    public class TextStoryModel : StoryViewModel {

        public string description;

        public string[] effects;

        public override void Validate() {
            if (string.IsNullOrEmpty(description)) {
                Debug.LogWarning("[TextStoryModel] Description can't be null");
            }

            if (effects != null && effects.Length > 4) {
                Debug.LogWarning("[TextStoryModel] There can not be more than 4 choices");
                effects = effects.Take(4).ToArray();
            }

            if (effects != null) {
                List<string> actualEffects = new List<string>();

                foreach (var effect in effects) {
                    if (string.IsNullOrEmpty(effect)) {
                        Debug.LogWarning("[TextStoryModel] Effect can't be null or empty");
                    } else {
                        actualEffects.Add(effect);
                    }
                }

                effects = actualEffects.ToArray();
            }
        }
    }
}