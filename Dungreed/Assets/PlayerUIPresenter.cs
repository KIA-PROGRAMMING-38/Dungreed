using UnityEngine;

public class PlayerUIPresenter : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Health _playerHealth;
    [SerializeField] private WeaponHand _playerHand;

    [SerializeField] private ProgressBar _playerHealthBar;
    [SerializeField] private ProgressBar _reloadBar;

    void Start()
    {
        _player = GameManager.Instance.Player;

        _playerHealth = _player.GetComponentInChildren<Health>();
        _playerHand = _player.GetComponentInChildren<WeaponHand>();

        _reloadBar = _player.GetComponentInChildren<ReloadBar>();

        SubscribePlayerEvent();
    }

    private void OnEnable()
    {
        if(_player == null) return;
        SubscribePlayerEvent();
    }

    private void OnDisable()
    {
        UnsubscribePlayerEvent();
    }

    void SubscribePlayerEvent()
    {
        _playerHealth.OnHealthChanged -= _playerHealthBar.UpdateProgressBar;
        _playerHealth.OnHealthChanged += _playerHealthBar.UpdateProgressBar;

        _playerHand.OnReload -= _reloadBar.UpdateProgressBar;
        _playerHand.OnReload += _reloadBar.UpdateProgressBar;
    }

    void UnsubscribePlayerEvent()
    {
        _playerHealth.OnHealthChanged -= _playerHealthBar.UpdateProgressBar;
        _playerHand.OnReload -= _reloadBar.UpdateProgressBar;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
