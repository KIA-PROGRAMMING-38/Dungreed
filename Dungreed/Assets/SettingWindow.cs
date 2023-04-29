using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SettingWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _volumeText;
    [SerializeField] private GameObject _exitButton;

    public void OpenWindow()
    {
        gameObject.SetActive(true);
        _exitButton.SetActive(true);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
        _exitButton.SetActive(false);
    }

    public void OnValueChanged(float t)
    {
        SoundManager.Instance.BGMVolume = t;
        int v = ((int)(t * 100));
        v = Mathf.Clamp(v, 0, 100);
        _volumeText.text = v.ToString();
    }
}
