using Cinemachine;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossCutScene : MonoBehaviour, ICutScene
{
    [SerializeField] private BossRoom _room;
    [SerializeField] private BossBase _boss;

    string _name;
    string _description;

    [SerializeField] private Image _panel;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private float _panelFadeInTime;
    [SerializeField] private Color _panelFadeInColor;
    [SerializeField] private float _textFadeInTime;
    [SerializeField] private float _fadeOutTime;
    
    private Action _cutSceneAfterAction;


    public void Initialize()
    {
        _name = _boss.EnemyData.Name;
        _description = _boss.EnemyData.Desription;
        _nameText.text = _name;
    }

    public void ProcessCutScene(Action before = null, Action after = null)
    {
        Initialize();
        _cutSceneAfterAction = after;
        StartCoroutine(ProcessCutSceneCoroutine());
    }

    private IEnumerator ProcessCutSceneCoroutine()
    {
        // 페이드 인 -> 판넬
        // 텍스트 페이드 인 ->
        // 다같이 페이드 아웃
        // 준비 끝
        var transposer = GameManager.Instance.CameraManager.VirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        GameManager.Instance.CameraManager.VirtualCamera.ForceCameraPosition(GameManager.Instance.Player.transform.position, Quaternion.identity);
        transposer.m_XDamping = 20f;
        transposer.m_YDamping = 20f;
        GameManager.Instance.CameraManager.VirtualCamera.Follow = _boss.transform;


        _nameText.gameObject.SetActive(true);
        _panel.gameObject.SetActive(true);
        _descriptionText.gameObject.SetActive(true);
        Color defaultPanelColor = Color.black;
        Color defaultTextColor = Color.white;
        defaultPanelColor.a = defaultTextColor.a = 0;
        _panel.color = defaultPanelColor;
        _nameText.color = defaultTextColor;
        float t = 0;
        Color col = Color.black;

        while (t - 0.1f < _panelFadeInTime)
        {
            t += Time.deltaTime;
            col.a = Mathf.Lerp(0, 1, t / _panelFadeInTime);
            _panel.color = col;
            yield return null;
        }

        t = 0;
        col = Color.white;

        while (t - 0.1f < _textFadeInTime)
        {
            t += Time.deltaTime;
            col.a = Utils.Math.Utility2D.EaseOutCubic(0, 1, t / _textFadeInTime);
            _nameText.color = col;
            _descriptionText.color = col;
            yield return null;
        }

        // Desription Typing Effect
        int index = 0;
        while (true)
        {
            if (index > _description.Length) break;
            _descriptionText.text = _description.Substring(0, index);
            index++;
            yield return YieldCache.WaitForSeconds(0.3f);
        }

        t = 0;
        Color panelCol = Color.black;
        Color textCol = Color.white;
            
        while (t - 0.1f < _fadeOutTime)
        {
            t += Time.deltaTime;
            float a = Utils.Math.Utility2D.EaseOutCubic(1, 0, t / _fadeOutTime);
            panelCol.a = a;
            textCol.a = a;

            _panel.color = panelCol;
            _nameText.color = textCol;
            _descriptionText.color = textCol;
            yield return null;
        }
        _nameText.gameObject.SetActive(false);
        _panel.gameObject.SetActive(false);
        _descriptionText.gameObject.SetActive(false);
        transposer.m_XDamping = 1f;
        transposer.m_YDamping = 1f;
        GameManager.Instance.CameraManager.VirtualCamera.Follow = GameManager.Instance.Player.transform;
        _cutSceneAfterAction?.Invoke();
    }
}
