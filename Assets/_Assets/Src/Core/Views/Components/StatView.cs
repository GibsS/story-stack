using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class StatView : MonoBehaviour {

    public string id { get; private set; }

    public GameObject iconContainer;
    public TMP_Text quantityText;

    public GameObject bumperGO;
    public TMP_Text bumperText;

    RectTransform bumperRect;

    ValueAnimator bumpAnimator;
    ValueAnimator valueAnimator;

    float baseBumper;

    IconView iconView;

    int cacheQuantity;

    IEnumerator bumpAnimation;

    public void Setup(StoryStatusStatModel model) {
        bumperRect = bumperGO.GetComponent<RectTransform>();

        bumpAnimator = ValueAnimator.Create(gameObject);
        valueAnimator = ValueAnimator.Create(gameObject);

        baseBumper = bumperRect.anchoredPosition.y;

        id = model.id;

        GameObject iconGO = Instantiate(PrefabStore.store.icon);
        iconGO.transform.SetParent(iconContainer.transform, false);
        iconGO.transform.localPosition = Vector3.zero;

        iconView = iconGO.GetComponent<IconView>();
        iconView.Setup(model);

        quantityText.text = model.quantity.ToString();
        cacheQuantity = model.quantity;

        valueAnimator.SetValue(model.quantity);

        bumperGO.SetActive(false);
    }

    void Update() {
        if (bumpAnimator.isAnimating) {
            bumperRect.anchoredPosition = new Vector2(0, baseBumper + bumpAnimator.value);
        }

        if (valueAnimator.isAnimating) {
            quantityText.text = ((int)valueAnimator.value).ToString();
        }
    }

    public void UpdateValue(int value) {
        if (value != cacheQuantity) {
            if (bumpAnimation != null) {
                StopCoroutine(bumpAnimation);
            }

            bumpAnimation = BumpValue(value);

            StartCoroutine(bumpAnimation);
        }
    }

    IEnumerator BumpValue(int newQuantity) {
        bumperRect.anchoredPosition = new Vector2(0, baseBumper);

        valueAnimator.ClearQueue();
        valueAnimator.QueueAnimation(newQuantity, 0.5f);

        bumperGO.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        bumperText.text = (newQuantity > cacheQuantity ? "+" : "-") + Mathf.Abs(newQuantity - cacheQuantity).ToString();

        bumpAnimator.ClearQueue();
        bumpAnimator.SetValue(0);
        bumpAnimator.QueueAnimation(-20, 0.8f);

        bumperGO.SetActive(true);

        yield return new WaitForSeconds(0.8f);

        bumperGO.SetActive(false);

        cacheQuantity = newQuantity;
    }
}
