using UnityEngine;

public class UIBinder : Singleton<UIBinder>
{
    private GameObject _player;

    [SerializeField] private PlayerUIPresenter _playerUiPresenter;
    [SerializeField] private BossRoomPresenter _bossRoomPresenter;

    public PlayerUIPresenter PlayerUIPresenter { get { return _playerUiPresenter; } } 
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

}
