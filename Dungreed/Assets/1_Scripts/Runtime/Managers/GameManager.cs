using TMPro;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField]
    public GameObject PlayerPrefab { get; private set; }

    private GameObject _player;

    public GameObject Player { get { return _player; } set { _player = value; } }
    
    public FxPooler FxPooler { get; private set; }

    public ProjectilePooler ProjectilePooler { get; private set; }

    public TextPooler DamageTextPooler { get; private set; }

    public CameraManager CameraManager { get; private set; }


    new protected void Awake()
    {
        base.Awake();
        FxPooler = GetComponentInChildren<FxPooler>();
        ProjectilePooler = GetComponentInChildren<ProjectilePooler>();
        DamageTextPooler = GetComponentInChildren<TextPooler>();

        CameraManager = GetComponentInChildren<CameraManager>();
    }
}
