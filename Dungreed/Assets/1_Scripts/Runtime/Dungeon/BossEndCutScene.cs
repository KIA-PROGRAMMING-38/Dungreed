using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossEndCutScene : MonoBehaviour, ICutScene
{
    [SerializeField] private Image _panel;
    [SerializeField] private Image _flash;
    [SerializeField] private TextMeshProUGUI _endingScriptText;
    [SerializeField] private float _boomSpawnInterval;
    [SerializeField] private float _timeRecoverTime;
    [SerializeField] private float _flashTime;
    [SerializeField] private float _boomParticleTime;
    [SerializeField] private float _panelFadeInTime;
    [SerializeField] private float _endingTypingInterval;
    [SerializeField] private float _fadeoutTime;
    public string endingScript;
    [SerializeField] private BossBase _boss;
    private Action _beforeAction;
    private Action _afterAction;


    private static readonly string _bossClearFxName = "BossClearFx";
    public bool IsPlayingCutScene { get; set; } = true;

    public void ProcessCutScene(Action before = null, Action after = null)
    {
        _beforeAction = before;
        _afterAction = after;

        StartCoroutine(CutScene());
    }

    private IEnumerator CutScene()
    {
        _beforeAction?.Invoke();

        SoundManager.Instance.EffectPlay("BossDefeat", transform.position);
        StartCoroutine(Flash());
        StartCoroutine(TimeSlow());

        float t = 0;
        float boomSpawnElapsedTime = 0;
        GameManager.Instance.CameraManager.VirtualCamera.Follow = _boss.transform;
        GameManager.Instance.CameraManager.CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 4;
        while(t - 0.1f < _boomParticleTime)
        {
            t += Time.deltaTime;
            boomSpawnElapsedTime += Time.deltaTime;
            if(boomSpawnElapsedTime > _boomSpawnInterval)
            {
                boomSpawnElapsedTime = 0f;
                Vector2 randomPosition = (Vector2)_boss.transform.position + UnityEngine.Random.insideUnitCircle * 5f;
                GameManager.Instance.FxPooler.GetFx(_bossClearFxName, randomPosition, Quaternion.identity, new Vector3(2,2,1));
                SoundManager.Instance.EffectPlay("BossDieParticle", false, 0.5f, transform.position);
            }
            yield return null;
        }
        
        // 검은색 화면 채우기
        t = 0f;
        _panel.gameObject.SetActive(true);
        Color color = Color.black;
        color.a = 0f;
        _panel.color = color;
        while(t - 0.1f < _panelFadeInTime)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, t / _panelFadeInTime);
            _panel.color = color;
            yield return null;
        }
        GameManager.Instance.CameraManager.CinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;

        // 보스 죽은 모습전환
        _afterAction?.Invoke();


        // Typing Effect
        int index = 0;
        _endingScriptText.gameObject.SetActive(true);
        while (true)
        {
            if (index > endingScript.Length) break;
            _endingScriptText.text = endingScript.Substring(0, index);
            index++;
            yield return YieldCache.WaitForSeconds(_endingTypingInterval);
        }

        t = 0f;
        color = _panel.color;
        Color textCol = Color.white;

        while (t < _fadeoutTime)
        {
            t += Time.deltaTime;
            textCol.a = color.a = Mathf.Lerp(1, 0, t / _fadeoutTime);   
            _panel.color = color;
            _endingScriptText.color = textCol;
            yield return null;
        }


        _endingScriptText.gameObject.SetActive(false);
        _panel.gameObject.SetActive(false);
    
        GameManager.Instance.CameraManager.VirtualCamera.Follow = GameManager.Instance.Player.transform;
        SceneManager.LoadScene(1);
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

    private IEnumerator Flash()
    {
        float t = 0f;
        Color color = Color.white;
        _flash.gameObject.SetActive(true);
        _flash.color = color;
        while(t -0.1f < _flashTime)
        {
            t += Time.unscaledDeltaTime;
            color.a = Mathf.Lerp(1, 0, t / _flashTime);
            _flash.color = color;
            yield return null;
        }
        _flash.gameObject.SetActive(false);
    }
}
