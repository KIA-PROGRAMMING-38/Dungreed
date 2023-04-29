using UnityEngine;

public class PlayerReloadBar : ProgressBar
{
    [SerializeField] private RectTransform _base;
    public Transform Owner;

    private WeaponHand _hand;

    [SerializeField] private string _reloadFxPath;
    private RectTransform _cursor;

    private Vector3 pos = Vector3.up * 1.5f;
    private float _startPos;
    private float _endPos;

    private void Start()
    {
        float sizeX = _base.rect.width / 2f;
        _startPos = -sizeX;
        _endPos = sizeX;
        _cursor = _progressBarImage.rectTransform;

        _base.gameObject.SetActive(false);
        _cursor.gameObject.SetActive(false);

        _hand = Owner.GetComponentInChildren<WeaponHand>();
        _hand.OnReload -= UpdateProgressBar;
        _hand.OnReload += UpdateProgressBar;
        _hand.OnWeaponChanged -= OffReloadBar;
        _hand.OnWeaponChanged += OffReloadBar;
    }

    private void OnDisable()
    {
        _hand.OnReload -= UpdateProgressBar;
        _hand.OnWeaponChanged -= OffReloadBar;
    }

    public void OffReloadBar()
    {
        Vector2 localPos = _cursor.localPosition;
        localPos.x = _startPos;
        _cursor.localPosition = localPos;
        _base.gameObject.SetActive(false);
        _cursor.gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        transform.position = Owner.position + pos;
        float playerDirection = Mathf.Sign(Owner.localScale.x);
        Vector3 newScale = Vector3.one;
        if (playerDirection == -1f)
        {
            newScale.x *= playerDirection;
            transform.localScale = newScale;
        }
        transform.localScale = newScale;
    }

    public override void UpdateProgressBar(float ratio)
    {
        _base.gameObject.SetActive(true);
        _cursor.gameObject.SetActive(true);

        Vector2 localPos = _cursor.localPosition;
        localPos.x = Utils.Math.Utility2D.EaseInBounce(_startPos, _endPos, ratio);
        _cursor.localPosition = localPos;

        if (ratio >= 1f)
        {
            GameManager.Instance.FxPooler.GetFx(_reloadFxPath, transform.position, Quaternion.identity);
            OffReloadBar();
        }
    }

}
