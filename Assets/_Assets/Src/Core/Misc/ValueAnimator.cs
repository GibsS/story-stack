using UnityEngine;

public class ValueAnimatorStep {
    public float value;
    public float time;
    public float delay;
    public EasingFunction function;
    public ValueAnimatorStep next;
}

public class ValueAnimator : MonoBehaviour {

    [HideInInspector]

    public bool isAnimating = false;

    public float value = 0f;

    float lastValue = 0f;

    ValueAnimatorStep nextStep;

    float timeStart = 0f;

    public static ValueAnimator Create(GameObject obj, float value = 0) {
        ValueAnimator animator = obj.AddComponent<ValueAnimator>();

        animator.Setup(value);

        return animator;
    }

    public void Setup(float value) {
        this.value = value;
    }

    public void SetValue(float newValue) {

        enabled = true;
        isAnimating = true;

        value = newValue;
        lastValue = newValue;

        nextStep = null;
    }

    public void QueueAnimationWithSpeed(float target, float speed, float delay = 0, EasingFunction function = null) {

        enabled = true;
        isAnimating = true;

        QueueAnimation(target, speed == 0 ? 1 : Mathf.Abs(target - value) / speed, delay, function);
    }

    public void QueueAnimation(float targetValue, float timeToGetThere, float delay = 0, EasingFunction function = null) {

        enabled = true;
        isAnimating = true;

        ValueAnimatorStep newStep = new ValueAnimatorStep();
        newStep.value = targetValue;
        newStep.time = timeToGetThere;
        newStep.delay = delay;
        newStep.function = function;

        if (nextStep == null) {
            nextStep = newStep;
        } else {
            ValueAnimatorStep step = nextStep;
            while (step.next != null) {
                step = step.next;
            }
            step.next = newStep;
        }

        timeStart = Time.time;
    }

    public void ClearQueue() {

        nextStep = null;

        lastValue = value;
    }

    void Update() {

        if (nextStep == null) {
            isAnimating = false;
            enabled = false;
            return;
        }

        float delta = (Time.time - timeStart);

        if (delta > nextStep.delay) {

            delta -= nextStep.delay;
            delta /= nextStep.time;

            if (delta > 1f) {

                value = nextStep.value;
                lastValue = value;
                nextStep = nextStep.next;
                timeStart = Time.time;

            } else if (nextStep.function == null) {

                float interp = (-1f * Mathf.Cos(delta * Mathf.PI) + 1f) * 0.5f;

                value = Mathf.LerpUnclamped(lastValue, nextStep.value, interp);

            } else {

                value = Mathf.LerpUnclamped(lastValue, nextStep.value, nextStep.function(delta));
            }
        }

        isAnimating = true;

    }
}