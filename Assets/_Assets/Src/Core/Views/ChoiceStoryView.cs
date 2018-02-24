using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ChoiceStoryView : StoryView {

    public event Action<int> onPick;
    public event Action onTimeOut;

    public TMP_Text descriptionText;

    public TMP_Text choiceDescriptionText;

    public GameObject effectContainer;
    public GameObject effectTemplate;

    public Image progressImage1;
    public Image progressImage2;

    public ChoiceView[] choiceViews;

    float startTime;
    float endTime;
    
    bool inputSignaled;

    Stack<ChoiceModel[]> choicesStack;

    public override void Initialize() {
        base.Initialize();

        InitializeChoices();

        effectTemplate.SetActive(false);
    }

    public void Setup(ChoiceStoryModel model) {
        choicesStack = new Stack<ChoiceModel[]>();

        descriptionText.text = model.description;

        if (!string.IsNullOrEmpty(model.choiceDescription)) {
            choiceDescriptionText.text = model.choiceDescription;
        } else {
            choiceDescriptionText.gameObject.SetActive(false);
        }

        if (model.effects != null) {
            foreach (string effect in model.effects) {
                GameObject obj = Instantiate(effectTemplate);
                obj.transform.SetParent(effectContainer.transform);

                TMP_Text text = obj.GetComponent<TMP_Text>();
                text.text = effect;
            }
        }


        if(model.duration != 0) {
            progressImage1.gameObject.SetActive(false);
            progressImage2.gameObject.SetActive(false);

            startTime = Time.time;
            endTime = Time.time + model.duration;
        } else {
            endTime = 1000000000000;
        }

        SetupChoices(model.choices);
    }

    void InitializeChoices() {
        for(int i = 0; i < 4; i++) {
            int a = i;
            choiceViews[i].Initialize();

            choiceViews[i].onClick += model => {
                if (choicesStack.Count > 1 && a == 0) {
                    choicesStack.Pop();
                    SetupChoices(choicesStack.Pop());
                } else {
                    if (model.subChoices == null) {
                        if (onPick != null && !inputSignaled) {
                            inputSignaled = true;
                            onPick(model.id);
                        }
                    } else {
                        SetupChoices(model.subChoices);
                    }
                }
            };
        }
    }

    void SetupChoices(ChoiceModel[] choices) {
        if (choicesStack.Count > 0) {
            choiceViews[0].SetupAsBack();

            for (int i = 0; i < 3; i++) {
                if (i < choices.Length) {
                    choiceViews[i + 1].gameObject.SetActive(true);

                    choiceViews[i + 1].Setup(choices[i]);
                } else {
                    choiceViews[i + 1].gameObject.SetActive(false);
                }
            }
        } else {
            for (int i = 0; i < 4; i++) {
                if (i < choices.Length) {
                    choiceViews[i].gameObject.SetActive(true);

                    choiceViews[i].Setup(choices[i]);
                } else {
                    choiceViews[i].gameObject.SetActive(false);
                }
            }
        }

        choicesStack.Push(choices);
    }

    protected override void Update() {
        base.Update();

        if (progressImage1 != null) {
            progressImage1.fillAmount = (Time.time - startTime) / (endTime - startTime);
            progressImage2.fillAmount = (Time.time - startTime) / (endTime - startTime);
        }

        if(Time.time > endTime && !inputSignaled) {
            inputSignaled = true;
            onTimeOut();
        }
    }
}
