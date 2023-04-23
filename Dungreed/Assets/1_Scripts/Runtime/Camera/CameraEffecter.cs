using Cinemachine;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraEffecter : MonoBehaviour
{
    [SerializeField, ShowOnly] private Camera _mainCamera;
    [SerializeField, ShowOnly] private CinemachineVirtualCamera _virtualCamera;
    [SerializeField, ShowOnly] private VolumeProfile _volumeProfile;


    #region  CameraShake
    [Header("--- Camera Shake ---")]

    private IEnumerator _cameraShakeCoroutine;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;

    [SerializeField, ShowOnly]
    private float _cameraShakeDuration;
    [SerializeField, ShowOnly]
    private float _cameraShakeIntensity;
    #endregion

    #region  Chromatic Aberration
    [Header("ChromaticAbberation"), Space(10)]
    [SerializeField, Range(0f, 1f)]
    private float _caDuration;
    [SerializeField, Range(0f, 1f)]
    private float _caMaxIntensity;
    private ChromaticAberration _chromaticAberration;
    #endregion

    #region Vignette
    [Header("--- Vignette ---")]
    [SerializeField]
    private Color _vignetteCol;
    [SerializeField]
    private float _vignetteDuration;
    [SerializeField]
    private float _vignetteMaxIntensity;
    [SerializeField, Range(0f, 1f)]
    private float _vignetteSmoothness;
    private Vignette _vignette;
    #endregion

    [SerializeField]
    private Material _roomTransitionMaterial;

    [SerializeField]
    private float _transitionTime = 1f;

    [SerializeField]
    private string _propertyName = "_Progress";

    private Action _onTransitionDone;
    private bool _reverseTransition;

    private IEnumerator _transitionCoroutine;

    public void CameraInitialize(Camera mainCam, CinemachineVirtualCamera virtualCamera)
    {
        _mainCamera = mainCam;
        _virtualCamera = virtualCamera;

        _volumeProfile = _mainCamera?.GetComponent<Volume>().profile;
        Debug.Assert(_volumeProfile != null, "VolumeProfile is Null", _volumeProfile);
        _cinemachineBasicMultiChannelPerlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _chromaticAberration = GetVolumeComponent<ChromaticAberration>(_volumeProfile);
        _vignette = GetVolumeComponent<Vignette>(_volumeProfile);

        _transitionCoroutine = TransitionCoroutine();
        _cameraShakeCoroutine = ShakeScreenEffectCoroutine();
    }

    T GetVolumeComponent<T>(VolumeProfile profile) where T : VolumeComponent
    {
        // Profile이 없다면 null 리턴
        if (profile is null) return null;

        if (_volumeProfile.TryGet<T>(out T comp) == false)
        {
            Debug.Assert(true, $"Invalid VolumeComponent : {nameof(T)}");
            return null;
        }

        return comp;
    }

    #region Screen Shake Method

    public void PlayScreenShake(float duration, float intensity)
    {
        _cameraShakeDuration = duration;
        _cameraShakeIntensity = intensity;
        StartCoroutine(_cameraShakeCoroutine);
    }

    public void PlayScreenShake()
    {
        _cameraShakeDuration = 0.1f;
        _cameraShakeIntensity = 2f;
        StartCoroutine(_cameraShakeCoroutine);
        PlayScreenBlink();
    }


    IEnumerator ShakeScreenEffectCoroutine()
    {
        while(true)
        {
            Debug.Log($"Camera Shake / Dur : {_cameraShakeDuration} / Inten : {_cameraShakeIntensity}");
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = _cameraShakeIntensity;
            yield return YieldCache.WaitForSeconds(_cameraShakeDuration);
            _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            StopCoroutine(_cameraShakeCoroutine);
            yield return null;
        }
    }
    #endregion

    #region ChromaticAbberation Method
    public void PlayChromaticAbberation(float duration = 0f, float intensity = 0f)
    {
        StartCoroutine(ChromaticAberrationEffectCoroutine(duration, intensity));
    }
    IEnumerator ChromaticAberrationEffectCoroutine(float duration, float intensity)
    {
        float t = 0f;
        float effectDuration = duration == 0f ? _caDuration : duration;
        float changeIntensity = 0;
        float caIntensity = intensity == 0f ? _caMaxIntensity : intensity;
        // 깜빡여야함  -1 ~ 1
        while (t -0.1f < effectDuration)
        {
            t += Time.deltaTime;
            float sinVal = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, t / effectDuration));
            changeIntensity = Mathf.Clamp(sinVal, 0, caIntensity);
            _chromaticAberration.intensity.value = changeIntensity;
            yield return null;
        }
        _chromaticAberration.intensity.value = 0;
    }
    #endregion

    #region Vignette Shake Method
    public void PlayScreenBlink()
    {
        StartCoroutine(VignetteEffectCoroutine());
    }
    IEnumerator VignetteEffectCoroutine()
    {
        float t = 0f;
        float currentIntensity = 0f;
        float middle = _vignetteDuration * 0.5f;

        _vignette.intensity.value = 0f;
        _vignette.smoothness.value = _vignetteSmoothness;
        _vignette.color.value = _vignetteCol;

        while (t - 0.1f < _vignetteDuration)
        {
            t += Time.deltaTime;
            if (t < middle)
            {
                float v = Mathf.Lerp(0, _vignetteMaxIntensity, t / middle);
                currentIntensity = v;
            }
            else
            {
                float v = Mathf.Lerp(_vignetteMaxIntensity, 0, (t - middle) / middle);
                currentIntensity = v;
            }
            _vignette.intensity.value = currentIntensity;
            // float sinVal = Mathf.Sin(Mathf.Lerp(0, Mathf.PI, t / duration));
            // currentIntensity = Mathf.Clamp(sinVal, 0, maxIntensity);
            // _vignette.intensity.value = currentIntensity;
            yield return null;
        }
        _vignette.intensity.value = 0f;

    }
    #endregion

    /// <summary>
    /// 씬 전환 효과입니다.
    /// </summary>
    /// <param name="transitionDone">트랜지션이 끝나면 실행할 콜백 함수</param>
    /// <param name="reverse">기본은 0 -> 1 입니다.(검은화면 = 1)
    /// 1 -> 0으로 만들지 </param>
    public void PlayTransitionEffect(Action transitionDone = null, bool reverse = false)
    {
        _onTransitionDone = transitionDone;
        _reverseTransition = reverse;
        StartCoroutine(_transitionCoroutine);
    }

    public void PlayTransitionEffect(bool reverse = false)
    {
        _onTransitionDone = null;
        _reverseTransition = reverse;
        StartCoroutine(_transitionCoroutine);
    }

    private IEnumerator TransitionCoroutine()
    {
        while (true)
        {
            float elapsedTime = 0f;
            _roomTransitionMaterial.SetFloat(_propertyName, 0f);
            yield return YieldCache.WaitForSecondsRealtime(0.1f);
            while (elapsedTime < _transitionTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float transitionRatio = _reverseTransition == false ? elapsedTime / _transitionTime : 1 - (elapsedTime / _transitionTime);
                _roomTransitionMaterial.SetFloat(_propertyName, Mathf.Clamp01(transitionRatio));
                yield return null;
            }
            _onTransitionDone?.Invoke();
            _onTransitionDone = null;
            StopCoroutine(_transitionCoroutine);
            yield return null;
        }
    }
}