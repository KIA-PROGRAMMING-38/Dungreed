using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : ProgressBar
{
    [SerializeField] private Health _health;
    private Image[] _images;
    public Image[] Images { get { return _images; } }
    private float _currentHealthRatio;

    public void SetOwnerHealth(Health health)
    { 
        _health = health;
        _health.OnHealthChanged -= UpdateProgressBar;
        _health.OnHealthChanged += UpdateProgressBar;
    }

    public void Awake()
    {
        _images = GetComponentsInChildren<Image>();
    }

    public void FadeOutImages()
    {
        if(_images != null )
        {
            foreach(Image image in _images)
            {
                image.enabled = false;
            }
        }
    }

    public void FadeInImages()
    {
        if (_images != null)
        {
            foreach (Image image in _images)
            {
                image.enabled = true;
            }
        }
    }

    public void OnEnable()
    {
        if(_health != null )
        {
            _health.OnHealthChanged -= UpdateProgressBar;
            _health.OnHealthChanged += UpdateProgressBar;
        }
    }

    public void OnDisable()
    {
        if (_health != null)
        {
            _health.OnHealthChanged -= UpdateProgressBar;
        }
    }

    public override void UpdateProgressBar(int cur, int max)
    {
        _currentHealthRatio = Mathf.Clamp01(cur / (float)max);
        _progressBarImage.fillAmount = _currentHealthRatio;
    }
}
