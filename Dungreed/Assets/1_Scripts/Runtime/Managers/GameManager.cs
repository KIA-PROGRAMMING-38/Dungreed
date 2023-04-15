using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField]
    public GameObject PlayerPrefab { get; private set; }

    private GameObject _player;

    public GameObject Player 
    { 
        get 
        { 
            if(_player == null)
            {
                _player = Instantiate(PlayerPrefab);
            }
            return _player;
        } 
    }
    
    public FxPooler FxPooler { get; private set; }

    public ProjectilePooler ProjectilePooler { get; private set; }

    public WeaponManager WeaponManager { get; private set; }

    public CameraManager CameraManager { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        FxPooler = GetComponentInChildren<FxPooler>();
        ProjectilePooler = GetComponentInChildren<ProjectilePooler>();
        WeaponManager = GetComponentInChildren<WeaponManager>();
        CameraManager = GetComponentInChildren<CameraManager>();
        _player = Instantiate(PlayerPrefab);
    }

}
