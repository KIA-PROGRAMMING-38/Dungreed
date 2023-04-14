using UnityEngine;

public class PlayerDashBar : ProgressBar
{
    [SerializeField] PlayerUIPresenter _presenter;
    private float _dashCountRatio;

    private void Start()
    {
        _presenter.OnDashCountChanged += UpdateProgressBar;
    }

    public override void UpdateProgressBar(int cur, int max)
    {
        _dashCountRatio = Mathf.Clamp01(cur /(float)max);
        _progressBarImage.fillAmount = _dashCountRatio;
    }
}
