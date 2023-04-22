using Cinemachine;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    private float _length;
    Vector2 _startPos;
    //public CinemachineVirtualCamera cam;
    public Camera cam;
    public CinemachineVirtualCamera virtualCamera;
    public float parallexEffect;
    void Start()
    {
        _startPos = transform.position;
    }



    private void LateUpdate()
    {
        //float dist = (cam.transform.position.x * parallexEffect);
        //float dist2 = (cam.transform.position.y * parallexEffect);
        //transform.position = new Vector3(dist, dist2, 0f);
        
        float cameraPosX = virtualCamera.transform.position.x;
        float cameraPosY = cam.transform.position.y;
        float layerPosX = _startPos.x + (cameraPosX * parallexEffect);
        float layerPosY = _startPos.y + (cameraPosY * parallexEffect);

    
        transform.position = new Vector2(layerPosX, layerPosY);
    }
}
