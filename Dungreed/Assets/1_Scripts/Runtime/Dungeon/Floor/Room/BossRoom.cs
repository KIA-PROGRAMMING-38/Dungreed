using UnityEngine;

public class BossRoom : RoomBase
{
    [SerializeField] private InitPosition _startPosition;
    private BossBase    _boss;
    private BossHealthBar _bossHealthBar;

    private bool        _isBossCleared = false;
    private bool        _isBattleStart = false;

    public bool IsBattleStart { get { return _isBattleStart; } }

    public bool IsBossCleared 
    {
        get 
        { 
            return _isBossCleared;  
        } 
        set
        {
            _isBossCleared = value;
            if(IsBossCleared)
            {
                OnRoomClear?.Invoke();
            }
        } 
    }

    public override void Initialize()
    {
        base.Initialize();
        if(_boss == null)
        {
            _boss = this.GetComponentAllCheck<BossBase>();
            _bossHealthBar = this.GetComponentAllCheck<BossHealthBar>();
        }

        _bossHealthBar.SetOwnerHealth(_boss.GetComponentAllCheck<Health>());
        _bossHealthBar.gameObject.SetActive(false);
        _boss?.Initialize(this);
    }

    public override void OnRoomEnter()
    {
        _player.transform.position = _startPosition.Position;
        GameManager.Instance.CameraManager.SettingCamera(RoomBounds, _startPosition);
        _player.transform.position = _startPosition.Position;
        _bossHealthBar.gameObject.SetActive(true);
        _isBattleStart = true;
    }

    public override void OnRoomExit()
    {

    }

    public override void OnRoomStay()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isBattleStart = true;
        }
    }
}
