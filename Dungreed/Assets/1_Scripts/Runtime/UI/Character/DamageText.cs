using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{

    [SerializeField] private float _lifeTime;
    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _defaultScale;
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _criticalColor;

    private ObjectPool<DamageText> _owner;
    private Text _text;

    public void Awake()
    {
        _text = GetComponent<Text>();
    }

    public void SetOwner(ObjectPool<DamageText> owner) => _owner = owner;

    public void SetUp(string damage, DamageType damageType, Vector2 startPosition)
    {
        _text.text = damage;

        _text.color = damageType switch
        {
            DamageType.Normal => _defaultColor,
            DamageType.Critical => _criticalColor,
            _ => _defaultColor
        };

        _text.rectTransform.anchoredPosition = startPosition;
        _text.rectTransform.localScale = _defaultScale;
        DamageTextEffectTask().Forget();
    }

    private async UniTaskVoid DamageTextEffectTask()
    {
        float t = 0f;
        Color col = _text.color;
        while (t < _lifeTime)
        {
            t += Time.deltaTime;
            _text.rectTransform.anchoredPosition += Vector2.up * (_speed * Time.deltaTime);

            float alpha = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, t / _lifeTime));
            col.a = alpha;
            _text.color = col;

            await UniTask.Yield();
        }
        transform.localScale = Vector3.one;
        _owner.Release(this);
    }

}
