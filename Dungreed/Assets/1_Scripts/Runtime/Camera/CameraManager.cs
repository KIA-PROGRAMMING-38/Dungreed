using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera                       MainCamera { get; private set; }
    public CinemachineVirtualCamera     VirtualCamera { get; private set; }
    public CinemachineConfiner2D Confiner2D { get; private set; }
    public CinemachineBasicMultiChannelPerlin CinemachineBasicMultiChannelPerlin { get; private set; }

    public CameraEffecter Effecter { get; private set; }

    public void Awake()
    {
        MainCamera = GetComponentInChildren<Camera>();
        VirtualCamera= GetComponentInChildren<CinemachineVirtualCamera>();
        Confiner2D = VirtualCamera.GetComponent<CinemachineConfiner2D>();
        CinemachineBasicMultiChannelPerlin = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Effecter = GetComponent<CameraEffecter>();
        Effecter.CameraInitialize(MainCamera, VirtualCamera);
    }

    public void SetConfiner(LevelBounds bound)
    {
        Confiner2D.m_BoundingShape2D = bound.Collider;
        GameManager.Instance.Player.GetComponent<PlayerController>().SetBounds(bound);
    }

    public void SettingCamera(LevelBounds bound, InitPosition initPos)
    {
        SetConfiner(bound);
        MainCamera.transform.position = initPos.Position;
        VirtualCamera.transform.position = initPos.Position;
    }
}
