using UnityEngine;
using UnityEngine.UI;

public class ScrollUIImage : MonoBehaviour
{
    public float moveSpeed;

    [SerializeField] private RectTransform[] _trasnforms;
    public Vector2 startPosition;
    public Vector2 endPosition;
    private float _width;
    public float _halfWidth;
    private float _minX;
    [SerializeField] 
    private float _posInterval = 0f;

    void Start()
    {
        _width = _trasnforms[1].rect.width;
        _halfWidth = _width * 0.5f;
        _minX = startPosition .x - _width;
        Initialize();
    }

    public void Initialize()
    {
        for(int i = 0; i < _trasnforms.Length; ++i)
        {
            float x = -_halfWidth + (_width * i) + _posInterval;
            Vector2 pos = new Vector2(x, 0f);
            _trasnforms[i].anchoredPosition = pos;
        }
        endPosition = _trasnforms[^1].anchoredPosition;
        endPosition.x -= _halfWidth;
    }

    public void UpdateRectTransformPosition()
    {
        for (int i = 0; i < _trasnforms.Length; ++i)
        {
            _trasnforms[i].anchoredPosition += Vector2.left * (Time.fixedDeltaTime * moveSpeed);
            if (_trasnforms[i].anchoredPosition.x <= _minX)
            {
                _trasnforms[i].anchoredPosition = new Vector2(endPosition.x + _width, 0f);
            }
        }
    }

    void FixedUpdate()
    {
        UpdateRectTransformPosition();
    }
}
