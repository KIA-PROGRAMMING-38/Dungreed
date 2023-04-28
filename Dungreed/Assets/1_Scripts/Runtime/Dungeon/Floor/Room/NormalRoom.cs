using UnityEngine;

public class NormalRoom : RoomBase
{
    public override void Initialize()
    {
        base.Initialize();

        _enemies = GetComponentsInChildren<EnemyBase>();
        _info.EnemyCount = _enemies?.Length ?? 0;

        Health currentPlayerHealth = _player.GetComponent<Health>();

        foreach (EnemyBase enemy in _enemies)
        {
            enemy.OnDie -= AddEnemyDieCount;
            enemy.OnDie += AddEnemyDieCount;
            currentPlayerHealth.OnDie += OnPlayerDie;
        }
    }


    public override void OnRoomEnter()
    {

    }

    public override void OnRoomStay()
    {
    }

    public override void OnRoomExit()
    {

    }

    public override void OnPlayerDie()
    {
        Health currentPlayerHealth = _player.GetComponent<Health>();

        foreach (EnemyBase enemy in _enemies)
        {
            enemy.ReleaseTarget();
            currentPlayerHealth.OnDie -= OnPlayerDie;
        }
    }

    private void AddEnemyDieCount()
    {
        _info.EnemyDieCount++;
        if (_info.Clear == true)
        {
            OnRoomClear?.Invoke();
        }
    }
}
