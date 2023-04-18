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
    [SerializeField] protected bool _isOnAwake;
    protected Collider2D _collision;
    protected BoxCollider2D _collider;
    [ShowOnly, SerializeField] protected TriggerState _state;

    public Collider2D Collision { get => _collision; }
    public BoxCollider2D Collider { get => _collider; }

    protected virtual void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;

        if(_isOnAwake == true)
        {
            OnTrigger();
        }
        else
        {
            OffTrigger();
        }

    }

    protected virtual void Start()
    {
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
        if (_state == TriggerState.Disable)
        {
            return;
        }

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
