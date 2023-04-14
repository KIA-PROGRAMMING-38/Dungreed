using System.Collections;
using UnityEngine;

public class DelayTrigger : Trigger
{
    [SerializeField] protected float _delayTime;
    protected bool _delayTrigger;
    protected IEnumerator _delayInvokeCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        _delayInvokeCoroutine = DelayInvoke();
    }

    protected override void TriggerEnter()
    {
        StartCoroutine(_delayInvokeCoroutine);
    }

    protected override void TriggerExit()
    {
        _ExitAction?.Invoke();
    }

    private IEnumerator DelayInvoke()
    {
        while (true)
        {
            _state = TriggerState.Disable;
            yield return YieldCache.WaitForSeconds(_delayTime);
            _EnterAction?.Invoke();

            StopCoroutine(_delayInvokeCoroutine);
            yield return null;
        }
    }
}
