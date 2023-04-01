using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] LevelBounds _levelBounds;

    [SerializeField]
    float _followSpeed = 1f;


    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Vector3 _offset;
    private Vector3 _velocity = Vector3.zero;
    private void LateUpdate()
    {
        Vector3 targetPos = _target.position + _offset;
        Vector3 DampPos = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, _followSpeed);
        transform.position = DampPos;



        var bounds = Camera.main.OrthographicBounds();
        float minX = bounds.min.x;
        float maxX = bounds.max.x;
        float minY = bounds.min.y;
        float maxY = bounds.max.y;


        Bounds lb = _levelBounds.Bounds;
        // 카메라 경계가 레벨 경계보다 작아지면
        if (minX < lb.min.x)
        {
            DampPos.x = lb.min.x + (bounds.size.x * 0.5f);
        }
        else if (maxX > lb.max.x)
        {
            DampPos.x = lb.max.x - (bounds.size.x * 0.5f);
        }

        if (minY < lb.min.y)
        {
            DampPos.y = lb.min.y + (bounds.size.y * 0.5f);
        }
        else if (maxY > lb.max.y)
        {
            DampPos.y = lb.max.y - (bounds.size.y * 0.5f);
        }

        DampPos.z = transform.position.z;

        transform.position = DampPos;
    }
}
