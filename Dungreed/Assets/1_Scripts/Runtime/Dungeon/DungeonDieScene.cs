using System;
using System.Collections;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DungeonDieScene : MonoBehaviour, ICutScene
{
    [SerializeField] private Image _panel;
    [SerializeField] private Image _dieTextMask;
    private TextMeshProUGUI _dieText;
    [SerializeField] private TextMeshProUGUI _reasonText;
    private static readonly string DefeatSoundName = "BossDefeat";
    private string _reason;

    [SerializeField] private float _fadeInTime;
    [SerializeField] private float _dieTextFadeInTime;
    [SerializeField] private float _timeRecoverTime;
    [SerializeField] private float _reasonTypingInterval;
    [SerializeField] private float _fadeOutTime;

    private Action _beforeAction;
    private Action _afterAction;

    private void Awake()
    {
        _dieText = _dieTextMask.GetComponentInChildren<TextMeshProUGUI>();
        _panel.gameObject.SetActive(false);
        _dieTextMask.gameObject.SetActive(false);
        _reasonText.gameObject.SetActive(false);
    }

    public void SetReason(string reasonEnemyName)
    {
        _reason = $"사망원인 : {reasonEnemyName}";
    }

    public void ProcessCutScene(Action before = null, Action after = null)
    {
        _beforeAction = before;
        _afterAction = after;
        StartCoroutine(CutScene());
    }

    public IEnumerator CutScene()
    {
        SoundManager.Instance.BGMPlay(DefeatSoundName, false, 1);
        _beforeAction?.Invoke();
        StartCoroutine(TimeSlow());
        float elapsedTime = 0f;
        Color color = _panel.color;
        Color textColor = _dieText.color;
        color.a = textColor.a = 0f;
        
        _panel.color= color;
        _panel.gameObject.SetActive(true);
        _dieTextMask.gameObject.SetActive(true);
        

        while (elapsedTime - 0.1f < _fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsedTime / _fadeInTime);
            _panel.color= color;
            yield return null;
        }

        elapsedTime = 0f;
        while(elapsedTime - 0.1f < _dieTextFadeInTime)
        {
            elapsedTime += Time.deltaTime;
            _dieTextMask.fillAmount = Mathf.Lerp(0, 1, elapsedTime/_dieTextFadeInTime);
            yield return null;
        }

        // Typing Effect
        int length = 1;
        _reasonText.color = Color.white;
        _reasonText.gameObject.SetActive(true);
        if(string.IsNullOrEmpty(_reason) == false)
        {
            while (true)
            {
                if (length > _reason.Length) break;
                _reasonText.text = _reason.Substring(0, length);
                if (_reason[length - 1] != ' ')
                {   
                    SoundManager.Instance.EffectPlay("Beep", Vector3.zero);
                }

                length++;
                yield return YieldCache.WaitForSeconds(_reasonTypingInterval);
            }
        }

        elapsedTime = 0f;
        textColor = _dieText.color;
        Color reasonTextColor = Color.white;

        while(elapsedTime - 0.1f < _fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / _fadeOutTime);
            textColor.a = reasonTextColor.a = alpha;
            _dieText.color = textColor;
            _reasonText.color = reasonTextColor;
            yield return null;
        }

        _dieTextMask.gameObject.SetActive(false);
        _reasonText.gameObject.SetActive(false);
        yield return YieldCache.WaitForSeconds(2f);
        _afterAction?.Invoke();
        yield return null;
    }

    private IEnumerator TimeSlow()
    {
        float t = 0;
        while (t - 0.1f < _timeRecoverTime)
        {
            t += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(0, 1, t / _timeRecoverTime);
            yield return null;
        }
    }
}
