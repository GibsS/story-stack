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

    public RectGraphic ripple;

    ChoiceModel choice;

    ValueAnimator rippleAnimator;

    bool clicked;

    public void Initialize() {
        rippleAnimator = ValueAnimator.Create(gameObject);

        button.onClick.AddListener(() => {
            if (!clicked) {
                clicked = true;

                rippleAnimator.QueueAnimation(1, 0.5f);

                if (onClick != null) onClick(choice);
            }
        });
    }

    void Update() {
        if(rippleAnimator.isAnimating) {
            ripple.margin = rippleAnimator.value * 5;
            ripple.lineWidth = (1 - rippleAnimator.value) * 2;
        }
    }

    public void Setup(ChoiceModel model) {
        choice = model;

        if(string.IsNullOrEmpty(model.requirementOrEffect)) {
            text1.gameObject.SetActive(true);
            text2.gameObject.SetActive(false);
            subText.gameObject.SetActive(false);

            text1.text = model.action;
        } else {
            text1.gameObject.SetActive(false);
            text2.gameObject.SetActive(true);
            subText.gameObject.SetActive(true);

            text2.text = model.action;
            subText.text = model.requirementOrEffect;
        }
    }
    public void SetupAsBack() {
        text1.gameObject.SetActive(true);
        text2.gameObject.SetActive(false);
        subText.gameObject.SetActive(false);

        text1.text = "Back";
    }
}
