using System.Collections;
using UnityEngine;

public class DelayTrigger : Trigger
{
    [SerializeField] protected float _delayTime;
    protected bool _delayTrigger;
    protected IEnumerator DelayInvokeCoroutine;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        DelayInvokeCoroutine = DelayInvoke();
    }

    protected override void TriggerEnter()
    {
        StartCoroutine(DelayInvokeCoroutine);
    }

    protected override void TriggerExit()
    {
        _ExitAction?.Invoke();
    }

    private IEnumerator DelayInvoke()
    {
        _state = TriggerState.Disable;
        yield return YieldCache.WaitForSeconds(_delayTime);
        _EnterAction?.Invoke();
    }
}
