using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    private float _length;
    Vector2 _startPos;
    public GameObject cam;
    public float parallexEffect;
    void Start()
    {
        
        _startPos = transform.position;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate() {
        float dist = (cam.transform.position.x * parallexEffect);
        float dist2 = (cam.transform.position.y * parallexEffect);
        transform.position = new Vector3(_startPos.x + dist, _startPos.y + dist2, 0f);
    }
}
