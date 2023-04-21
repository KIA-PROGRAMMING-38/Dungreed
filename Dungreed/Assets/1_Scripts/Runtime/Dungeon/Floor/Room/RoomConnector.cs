using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class RoomConnector : MonoBehaviour
{
    [Header("Debug")]
    public bool ShowConnectedLine;

    [SerializeField] private RoomBase _owner;
    [SerializeField] private RoomBase _connectedRoom;
    [SerializeField] private InitPosition _initPos;

    [SerializeField] private Trigger _trigger;
    private ParticleSystem[] _connectorParticle;

    private static readonly int ID_OpenTrigger = Animator.StringToHash("Open");
    private Animator _anim;
    private BoxCollider2D _collider;
    private PlayerController _playerController;
    private UnityEvent<int> _customAction;

    public void Initialize()
    {
        _anim = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _connectorParticle = GetComponentsInChildren<ParticleSystem>();

        foreach (var particles in _connectorParticle)
        {
            particles.Stop();
        }

        _trigger = GetComponentInChildren<Trigger>();
        _trigger.Collider.isTrigger = true;
        _trigger.OffTrigger();
        _owner.OnRoomClear -= OnRoomClear;
        _owner.OnRoomClear += OnRoomClear;
    }

    private void OnDisable()
    {
        _owner.OnRoomClear -= OnRoomClear;
    }

    public void OnRoomClear()
    {
        foreach (var particles in _connectorParticle)
        {
            particles.Play();
        }
        _trigger.OnTrigger();
        if(_anim!= null)
        {
            _anim.SetTrigger(ID_OpenTrigger);
            _collider.enabled = false;
        }
           
    }

    public void ChangeRoomProcess()
    {
        GameManager.Instance.CameraManager.Effecter.PlayTransitionEffect(MoveToConnectedRoom);
    }

    private void MoveToConnectedRoom()
    {
        _owner.Floor.ChangeRoom(_connectedRoom);

        if (_connectedRoom is not BossRoom)
        { 
            GameManager.Instance.CameraManager.SettingCamera(_connectedRoom.RoomBounds, _initPos);
            GameManager.Instance.Player.transform.position = _initPos.Position;
        }


        if(_playerController == null)
        {
            _playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
        }

        _playerController.Anim.SetTrigger(_playerController.Id_IdleAnimationParameter);
        _playerController.Rig2D.velocity = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        if(ShowConnectedLine)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, _initPos.Position);
        }
    }
}
