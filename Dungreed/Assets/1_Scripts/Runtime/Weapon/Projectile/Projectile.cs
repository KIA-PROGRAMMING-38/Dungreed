using System.IO;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    protected ObjectPool<Projectile> _owner;
    protected SpriteRenderer _renderer;
    protected BoxCollider2D _collider;
    protected ProjectileData _data;
    protected float _mouseAngle;
    protected int _damage;
    protected Vector2 _direction;
    protected LayerMask _collisionMask;
    protected Vector2 _startPosition;

    protected bool _isReleased;

    protected void OnEnable()
    {
        _renderer = this.GetComponentAllCheck<SpriteRenderer>();
        _collider = this.GetComponentAllCheck<BoxCollider2D>();
    }

    public void SetOwner(ObjectPool<Projectile> owner)
    {
        _owner = owner;
    }

    public void SetCollisionMask(LayerMask collision)
    {
        _collisionMask = collision;
    }

    public void InitProjectTile(Vector3 position, Vector3 dir, ProjectileData data, int damage)
    {
        transform.position = position;
        _data = data;
        _renderer.sprite = ResourceCache.GetResource<Sprite>(Path.Combine(ResourcePath.DefaultSpritesPath, data.SpritePath));
        _startPosition = position;
        _direction = dir;
        _mouseAngle = Utils.Utility2D.GetAngleFromVector(dir);
        transform.rotation = Quaternion.Euler(0, 0, _mouseAngle);
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
        _isReleased = false;
    }

    protected virtual void FixedUpdate()
    {
        if (_data == null || _isReleased == true) return;

        float startToCurrentDist = Vector2.Distance(_startPosition, transform.position);

        if (startToCurrentDist > _data.Range)
        {
            _isReleased = true;
            _owner.Release(this);
            return;
        }

        transform.position += (Vector3)(_direction * (_data.Speed * Time.fixedDeltaTime));
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_data == null) return;
        if (_isReleased == true) return;


        if (Globals.LayerMask.CompareMask(collision.gameObject.layer, _collisionMask))
        {
            IDamageable obj = collision.GetComponent<IDamageable>();
            obj?.Hit(_damage, gameObject);
            // ¤µ¢a
            Vector2 direction = ((Vector2)transform.position - _startPosition).normalized;
            float angle = Utils.Utility2D.DirectionToAngle(direction.x, direction.y);
            angle += _data.SpriteAngleOffset;
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            if ((Globals.LayerMask.Enemy & 1 << collision.gameObject.layer) != 0
                && _data.Type == EnumTypes.ProjectileType.Through)
            {
                return;
            }

            GameManager.Instance.FxPooler.GetFx(_data.HitFxPath, collision.ClosestPoint(transform.position), rot, Vector3.one);

            _isReleased = true;
            _owner.Release(this);
        }
    }
}
