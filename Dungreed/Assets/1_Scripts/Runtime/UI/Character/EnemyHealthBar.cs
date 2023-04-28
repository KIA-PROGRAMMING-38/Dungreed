using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : ProgressBar
{
    public enum PositionPivot
    {
        ColliderBound,
        Transform,
    }

    [SerializeField] private PositionPivot pivot;
    [SerializeField] private Image _baseImage;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Transform _owner;

    private Health _health;
    private Collider2D _collider;
    private SpriteRenderer _spriteRenderer;

    private float _currentHealthRatio;

    private Vector3 _uiPos;

    private void Awake()
    {
        _health = _owner.GetComponent<Health>();
        _collider = _owner.GetComponent<Collider2D>(); 
        _spriteRenderer = _owner.GetComponent<SpriteRenderer>(); 

    }

    private void Start()
    {
        _baseImage.enabled = false;
        _backgroundImage.enabled = false;

        _uiPos = Vector3.down * 0.25f;
        _progressBarImage.fillAmount = 1f;
        _currentHealthRatio = 1f;
        _progressBarImage.fillAmount = _currentHealthRatio;

        FadeOutProgressBarImage();
    }

    private void Update()
    {
        transform.position = pivot switch
        {
            PositionPivot.ColliderBound 
            => new Vector2(_collider.bounds.center.x, _collider.bounds.min.y) + (Vector2)_uiPos,
            PositionPivot.Transform => (_owner.transform.position + _uiPos),
            _ => Vector2.zero
        };

        Vector3 newScale = _owner.localScale;
        if (newScale.x == -1) newScale.x = -1f;
        else newScale.x = 1f;
        transform.localScale = newScale;
    }

    private void FadeOutProgressBarImage()
    {
        _baseImage.enabled = false;
        _backgroundImage.enabled = false;
        _progressBarImage.enabled = false;
    }

    private void FadeInProgressBarImage()
    {
        _baseImage.enabled = true;
        _backgroundImage.enabled = true;
        _progressBarImage.enabled = true;
    }

    private void OnEnable()
    {
        _health.OnHealthChanged -= UpdateProgressBar;
        _health.OnHealthChanged += UpdateProgressBar;
    }

    public override void UpdateProgressBar(int cur, int max)
    {
        FadeInProgressBarImage();
        _currentHealthRatio = Mathf.Clamp01(cur / (float)max);
        _progressBarImage.fillAmount = _currentHealthRatio;
    }

}
