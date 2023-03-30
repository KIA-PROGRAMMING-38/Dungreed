
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Temp : MonoBehaviour
{
    Camera mainCam;
    public float duration;
    public float Intensity;
    VolumeProfile _volumeProfile;
    ChromaticAberration _ca;
    Bloom _bloom;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        _volumeProfile = mainCam.GetComponent<Volume>().profile;
        _volumeProfile.TryGet<ChromaticAberration>(out _ca);
        _volumeProfile.TryGet<Bloom>(out _bloom);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            StartCoroutine(EaseTest());
        }
        if(Input.GetMouseButton(1))
        {
            StartCoroutine(LerpTest());
        }
    }

    IEnumerator EaseTest()
    {
        float t = 0f;
        _bloom.intensity.value = 0;
        while(t-0.1f < duration)
        {
            t += Time.deltaTime;
            _bloom.intensity.value = Utils.Math.Utility2D.Spring(0, Intensity, t/duration);
            yield return null;
        }
         
    }
    IEnumerator LerpTest()
    {
        float t = 0f;
        _bloom.intensity.value = 0;
        while(t-0.1f < duration)
        {
            t += Time.deltaTime;
            _bloom.intensity.value = Mathf.Lerp(0, Intensity, t/duration);
            yield return null;
        }
         
    }
}
