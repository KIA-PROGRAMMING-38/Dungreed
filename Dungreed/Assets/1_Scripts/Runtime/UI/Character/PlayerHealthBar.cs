using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : ProgressBar
{
    [SerializeField] PlayerUIPresenter _presenter;
    [SerializeField] private Image _decreaseProgressBarImage;
    [SerializeField] private float _decreaseHealthTime;
    [SerializeField] private TextMeshProUGUI _healhText;

    private StringBuilder _healthTextBuilder;
    private IEnumerator _healthChangeCoroutine;

    private float _currentHealthRatio = 1f;

    private void Awake()
    {
        _healthTextBuilder = new StringBuilder();
        _decreaseHealthTime = 1f;
        _progressBarImage.fillAmount = 1f;
        _progressBarImage.fillAmount = _decreaseProgressBarImage.fillAmount;

        _healthChangeCoroutine = HealthChangeCoroutine();
    }

    private void OnEnable()
    {
        _presenter.OnHealthChanged -= UpdateProgressBar;
        _presenter.OnHealthChanged += UpdateProgressBar;
    }

    private void OnDisable()
    {
        _presenter.OnHealthChanged -= UpdateProgressBar;
    }

    public void UpdateHealthText(int cur, int max)
    {
        _healthTextBuilder.Clear();
        _healthTextBuilder.Append($"{cur} / {max}");
        _healhText.text = _healthTextBuilder.ToString();
    }
  
    public override void UpdateProgressBar(int cur, int max)
    {
        _currentHealthRatio = Mathf.Clamp01(cur/(float)max);
        StartCoroutine(_healthChangeCoroutine);
        UpdateHealthText(cur, max);
    }

    IEnumerator HealthChangeCoroutine()
    {
        while (true)
        {
            float t = 0f;
            float startRatio = _currentHealthRatio;
            float progressFillAmount = _decreaseProgressBarImage.fillAmount;
            _progressBarImage.fillAmount = _currentHealthRatio;

            while (t-0.1f< _decreaseHealthTime)
            {
                if(startRatio != _currentHealthRatio) 
                {
                    t = 0f; 
                    startRatio = _currentHealthRatio;
                    _progressBarImage.fillAmount = _currentHealthRatio;
                    progressFillAmount = _decreaseProgressBarImage.fillAmount;
                }
                t += Time.deltaTime;
                _decreaseProgressBarImage.fillAmount = Mathf.Lerp(progressFillAmount, _currentHealthRatio, t/_decreaseHealthTime);
                yield return null;
            }
            StopCoroutine(_healthChangeCoroutine);
            yield return null;
        }
    }
}
