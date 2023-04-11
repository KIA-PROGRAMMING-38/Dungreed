public class ImmediatelyTrigger : Trigger
{
    protected override void TriggerEnter()
    {
        _EnterAction?.Invoke();
        _state = TriggerState.Disable;
    }

    protected override void TriggerExit()
    {
        _ExitAction?.Invoke();
    }
}
