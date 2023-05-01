using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasSettingCamera : MonoBehaviour
{
    Canvas _owner;
    private void Awake()
    {
        _owner = GetComponent<Canvas>();
        if (_owner.renderMode != RenderMode.ScreenSpaceCamera) return;
        if (_owner.worldCamera == null)
        {
            _owner.worldCamera = GameManager.Instance.CameraManager.MainCamera;
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if(_owner.renderMode != RenderMode.ScreenSpaceCamera) return;
        _owner = GetComponent<Canvas>();
        if (GameManager.Instance.CameraManager)
        {
            _owner.worldCamera = GameManager.Instance.CameraManager.MainCamera;
        }
    }
}
