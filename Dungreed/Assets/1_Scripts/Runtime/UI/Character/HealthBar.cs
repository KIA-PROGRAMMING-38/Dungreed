using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : ProgressBar
{
    [SerializeField] private Image _decreaseProgressBarImage;
    [SerializeField] private float _decreaseHealthTime;
    [SerializeField] private Health _health;

    private IEnumerator _healthChangeCoroutine;

    private int _maxHealth;
    private int _currentHealth;

    private float _currentHealthRatio = 1f;
    
    void Start()
    {
        _decreaseHealthTime = 1f;
        _progressBarImage.fillAmount = 1f;
        _progressBarImage.fillAmount = _decreaseProgressBarImage.fillAmount;
        _maxHealth = _health.MaxHp;
        _currentHealth = _health.CurrentHp;

        _healthChangeCoroutine = HealthChangeCoroutine();
        _health.OnHealthChanged += UpdateProgressBar;

        SetHealthBar(_currentHealth / (float)_maxHealth);
        StartCoroutine(_healthChangeCoroutine);
    }

    public void SetHealthBar(float ratio)
    {
        _progressBarImage.fillAmount = ratio;
        _decreaseProgressBarImage.fillAmount = ratio;
        _currentHealthRatio = ratio;
    }

    IEnumerator HealthChangeCoroutine()
    {
        while (true)
        {
            float t = 0f;
            Debug.Log("CoroutineStart");
            // 코루틴 시작 비율 저장
            float startRatio = _currentHealthRatio;
            float progressFillAmount = _decreaseProgressBarImage.fillAmount;
            _progressBarImage.fillAmount = _currentHealthRatio;

            while (t-0.1f< _decreaseHealthTime)
            {
                if(startRatio != _currentHealthRatio) 
                {
                    Debug.Log("Coroutine 초기화");
                    t = 0f; 
                    startRatio = _currentHealthRatio;
                    _progressBarImage.fillAmount = _currentHealthRatio;
                    progressFillAmount = _decreaseProgressBarImage.fillAmount;
                }
                t += Time.deltaTime;
                _decreaseProgressBarImage.fillAmount = Mathf.Lerp(progressFillAmount, _currentHealthRatio, t/_decreaseHealthTime);
                yield return null;
            }
            Debug.Log("HealthChange Coroutine Over");
            StopCoroutine(_healthChangeCoroutine);
            yield return null;
        }

    }

    public override void UpdateProgressBar(float ratio)
    {
        _currentHealthRatio = ratio;
        StartCoroutine(_healthChangeCoroutine);
    }
}
