using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class CameraEffectManager : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] VolumeProfile _volumeProfile;


    #region  CameraShake
    [Header("--- Camera Shake ---")]
    [SerializeField]
    private float _cameraShakeDuration = 0.3f;
    [SerializeField, Range(0, 10)]
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        CameraInitialize();
        Debug.Log("Camera Initialize Call");
    }

    private void Awake()
    {
        CameraInitialize();
    }

    void CameraInitialize()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        _volumeProfile = _mainCamera?.GetComponent<Volume>().profile;
        Debug.Assert(_volumeProfile != null, "VolumeProfile is Null", _volumeProfile);

        _chromaticAberration = GetVolumeComponent<ChromaticAberration>(_volumeProfile);
        _vignette = GetVolumeComponent<Vignette>(_volumeProfile);
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

    public void PlayScreenShake(float duration = 0f, float intensity = 0f)
    {
        StartCoroutine(ShakeScreenEffectCoroutine(duration, intensity));
    }


    IEnumerator ShakeScreenEffectCoroutine(float duration, float intensity)
    {
        float t = duration == 0f ? _cameraShakeDuration : duration;
        float shakeIntensity = intensity == 0f ? _cameraShakeIntensity : intensity;

        while (t > 0f)
        {
            t -= Time.deltaTime;
            Vector3 shakePos = Random.insideUnitCircle * shakeIntensity;
            _mainCamera.transform.position = _mainCamera.transform.position + shakePos;
            yield return null;
        }
        /// _mainCamera.transform.position = originPos;
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
            // 0 ~ 파이 까지
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
}