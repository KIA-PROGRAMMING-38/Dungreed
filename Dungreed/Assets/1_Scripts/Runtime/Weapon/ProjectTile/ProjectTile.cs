using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class ProjectTile : MonoBehaviour
{
    protected ObjectPool<ProjectTile> _owner;
    protected SpriteRenderer _renderer;
    protected BoxCollider2D _collider;
    protected ProjectTileData _data;
    protected int       _damage;
    protected Vector2   _direction;
    protected LayerMask _collisionMask;
    protected Vector2 _startPosition;

    protected void OnEnable()
    {
        _renderer = this.GetComponentAllCheck<SpriteRenderer>();
        _collider = this.GetComponentAllCheck<BoxCollider2D>();

    }

    public void SetOwner(ObjectPool<ProjectTile> owner)
    {
        _owner = owner;
    }

    public void SetCollisionMask(LayerMask collision)
    {
        _collisionMask = collision;
    }

    public void InitProjectTile(Vector3 position, Vector3 dir, ProjectTileData data, int damage)
    {
        transform.position = position;
        _data = data;
        _renderer.sprite = ResourceCache.GetResource<Sprite>(data.SpritePath);
        _startPosition = position;
        _direction = dir;
        float angle = Utils.Utility2D.GetAngleFromVector(dir);
        Debug.Log(angle);
        transform.rotation = Quaternion.Euler(0,0, angle);
        _collider.size = _renderer.sprite.bounds.size;
        _damage = damage;
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        _data = null;
        _renderer.sprite = null;
        _startPosition = Vector2.zero;
        _direction = Vector3.zero;
        _collider.size = Vector3.zero;
    }

    protected virtual void FixedUpdate()
    {
        if (_data == null) return;

        float startToCurrentDist = Vector2.Distance(_startPosition, transform.position);

        if(startToCurrentDist > _data.Range)
        {
            _owner.Release(this);
            return;
        }

        transform.position += (Vector3)(_direction * (_data.Speed * Time.fixedDeltaTime));
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if((_collisionMask & 1 << collision.gameObject.layer ) == 1 << collision.gameObject.layer)
        {
           IDamageable obj = collision.GetComponent<IDamageable>();
            obj?.Hit(_damage, gameObject);
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            float angle = Utils.Utility2D.GetAngle(transform.right, direction);
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            GameManager.Instance.FxPooler.GetFx("MoveFx", collision.ClosestPoint(transform.position), rot, Vector3.one);

            _owner.Release(this);
        }
    }
}
