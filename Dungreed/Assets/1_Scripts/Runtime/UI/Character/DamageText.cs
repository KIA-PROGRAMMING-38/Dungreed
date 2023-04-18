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
    private IEnumerator _effectCoroutine;

    public void Awake()
    {
        _text = GetComponent<Text>();
        _effectCoroutine = DamageTextEffectCoroutine();
    }

    public void SetOwner(ObjectPool<DamageText> owner) => _owner = owner;

    public void SetUp(DamageInfo damageInfo, Vector2 startPosition)
    {
        _text.text = damageInfo.Damage.ToString();

        _text.color = damageInfo.Type switch
        {
            DamageType.Normal => _defaultColor,
            DamageType.Critical => _criticalColor,
            _ => _defaultColor
        };

        _text.rectTransform.anchoredPosition = startPosition;
        _text.rectTransform.localScale = _defaultScale;
        StartCoroutine(_effectCoroutine);
    }

    private IEnumerator DamageTextEffectCoroutine()
    {
        while (true)
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

                yield return null;
            }
            transform.localScale = Vector3.one;
            _owner.Release(this);
            StopCoroutine(_effectCoroutine);
            yield return null;
        }
    }

}
