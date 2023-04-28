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
    private bool _isReloading;
    private float _reloadTime;
    private float _reloadElapsedTime;

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
        Vector2 pos = _cursor.transform.position;
        pos.x = _startPos;
        _cursor.transform.position = pos;
        _reloadTime = 0f;
        _base.gameObject.SetActive(false);
        _cursor.gameObject.SetActive(false);
        _isReloading = false;
        _reloadElapsedTime = 0f;
    }

    private void Update()
    {
        if (_isReloading)
        {
            _base.gameObject.SetActive(true);
            _cursor.gameObject.SetActive(true);

            _reloadElapsedTime += Time.deltaTime;
            Vector2 localPos = _cursor.localPosition;
            localPos.x = Utils.Math.Utility2D.EaseInBounce(_startPos, _endPos, _reloadElapsedTime / _reloadTime);
            _cursor.localPosition = localPos;

            if(_reloadElapsedTime > _reloadTime)
            {
                _reloadElapsedTime = 0f;
                GameManager.Instance.FxPooler.GetFx(_reloadFxPath, transform.position, Quaternion.identity);
                _base.gameObject.SetActive(false);
                _cursor.gameObject.SetActive(false);
                _isReloading = false;
            }
        }
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

    public override void UpdateProgressBar(float reloadTime)
    {
        _reloadTime = reloadTime;
        _isReloading = true;
    }

}
