using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class ChoiceModel {

    public int id;

    public string action;
    public string requirementOrEffect;

    public bool locked;
    public bool requirement;

    public ChoiceModel[] subChoices;

    public bool isMenu { get { return subChoices != null && subChoices.Length != 0; } }

    public void Validate() {
        if(string.IsNullOrEmpty(action)) {
            Debug.LogWarning("[ChoiceModel] Action is empty.");
        }

        if(subChoices != null && subChoices.Length == 0) {
            Debug.LogWarning("[ChoiceModel] A menu can't be empty");
        }

        if(subChoices != null) {
            List<ChoiceModel> actualSubChoices = new List<ChoiceModel>();

            foreach(ChoiceModel choice in subChoices) {
                if(choice == null) {
                    Debug.LogWarning("[ChoiceModel] Choices can't be null");
                } else {
                    actualSubChoices.Add(choice);
                }
            }

            subChoices = actualSubChoices.ToArray();
        }
    }

    public ChoiceModel AddChoice(int id, string action, string requirement = null) {
        ChoiceModel newChoice = new ChoiceModel {
            id = id,
            action = action,
            requirementOrEffect = requirement
        };

        if (subChoices == null) {
            subChoices = new ChoiceModel[] { newChoice };
        } else {
            List<ChoiceModel> newChoices = subChoices.ToList();

            newChoices.Add(newChoice);

            subChoices = newChoices.ToArray();
        }

        return newChoice;
    }
}
