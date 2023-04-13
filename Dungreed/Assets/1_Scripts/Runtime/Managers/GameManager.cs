using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField]
    public GameObject PlayerPrefab { get; private set; }

    // test
    public LevelBounds lev;
    public GameObject Player;
    public Transform InitPos;

    [field: SerializeField]
    public FxPooler FxPooler { get; private set; }
    [field: SerializeField]
    public ProjectilePooler ProjectilePooler { get; private set; }
    [field: SerializeField]
    public WeaponManager WeaponManager { get; private set; }
    [field: SerializeField]
    public CameraEffectManager CameraEffectManager { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        Player = Instantiate(PlayerPrefab);
        Player.transform.position = InitPos.position;
        Player.GetComponent<BaseController>().SetBounds(lev);
    }

    protected void OnDestroy()
    {
        FxPooler = null;
        WeaponManager = null;
    }

}
