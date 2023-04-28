using UnityEngine;

public class UIBinder : Singleton<UIBinder>
{
    private GameObject _player;

    [SerializeField] private PlayerUIPresenter _playerUiPresenter;
    [SerializeField] private BossRoomPresenter _bossRoomPresenter;
    [SerializeField] private QuickSlot _quickSlot;

    public PlayerUIPresenter PlayerUIPresenter { get { return _playerUiPresenter; } } 
    public QuickSlot QuickSlot { get { return _quickSlot; } }
    public BossRoomPresenter BossRoomPresenter{ get { return _bossRoomPresenter; } } 

    public void SetPlayer(GameObject player) => _player = player;

    public void BindPlayerUIPresenter(GameObject player)
    {
        SetPlayer(player);
        var controller = player.GetComponent<PlayerController>();
        _playerUiPresenter.Bind(_player);
        controller.Initialize();
    }

    public void BindBossRoomPresenter(BossBase boss, BossRoom bossRoom)
    {
        _bossRoomPresenter.Bind(boss, bossRoom);
        _bossRoomPresenter.HealthBar.FadeOutImages();
    }

    public void BindQuickSlot(WeaponHand hand)
    {
        _quickSlot.SetPlayerHand(hand);
    }
}
