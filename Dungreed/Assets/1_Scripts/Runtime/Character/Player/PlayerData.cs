using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public PlayerStatus _status = new PlayerStatus();

    public void Awake()
    {
        JsonDataManager.Instance.LoadFromJson<PlayerStatus>("Data", "PlayerStatus", out _status);
        CurrentDashCount = _status.MaxDashCount;
    }
    public void LoadStatus()
    {
    }

    public void SaveStatus()
    {
        //Debug.Log("Save");
        //Debug.Log($"{Application.dataPath} /TestJson.json");
        //Debug.Log($"{JsonUtility.ToJson(_status)}");
        _status.MoveSpeed++;
        JsonDataManager.Instance.SaveToJson(null, "TestJson", _status);
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
    public int   CurrentDashCount = 2;
}
