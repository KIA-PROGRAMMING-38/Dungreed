using Cysharp.Threading.Tasks;
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
        ProcessCutSceneTask().Forget();
    }

    private async UniTaskVoid ProcessCutSceneTask()
    {
        GameManager.Instance.CameraManager.VirtualCamera.Follow = _boss.transform;
        GameManager.Instance.CameraManager.VirtualCamera.ForceCameraPosition(GameManager.Instance.Player.transform.position, Quaternion.identity);
        await UniTask.Delay(1000);

        _nameText.gameObject.SetActive(true);
        _panel.gameObject.SetActive(true);
        _descriptionText.gameObject.SetActive(true);
        Color defaultPanelColor = Color.black;
        Color defaultTextColor = Color.white;
        defaultPanelColor.a = defaultTextColor.a = 0;
        _panel.color = defaultPanelColor;
        _nameText.color = defaultTextColor;

        float elapsedTime = 0;
        Color col = Color.black;

        while (elapsedTime - 0.1f < _panelFadeInTime)
        {
            elapsedTime += Time.deltaTime;
            col.a = Mathf.Lerp(0, 1, elapsedTime / _panelFadeInTime);
            _panel.color = col;
            await UniTask.Yield();
        }

        elapsedTime = 0;
        col = Color.white;

        while (elapsedTime - 0.1f < _textFadeInTime)
        {
            elapsedTime += Time.deltaTime;
            col.a = Utils.Math.Utility2D.EaseOutCubic(0, 1, elapsedTime / _textFadeInTime);
            _nameText.color = col;
            _descriptionText.color = col;
            await UniTask.Yield();
        }

        // Desription Typing Effect
        int length = 1;
        while (true)
        {
            if (length > _description.Length) break;
            _descriptionText.text = _description.Substring(0, length);
            if (_description[length - 1] != ' ')
            {
                SoundManager.Instance.EffectPlay("Beep", Vector3.zero);
            }
            length++;
            await UniTask.Delay(300);
        }

        elapsedTime = 0;
        Color panelCol = Color.black;
        Color textCol = Color.white;
            
        while (elapsedTime - 0.1f < _fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            float a = Utils.Math.Utility2D.EaseOutCubic(1, 0, elapsedTime / _fadeOutTime);
            panelCol.a = a;
            textCol.a = a;

            _panel.color = panelCol;
            _nameText.color = textCol;
            _descriptionText.color = textCol;
            await UniTask.Yield();
        }
        _nameText.gameObject.SetActive(false);
        _panel.gameObject.SetActive(false);
        _descriptionText.gameObject.SetActive(false);
        GameManager.Instance.CameraManager.VirtualCamera.Follow = GameManager.Instance.Player.transform;
        _cutSceneAfterAction?.Invoke();
    }
}
