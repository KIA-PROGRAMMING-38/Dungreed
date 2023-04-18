using UnityEngine;
using UnityEngine.Pool;

public class TextPooler : MonoBehaviour
{
    [SerializeField] GameObject _origin;
    ObjectPool<DamageText> _pool;
    [SerializeField] Canvas _canvas;

    private void Awake()
    {
        _pool = new ObjectPool<DamageText>(CreateAction, GetAction, ReleaseAction, DestroyAction, true, 10, 100);
    }

    public void PopupDamage(DamageInfo damageInfo, Vector2 startPosition)
    {
        DamageText damageText = _pool.Get();
        damageText.SetUp(damageInfo, startPosition);
    }

    public DamageText CreateAction()
    {
        var pooledObj = Instantiate(_origin, _canvas.transform).GetComponent<DamageText>();
        pooledObj.SetOwner(_pool);
        pooledObj.gameObject.SetActive(false);
        return pooledObj;
    }

    public void GetAction (DamageText text)
    {
        text.gameObject.SetActive(true);
    }

    public void ReleaseAction(DamageText text)
    {
        text.gameObject.SetActive(false);
    }

    public void DestroyAction(DamageText text)
    {
    }
}
