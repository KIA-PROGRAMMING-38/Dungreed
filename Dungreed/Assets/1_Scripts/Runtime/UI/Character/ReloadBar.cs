using System.Collections;
using UnityEngine;

public class ReloadBar : ProgressBar
{
    [SerializeField] private RectTransform _base;
    public Transform Owner;

    [SerializeField] private string _reloadFxPath;
    private RectTransform _cursor;

    private Vector3 pos = Vector3.up * 1.5f;
    private float _startPos;
    private float _endPos;

    private void Awake()
    {
        float sizeX = _base.rect.width / 2f;
        _startPos = -sizeX;
        _endPos = sizeX;
        _cursor = _progressBarImage.rectTransform;

        _base.gameObject.SetActive(false);
        _cursor.gameObject.SetActive(false);
        
    }
    // TODO 

    void LateUpdate()
    {
        transform.position = Owner.position + pos;
        float playerDirection = Mathf.Sign(Owner.localScale.x);
        Vector3 newScale = Vector3.one;
        if(playerDirection == -1f)
        {
            newScale.x *= playerDirection;
            transform.localScale = newScale;
        }
        transform.localScale = newScale;
    }

    public override void UpdateProgressBar(float reloadTime)
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
