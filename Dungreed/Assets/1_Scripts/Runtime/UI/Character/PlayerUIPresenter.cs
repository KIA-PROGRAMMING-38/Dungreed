using System;
using UnityEngine;

public class PlayerUIPresenter : MonoBehaviour
{
    private GameObject _player;

    private PlayerController _playerController;
    private Health _health;
    private WeaponHand _weaponHand;

    public event Action<int, int> OnHealthChanged;
    public event Action<float> OnReload;
    public event Action<int, int> OnDashCountChanged;

    public void Bind(GameObject player)
    {
        _player = player;

        _playerController = _player.GetComponent<PlayerController>();
        _health = _player.GetComponent<Health>();
        _weaponHand = _player.GetComponentInChildren<WeaponHand>();

        _playerController.OnDashAction -= ChangeDashCount;
        _playerController.OnDashAction += ChangeDashCount;

        _health.OnHealthChanged -= ChangeHealth;
        _health.OnHealthChanged += ChangeHealth;

        _weaponHand.OnReload -= Reload;
        _weaponHand.OnReload += Reload;
    }

    private void OnDisable()
    {
        if (_player != null)
        {
            _playerController.OnDashAction -= ChangeDashCount;
            _health.OnHealthChanged -= ChangeHealth;
            _weaponHand.OnReload -= Reload;
        }
    }

    public void ChangeHealth(int cur, int max)
    {
        OnHealthChanged?.Invoke(cur, max);
    }

    public void ChangeDashCount(int cur, int max)
    {
        OnDashCountChanged?.Invoke(cur, max);
    }

    public void Reload(float reloadTime)
    {
        OnReload?.Invoke(reloadTime);
    }

}
