public class ImmediatelyTrigger : Trigger
{
    protected override void TriggerEnter()
    {
        _state = TriggerState.Disable;
        _EnterAction?.Invoke();
    }

    protected override void TriggerExit()
    {
        _ExitAction?.Invoke();
    }
}
