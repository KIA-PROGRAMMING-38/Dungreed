using UnityEngine;

public class NormalRoom : RoomBase
{
    public override void Initialize()
    {
        base.Initialize();

        _enemies = GetComponentsInChildren<EnemyBase>();
        _info.EnemyCount = _enemies?.Length ?? 0;

        foreach (EnemyBase enemy in _enemies)
        {
            enemy.OnDie -= AddEnemyDieCount;
            enemy.OnDie += AddEnemyDieCount;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        foreach (EnemyBase enemy in _enemies)
        {
            enemy.OnDie -= AddEnemyDieCount;
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

    private void AddEnemyDieCount()
    {
        _info.EnemyDieCount++;
        Debug.Log(_info.EnemyDieCount);
        if (_info.Clear == true)
        {
            OnRoomClear?.Invoke();
        }
    }
}
