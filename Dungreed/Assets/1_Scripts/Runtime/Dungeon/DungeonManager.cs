using System;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private FloorBase[] _floors;
    [SerializeField] private FloorBase _startFloor;
    private FloorBase _currentFloor;
    public FloorBase CurrentFloor { get { return _currentFloor; } }
    public event Action OnPlayerDie;

    private void Awake()
    {
        Debug.Assert(_floors.Count() != 0);
        foreach (FloorBase floor in _floors)
        {
            floor.Owner = this;
            floor.Initialize();
        }
    }

    public void Start()
    {
        ChangeFloor(_startFloor);
    }

    // 플레이어가 죽었을 때 처리 해주는 메서드
    public void PlayerDieProcess()
    {

    }

    public void ChangeFloor(FloorBase floor)
    {
        if(_currentFloor == null)
        {
            _currentFloor = floor;
            _currentFloor.OnFloorEnter();
            return;
        }

        if(_currentFloor.Equals(floor)) return;

        _currentFloor.OnFloorExit();
        _currentFloor = floor;
        _currentFloor.OnFloorEnter();
    }

}
