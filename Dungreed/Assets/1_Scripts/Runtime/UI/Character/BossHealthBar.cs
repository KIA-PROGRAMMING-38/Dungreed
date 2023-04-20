using UnityEngine;

public class BossHealthBar : ProgressBar
{
    [SerializeField] private Health _health;
    private float _currentHealthRatio;

    public void SetOwnerHealth(Health health)
    { 
        _health = health;
        _health.OnHealthChanged -= UpdateProgressBar;
        _health.OnHealthChanged += UpdateProgressBar;
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
