using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace StoryStack {

    public class ChoiceStoryModel : StoryViewModel {

        public string description;
        public string choiceDescription;

        public float duration;

        public string[] effects;

        public ChoiceModel[] choices;

        public ChoiceModel AddChoice(int id, string action, string requirementOrEffect) {
            ChoiceModel choice = new ChoiceModel {
                id = id,
                action = action,
                requirementOrEffect = requirementOrEffect
            };

            if (choices == null) {
                choices = new ChoiceModel[] { choice };
            } else {
                List<ChoiceModel> newChoices = new List<ChoiceModel>(choices);

                newChoices.Add(choice);

                choices = newChoices.ToArray();
            }

            return choice;
        }
        public ChoiceModel AddChoice(string action, string requirementOrEffect) {
            if (choices == null) {
                return AddChoice(0, action, requirementOrEffect);
            } else {
                return AddChoice(choices[choices.Length - 1].id + 1, action, requirementOrEffect);
            }
        }

        public override void Validate() {
            if (string.IsNullOrEmpty(description)) {
                Debug.LogWarning("[ChoiceStoryModel] Description can't be null or empty");
            }

            if (duration < 0) {
                Debug.LogWarning("[ChoiceStoryModel] Duration can't be negative");
                duration = 0;
            }

            if (effects != null && effects.Length > 4) {
                Debug.LogWarning("[ChoiceStoryModel] There can not be more than 4 choices");
                effects = effects.Take(4).ToArray();
            }

            if (effects != null) {
                List<string> actualEffects = new List<string>();

                foreach (var effect in effects) {
                    if (string.IsNullOrEmpty(effect)) {
                        Debug.LogWarning("[ChoiceStoryModel] Effect can't be null or empty");
                    } else {
                        actualEffects.Add(effect);
                    }
                }

                effects = actualEffects.ToArray();
            }

            if (choices == null || choices.Length == 0) {
                Debug.LogWarning("[ChoiceStoryModel] Choice array can't be null or empty");
            }

            // Remove null choices
            List<ChoiceModel> actualChoices = new List<ChoiceModel>();

            foreach (ChoiceModel choice in choices) {
                if (choice == null) {
                    Debug.LogWarning("[ChoiceStoryModel] Choice can't be null");
                } else {
                    actualChoices.Add(choice);
                }
            }

            choices = actualChoices.ToArray();

            // Never more than 4 choices
            if (choices.Length > 4) {
                Debug.LogWarning("[ChoiceStoryModel] There can only be at most 4 choices");
            }

            // Check for duplicate choices
            List<int> choiceIds = new List<int>();

            Stack<ChoiceModel> openset = new Stack<ChoiceModel>(choices);

            while (openset.Count > 0) {
                ChoiceModel choice = openset.Pop();

                choiceIds.Add(choice.id);

                if (choice.isMenu) {
                    if (choice.subChoices.Length > 3) {
                        Debug.LogWarning("[ChoiceStoryModel] There can only be at most 3 sub choices");
                    }

                    foreach (ChoiceModel c in choice.subChoices) {
                        openset.Push(c);
                    }
                }
            }

            if (choiceIds.Count != choiceIds.Distinct().Count()) {
                Debug.LogWarning("[ChoiceStoryModel] Can't have several choices with the same id");
            }
        }
    }
}