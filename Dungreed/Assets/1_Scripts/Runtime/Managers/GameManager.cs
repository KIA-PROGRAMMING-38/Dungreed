using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField]
    public GameObject PlayerPrefab { get; private set; }

    [field: SerializeField]
    public FxPooler FxPooler { get; private set; }
    [field: SerializeField]
    public WeaponDataManager WeaponDataManager { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    protected void OnDestroy()
    {
        FxPooler = null;
    }

}
