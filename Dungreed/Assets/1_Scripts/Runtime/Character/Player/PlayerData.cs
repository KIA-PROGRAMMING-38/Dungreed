using UnityEngine;

public class PlayerData : MonoBehaviour
{
    private static readonly string DataFolderName = "Data";
    private static readonly string PlayerStatusFileName = "PlayerStatus";
    private static readonly string PlayerSaveDataFileName = "PlayerSaveData";

    private PlayerStatus _status = new PlayerStatus();
    private PlayerSaveData _saveData = new PlayerSaveData();

    [SerializeField] private InventoryObject _inventory;
    public InventoryObject Inventory { get { return _inventory; } }

    public PlayerStatus Status { get { return _status; } }
    public int Gold { get { return _saveData?.Gold ?? 0; } set { _saveData.Gold += value; } }

    public void SaveData(int adventureTime, int gold)
    {
        _saveData.TotalPlayTimeBySeconds += adventureTime;
        _saveData.Gold = gold;
    }

    public void Initialize()
    {
        LoadStatus();
        LoadSaveData();
    }

    public void LoadStatus()
    {
        JsonDataManager.Instance.LoadFromJson<PlayerStatus>(DataFolderName, PlayerStatusFileName, out _status);
        CurrentDashCount = _status.MaxDashCount;
    }

    public void LoadSaveData()
    {
        if(false == JsonDataManager.Instance.LoadFromJson<PlayerSaveData>(DataFolderName, PlayerSaveDataFileName, out _saveData))
        {
            SavePlayerData();
        }
    }

    public void SavePlayerStatus()
    {
        JsonDataManager.Instance.SaveToJson(DataFolderName, PlayerStatusFileName, _status);
    }

    public void SavePlayerData()
    {
        JsonDataManager.Instance.SaveToJson(DataFolderName, PlayerSaveDataFileName, _saveData);
    }

    //private void Awake()
    //{
    //    LoadStatus();
    //}


    public const float DEFAULT_MOVE_SPEED = 7;
    public const float DEFAULT_JUMP_FORCE = 14;
    public const float DEFAULT_JUMP_TIME = 0.15f;
    public const float DEFAULT_DASH_COUNT_INTERVAL = 2f;
    public const float DEFAULT_DASH_POWER = 20f;
    public const float DEFAULT_DASH_TIME = 0.2f;
    public const float MOVE_FX_SPAWN_INTERVAL = 0.3f;

    public float JumpForce { get { return DEFAULT_JUMP_FORCE + _status.JumpPower; } }
    public float MoveSpeed { get { return DEFAULT_MOVE_SPEED + _status.MoveSpeed; } }
    public int   MaxDashCount { get { return _status.MaxDashCount; } }
    public int   CurrentDashCount { get; set; }
}
