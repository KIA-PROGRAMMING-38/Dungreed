using System.Collections;
using UnityEngine;

public class ReloadBar : MonoBehaviour
{
    public Transform Player;

    [SerializeField] private string _reloadFxPath;
    [SerializeField] private RectTransform _base;
    [SerializeField] private RectTransform _cursor;

    private Vector3 pos = Vector3.up * 1.5f;
    private float _startPos;
    private float _endPos;

    private void Awake()
    {
        float sizeX = _base.rect.width / 2f;
        _startPos = -sizeX;
        _endPos = sizeX;
        _base.gameObject.SetActive(false);
        _cursor.gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

    void LateUpdate()
    {
        transform.position = Player.position + pos;
    }

    public void Reload(float reloadTime)
    {
        StartCoroutine(ReloadCoroutine(reloadTime));
    }

    public IEnumerator ReloadCoroutine(float reloadTime)
    {
        _base.gameObject.SetActive(true);
        _cursor.gameObject.SetActive(true);

        float t = 0f;
        while(t-0.1f < reloadTime)
        {
            t += Time.deltaTime;
            Vector2 localPos = _cursor.localPosition;
            localPos.x = Utils.Math.Utility2D.EaseInBounce(_startPos, _endPos, t/reloadTime);
            _cursor.localPosition = localPos;
            yield return null;
        }

        GameManager.Instance.FxPooler.GetFx(_reloadFxPath, transform.position, Quaternion.identity);
        _base.gameObject.SetActive(false);
        _cursor.gameObject.SetActive(false);
    }
}
