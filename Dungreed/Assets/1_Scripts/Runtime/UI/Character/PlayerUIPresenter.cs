using System;
using UnityEngine;

public class PlayerUIPresenter : MonoBehaviour
{
    private GameObject _player;

    public event Action<int, int> OnHealthChanged;
    public event Action<float> OnReload;
    public event Action<int, int> OnDashCountChanged;

    private void Awake()
    {
        _player = GameManager.Instance.Player;
    }

    private void OnEnable()
    {
        if (_player == null)
        {
            _player = GameManager.Instance.Player;
        }

        _player.GetComponent<PlayerController>().OnDashAction -= ChangeDashCount;
        _player.GetComponent<PlayerController>().OnDashAction += ChangeDashCount;
        _player.GetComponent<Health>().OnHealthChanged -= ChangeHealth;
        _player.GetComponent<Health>().OnHealthChanged += ChangeHealth;
        _player.GetComponentInChildren<WeaponHand>().OnReload -= Reload;
        _player.GetComponentInChildren<WeaponHand>().OnReload += Reload;
    }

    private void OnDisable()
    {
        if (_player == null)
        {
            return;
        }

        _player.GetComponent<PlayerController>().OnDashAction -= ChangeDashCount;
        _player.GetComponent<Health>().OnHealthChanged -= ChangeHealth;
        _player.GetComponentInChildren<WeaponHand>().OnReload -= Reload;
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
