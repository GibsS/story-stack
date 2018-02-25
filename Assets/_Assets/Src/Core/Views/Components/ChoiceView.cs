using System;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ChoiceView : MonoBehaviour {

    public event Action<ChoiceModel> onClick;

    public Button button;

    public TMP_Text text1;
    public TMP_Text text2;
    public TMP_Text subText;

    public RectGraphic rect;

    ChoiceModel choice;

    ValueAnimator rippleAnimator;

    public void Initialize() {
        rippleAnimator = ValueAnimator.Create(gameObject);

        button.onClick.AddListener(() => {
            rippleAnimator.QueueAnimation(1, 0.5f);

            if (onClick != null) onClick(choice);
        });
    }

    public void Setup(ChoiceModel model) {
        choice = model;

        if(string.IsNullOrEmpty(model.requirementOrEffect)) {
            text1.gameObject.SetActive(true);
            text2.gameObject.SetActive(false);
            subText.gameObject.SetActive(false);

            if(model.isMenu) {
                text1.text = "[" + model.action + "]";
            } else {
                text1.text = model.action;
            }
        } else {
            text1.gameObject.SetActive(false);
            text2.gameObject.SetActive(true);
            subText.gameObject.SetActive(true);
            
            if (model.isMenu) {
                text2.text = "[" + model.action + "]";
            } else {
                text2.text = model.action;
            }
            subText.text = model.requirementOrEffect;
        }

        if (model.locked) {
            rect.color = PrefabStore.store.lockedChoiceColor;
            button.interactable = false;
        } else if (model.requirement) {
            rect.color = PrefabStore.store.requirementChoiceColor;
            button.interactable = true;
        } else if (model.isMenu) {
            rect.color = PrefabStore.store.menuChoiceColor;
            button.interactable = true;
        } else {
            rect.color = PrefabStore.store.normalChoiceColor;
            button.interactable = true;
        }
    }
    public void SetupAsBack() {
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(false);
        subText.gameObject.SetActive(false);

        text1.text = "[Back]";
        rect.color = PrefabStore.store.menuChoiceColor;
        button.interactable = true;
    }
}
