using UnityEngine;
using System.Collections;

public class ChoiceStoryModel : StoryViewModel {

    public string description;
    public string choiceDescription;

    public float duration;

    public string[] effects;

    public ChoiceModel[] choices;
}
