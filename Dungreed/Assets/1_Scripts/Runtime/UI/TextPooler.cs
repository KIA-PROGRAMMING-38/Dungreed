using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TextPooler : MonoBehaviour
{
    [SerializeField] private GameObject _origin;
    private ObjectPool<DamageText> _pool;
    [SerializeField] private Canvas _canvas;

    private Dictionary<int, string> _damageStrings;

    private void Awake()
    {
        _pool = new ObjectPool<DamageText>(CreateAction, GetAction, ReleaseAction, DestroyAction, true, 10, 100);
        _damageStrings = new Dictionary<int, string>();
    }

    public void PopupDamage(DamageInfo damageInfo, Vector2 startPosition)
    {
        DamageText damageText = _pool.Get();
        damageText.SetUp(GetDamageString(damageInfo.Damage), damageInfo.Type, startPosition);
    }

    private string GetDamageString(int damage)
    {
        if (_damageStrings.TryGetValue(damage, out string str) == false)
        {
            str = damage.ToString();
            return str;
        }

        return str;
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
