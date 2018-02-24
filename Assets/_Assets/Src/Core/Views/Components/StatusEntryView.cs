using UnityEngine;

using TMPro;

public class StatusEntryView : MonoBehaviour {

    public TMP_Text descriptionText;
    public TMP_Text statusText;

    public void Setup(StoryStatusEntryModel model) {
        descriptionText.text = model.name + " - " + model.description;
        statusText.text = model.status;
    }
}
