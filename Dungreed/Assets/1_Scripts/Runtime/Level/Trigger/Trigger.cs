using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Trigger : MonoBehaviour
{
    public enum TriggerState
    {
        Enable,
        Disable,
    }

    [SerializeField] protected LayerMask _triggerMask;
    [SerializeField] protected UnityEvent _EnterAction;
    [SerializeField] protected UnityEvent _ExitAction;

    protected Collider2D _collision;
    protected BoxCollider2D _collider;
    protected TriggerState _state;

    public Collider2D Collision { get => _collision; }

    protected virtual void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Start()
    {
        OnTrigger();
        _collider.isTrigger = true;
    }

    protected abstract void TriggerEnter();
    protected abstract void TriggerExit();

    public void OnTrigger()
    {
        _state = TriggerState.Enable;
    }

    public void OffTrigger()
    {
        _state = TriggerState.Disable;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_state == TriggerState.Disable) return;

        if (true == Globals.LayerMask.CompareMask(collision.gameObject.layer, _triggerMask))
        {
            _collision = collision;
            TriggerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (true == Globals.LayerMask.CompareMask(collision.gameObject.layer, _triggerMask))
        {
            _collision = null;
            TriggerExit();
        }
    }
}
