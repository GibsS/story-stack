using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using TMPro;

public class TextStoryView : StoryView {

    const float DURATION_BEFORE_HINT = 20;

    public event Action onClick;

    public Button button;
    public TMP_Text descriptionText;
    public TMP_Text tapLabel;

    public GameObject effectContainer;
    public GameObject effectTemplate;

    ValueAnimator tapLabelAnimator;

    bool inputSignaled;

    float appear;

    public override void Initialize() {
        base.Initialize();

        tapLabelAnimator = ValueAnimator.Create(gameObject, 0);

        effectTemplate.gameObject.SetActive(false);

        tapLabel.color = new Color(tapLabel.color.r, tapLabel.color.g, tapLabel.color.b, 0);

        button.onClick.AddListener(() => {
            if (onClick != null && !inputSignaled) {
                inputSignaled = true;

                onClick();
            }
        });
    }

    public void Setup(TextStoryModel model) {
        appear = Time.time + DURATION_BEFORE_HINT;

        descriptionText.text = model.description;

        if (model.effects != null) {
            foreach (string effect in model.effects) {
                GameObject obj = Instantiate(effectTemplate);
                obj.transform.SetParent(effectContainer.transform, false);
                obj.SetActive(true);

                TMP_Text text = obj.GetComponent<TMP_Text>();
                text.text = effect;
            }
        }
    }

    protected override void Update() {
        base.Update();

        if(Time.time > appear) {
            appear = 1000000000000;
            tapLabelAnimator.QueueAnimation(1, 1);
        }

        if(tapLabelAnimator.isAnimating) {
            tapLabel.color = new Color(tapLabel.color.r, tapLabel.color.g, tapLabel.color.b, tapLabelAnimator.value);
        }
    }
}
