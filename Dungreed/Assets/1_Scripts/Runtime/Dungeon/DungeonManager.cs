using System;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] private FloorBase[] _floors;
    private FloorBase _currentFloor;
    private FloorBase _startFloor;
    public FloorBase CurrentFloor { get { return _currentFloor; } }
    public event Action OnPlayerDie;

    private void Awake()
    {
        // base.Awake();
        Debug.Assert(_floors.Count() == 0);
        foreach(FloorBase floor in _floors)
        {
            floor.Owner = this;
        }
    }

    public void PlayerDieProcess()
    {

    }

    public void ChangeFloor(FloorBase floor)
    {
        if(_currentFloor.Equals(floor)) return;
        if(_currentFloor == null)
        {
            _currentFloor = floor;
            _currentFloor.OnFloorEnter();
            return;
        }

        _currentFloor.OnFloorExit();
        _currentFloor = floor;
        _currentFloor.OnFloorEnter();
    }

}
