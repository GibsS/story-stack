using System;

using UnityEngine;

using TMPro;

namespace StoryStack.Views {

    public class StatEntryView : MonoBehaviour {

        public GameObject iconContainer;

        IconView iconView;

        public TMP_Text quantityText;
        public TMP_Text descriptionText;
        public TMP_Text statusText;

        public void Setup(StoryStatusStatModel model) {
            GameObject iconGO = Instantiate(PrefabStore.store.icon);
            iconGO.transform.SetParent(iconContainer.transform, false);
            iconGO.transform.localPosition = Vector3.zero;

            iconView = iconGO.GetComponent<IconView>();
            iconView.Setup(model);

            quantityText.text = model.quantity.ToString();
            descriptionText.text = StringUtility.FirstCharToUpper(model.name) + " - " + StringUtility.FirstCharToUpper(model.description);
            statusText.text = StringUtility.FirstCharToUpper(model.status);
        }
    }
}