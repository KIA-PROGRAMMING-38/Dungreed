using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;

public class DelayTrigger : Trigger
{
    [SerializeField] protected float _delayTime;
    protected bool _delayTrigger;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void TriggerEnter()
    {
        DelayInvoke().Forget();
    }

    protected override void TriggerExit()
    {
        _ExitAction?.Invoke();
    }

    private async UniTaskVoid DelayInvoke()
    {
        _state = TriggerState.Disable;
        await UniTask.Delay((int)(1000 * _delayTime));
        _EnterAction?.Invoke();
    }
}
