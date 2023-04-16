using UnityEngine;

public class UIDoTween : MonoBehaviour
{
    public RectTransform Target;
    public RectTransform Start;
    public RectTransform End;

    public AnimationCurve Curve;

    [ShowOnly] public bool IsTweenEnd;

    public bool PlayOnAwake = false;
    public bool TriggerOn = false;

    public float TweenTime;

    private float _elapsedTime;

    private void Awake()
    {
        if(PlayOnAwake == true)
        {
            TriggerOn = true;
        }
        else
        {
            IsTweenEnd = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(TriggerOn)
        {
            IsTweenEnd = false;
            _elapsedTime = 0f;
            TriggerOn = false;
        }
        if(IsTweenEnd == false)
        {
            _elapsedTime += Time.deltaTime;
            Target.anchoredPosition = Vector2.Lerp(Start.anchoredPosition, End.anchoredPosition, Curve.Evaluate(_elapsedTime / TweenTime));
            if(_elapsedTime - 0.1f > TweenTime)
            {
                IsTweenEnd = true;
                _elapsedTime = 0f;
            }
        }
    }
}
