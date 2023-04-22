using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField]
    public GameObject PlayerPrefab { get; private set; }

    private GameObject _player;

    public GameObject Player 
    { 
        get 
        { 
            return _player;
        } 
    }
    
    public FxPooler FxPooler { get; private set; }

    public ProjectilePooler ProjectilePooler { get; private set; }

    public TextPooler DamageTextPooler { get; private set; }

    public WeaponManager WeaponManager { get; private set; }

    public CameraManager CameraManager { get; private set; }


    new protected void Awake()
    {
        base.Awake();
        FxPooler = GetComponentInChildren<FxPooler>();
        ProjectilePooler = GetComponentInChildren<ProjectilePooler>();
        DamageTextPooler = GetComponentInChildren<TextPooler>();

        WeaponManager = GetComponentInChildren<WeaponManager>();
        CameraManager = GetComponentInChildren<CameraManager>();
    }

    protected virtual void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    protected virtual void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        _player = Instantiate(PlayerPrefab);
        _player.SetActive(true);
    }

}
