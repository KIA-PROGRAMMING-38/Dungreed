using UnityEngine;
using static BaseController;

[RequireComponent(typeof(BoxCollider2D))]
public class BaseController : MonoBehaviour
{
    [SerializeField] 
    protected LevelBounds _bounds;
    public LevelBounds Bounds { get { return _bounds; } }
    protected Vector2 _direction;
    protected Vector2 _faceDirection;
    public Vector2 Direction { get { return _direction; } }
    public Vector2 FaceDirection { get { return _faceDirection; } }

    protected Collider2D _onewayPlatformCollider;
    [SerializeField]
    protected LayerMask _platformMask;
    [SerializeField]
    protected LayerMask _onewayPlatformMask;
    [SerializeField]
    protected LayerMask _enemyMask;
    public    LayerMask EnemyMask { get { return _enemyMask; } }

    [SerializeField] protected float _raycastDistance = 0.1f;
    [SerializeField] protected int _verticalRayCount = 4;
    [SerializeField] protected int _horizontalRayCount = 3;
    private float _verticalRaySpacing;
    private float _horizontalRaySpacing;
    private RaycastHit2D[] _hit = new RaycastHit2D[1];

    protected BoxCollider2D _collider;
    protected Rigidbody2D _rig2D;

    public Rigidbody2D Rig2D { get { return _rig2D; } }
    public BoxCollider2D Collider { get { return _collider; } }

    public Vector2 BoundCenter { get => _collider.bounds.center;  }
    public float BoundWidth { get => _collider.size.x; }
    public float BoundWidthHalf { get => _collider.size.x * 0.5f; }
    public float BoundHeight { get => _collider.size.y; }
    public float BoundHeightHalf { get => _collider.size.x * 0.5f; }
    public float LeftBound { get => _collider.bounds.center.x - _collider.bounds.extents.x; }
    public float RightBound { get => _collider.bounds.center.x + _collider.bounds.extents.x; }
    public float TopBound { get => _collider.bounds.center.y + _collider.bounds.extents.y; }
    public float BottomBound { get => _collider.bounds.center.y - _collider.bounds.extents.y; }

    public CollisionsInfo CollisionInfo = new CollisionsInfo();

    protected virtual void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rig2D = GetComponent<Rigidbody2D>();

        CalcRaySpacing();
    }
    protected virtual void Start()
    {

    }
    public void CheckRayAll()
    {
        CollisionInfo.Reset();
        VerticalCheck();
        HorizontalCheck();
    }

    public void CalcRaySpacing()
    {
        Bounds bounds = _collider.bounds;

        _verticalRaySpacing = bounds.size.x / (_verticalRayCount - 1);
        _horizontalRaySpacing = bounds.size.x / (_horizontalRayCount- 1);
    }
    public void VerticalCheck()
    {
        Bounds bounds = _collider.bounds;

        // ÇÃ·§Æû Ã¼Å©
        for (int i = 0; i < _verticalRayCount; i++)
        {
            Vector2 rayOrigin = new Vector2(bounds.min.x + (i * _verticalRaySpacing), bounds.min.y);
            int hitCount = Physics2D.RaycastNonAlloc(rayOrigin, Vector2.down, _hit, _raycastDistance, _platformMask);

            if (hitCount > 0)
            {
                CollisionInfo.IsGrounded = true;
                break;
            }
        }

        // ¿ø¿þÀÌ ÇÃ·§Æû Ã¼Å©
        for (int i = 0; i < _verticalRayCount; i++)
        {
            Vector2 rayOrigin = new Vector2(bounds.min.x + (i * _verticalRaySpacing), bounds.min.y);
            int hitCount = Physics2D.RaycastNonAlloc(rayOrigin, Vector2.down, _hit,_raycastDistance, _onewayPlatformMask);


            if (hitCount > 0)
            {
                CollisionInfo.IsGrounded = true;
                CollisionInfo.IsOnewayGrounded = true;
                _onewayPlatformCollider = _hit[0].collider;
                break;
            }
        }

    }


    public void HorizontalCheck()
    {
        if (_direction == Vector2.zero) return;

        Bounds bounds = _collider.bounds;

        float calcX = _direction.x == -1f ? bounds.min.x : bounds.max.x;

        for (int i = 0; i < _horizontalRayCount; ++i)
        {
            Vector2 rayOrigin = new Vector2(calcX, bounds.min.y + (i * _horizontalRaySpacing));
            int hitCount = Physics2D.RaycastNonAlloc(rayOrigin, _direction, _hit, _raycastDistance, _platformMask | _onewayPlatformMask);

            if (hitCount > 0)
            {
                CollisionInfo.left  = (Mathf.Sign(_direction.x) == -1f) ? true : false;
                CollisionInfo.right = (Mathf.Sign(_direction.x) == 1f) ? true : false;
                break;
            }
        }
    }

    protected void CharacterMovementBoundaryCheck()
    {
        Vector2 vel = Rig2D.velocity;
        Vector2 pos = transform.position;
        Bounds levelBounds= _bounds.Bounds;
        if (Rig2D.velocity.x < 0 && LeftBound <= levelBounds.min.x)
        {
            pos.x = levelBounds.min.x+ BoundWidthHalf;
            vel.x = 0;
        }
        else if (Rig2D.velocity.x > 0 && RightBound >= levelBounds.max.x)
        {
            pos.x = levelBounds.max.x - BoundWidthHalf;
            vel.x = 0;
        }
        if (Rig2D.velocity.y < 0 && BottomBound <= levelBounds.min.y)
        {
            pos.y = levelBounds.min.y + BoundHeightHalf;
            vel.y = 0;
        }
        else if (Rig2D.velocity.y > 0 && TopBound >= levelBounds.max.y)
        {
            pos.y = levelBounds.max.y - BoundHeightHalf;
            vel.y = 0;
        }

        Rig2D.velocity = vel;
        Rig2D.position = pos;
    }

    public struct CollisionsInfo
    {
        public bool left, right;
        public bool IsGrounded;
        public bool IsOnewayGrounded;
        public void Reset()
        {
            left = right = false;
            IsGrounded = IsOnewayGrounded = false;
        }
    }

}
