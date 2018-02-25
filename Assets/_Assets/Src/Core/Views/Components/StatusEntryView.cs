using UnityEngine;

using TMPro;

namespace StoryStack.Views {

    public class StatusEntryView : MonoBehaviour {

        public TMP_Text descriptionText;
        public TMP_Text statusText;

        public void Setup(StoryStatusEntryModel model) {
            descriptionText.text = StringUtility.FirstCharToUpper(model.name) + " - " + StringUtility.FirstCharToUpper(model.description);
            statusText.text = StringUtility.FirstCharToUpper(model.status);
        }
    }
}