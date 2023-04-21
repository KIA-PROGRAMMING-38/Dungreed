using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DungeonManager : MonoBehaviour
{
    public Image test;
    public GameObject CurrentPlayer { get; private set; }
    [SerializeField] private FloorBase[] _floors;
    [SerializeField] private FloorBase _startFloor;

    private int adventureTime = 0;
    private FloorBase _currentFloor;
    public FloorBase CurrentFloor { get { return _currentFloor; } }

    private void Awake()
    {
        Debug.Assert(_floors.Count() != 0);
        CurrentPlayer = GameManager.Instance.Player;
    }

    public void Start()
    {
        if (CurrentPlayer == null)
            CurrentPlayer = GameManager.Instance.Player;

        foreach (FloorBase floor in _floors)
        {
            floor.Owner = this;
            floor.SetPlayer(CurrentPlayer);
            floor.Initialize();
        }

        Health playerHealth = CurrentPlayer.GetComponent<Health>();
        playerHealth.OnDie -= PlayerDieProcess;
        playerHealth.OnDie += PlayerDieProcess;
        ChangeFloor(_startFloor);
    }

    public void OnDisable()
    {
        if (CurrentPlayer != null)
        {
            Health playerHealth = CurrentPlayer?.GetComponent<Health>();
            playerHealth.OnDie -= PlayerDieProcess;
        }
    }

    // 플레이어가 죽었을 때 처리 해주는 메서드
    public void PlayerDieProcess()
    {
        _currentFloor.OnPlayerDie();
        adventureTime = (int)Time.timeSinceLevelLoad;
        var data =CurrentPlayer.GetComponent<PlayerData>();

        // 플레이어 데이터 갱신
        data.SaveData(adventureTime, 0);
        data.SavePlayerData();
        data.LoadSaveData();
    }

    public void DungeonToTown()
    {

    }

    public void TownToDungeon()
    {

    }


    public void ChangeFloor(FloorBase floor)
    {
        if (_currentFloor == null)
        {
            _currentFloor = floor;
            _currentFloor.OnFloorEnter();
            return;
        }

        if (_currentFloor.Equals(floor)) return;

        _currentFloor.OnFloorExit();
        _currentFloor = floor;
        _currentFloor.OnFloorEnter();
    }

}
