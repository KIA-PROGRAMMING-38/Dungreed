using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    public GameObject CurrentPlayer { get; private set; }
    [SerializeField] private FloorBase[] _floors;
    [SerializeField] private FloorBase _startFloor;

    private int adventureTime = 0;
    private DungeonDieScene _dieScene;
    private FloorBase _currentFloor;
    public FloorBase CurrentFloor { get { return _currentFloor; } }

    private void Awake()
    {
        Debug.Assert(_floors.Count() != 0);
        _dieScene = GetComponent<DungeonDieScene>();
    }

    public void Start()
    {
        SetPlayer();

        foreach (FloorBase floor in _floors)
        {
            floor.Owner = this;
            floor.SetPlayer(CurrentPlayer);
            floor.Initialize();
        }

        ChangeFloor(_startFloor);
    }

    public void OnDisable()
    {
        if (CurrentPlayer != null)
        {
            Health playerHealth = CurrentPlayer?.GetComponent<Health>();
            playerHealth.OnDieWithSender -= PlayerDieProcess;
        }
    }

    public void SetPlayer()
    {
        CurrentPlayer = Instantiate(GameManager.Instance.PlayerPrefab);
        GameManager.Instance.Player = CurrentPlayer;

        UIBinder.Instance.BindPlayerUIPresenter(CurrentPlayer);

        Health playerHealth = CurrentPlayer.GetComponent<Health>();
        playerHealth.OnDieWithSender -= PlayerDieProcess;
        playerHealth.OnDieWithSender += PlayerDieProcess;
    }

    // �÷��̾ �׾��� �� ó�� ���ִ� �޼���
    public void PlayerDieProcess(GameObject sender)
    {
        _currentFloor.OnPlayerDie();
        adventureTime = (int)Time.timeSinceLevelLoad;
        var data = CurrentPlayer.GetComponent<PlayerData>();

        EnemyBase enemy = sender.GetComponent<EnemyBase>();
        BossBase boss = sender.GetComponent<BossBase>();
        if (enemy)
        {
            _dieScene.SetReason(enemy.Data.Name);
        }
        else if(boss)
        {
            _dieScene.SetReason(boss.EnemyData.Name);
        }
        

        // �÷��̾� ������ ����
        data.SaveData(adventureTime, 0);
        data.SavePlayerData();
        data.LoadSaveData();

        _dieScene.ProcessCutScene(null, ReturnToTown);
    }

    public void ReturnToTown()
    {
        SceneManager.LoadScene(1);
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
