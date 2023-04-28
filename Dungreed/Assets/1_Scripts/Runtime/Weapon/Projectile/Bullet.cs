using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private GameObject _ownerObject;
    private ObjectPool<Bullet> _owner;
    [SerializeField] private float _lifeTime;
    [SerializeField] private string _hitFxPath;
    [SerializeField] private LayerMask _collisionMask;
    private float _elapsedTime;
    [SerializeField] private float _speed;
    protected Vector2 _direction;
    protected DamageInfo _damageInfo;
    protected Vector2 _startPosition;
    protected bool _isReleased;

    public void SetOwner(ObjectPool<Bullet> owner) => _owner = owner;
    public void SetOwnerObject(GameObject ownerObj) => _ownerObject = ownerObj;

    protected virtual void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > _lifeTime)
        {
            if (_isReleased == false)
            {
                _isReleased = true;
                _owner.Release(this);
                GameManager.Instance.FxPooler.GetFx(_hitFxPath, transform.position, Quaternion.identity, Vector3.one);
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        transform.position += (Vector3)(_direction * _speed * Time.fixedDeltaTime);
    }

    public void ResetBullet()
    {
        _elapsedTime = 0f;
        _isReleased = false;
        _direction = Vector3.zero;
    }

    public void InitBullet(Vector3 position, Vector3 dir, DamageInfo damageInfo)
    {
        transform.position = position;
        _startPosition = position;
        _direction = dir;
        _damageInfo = damageInfo;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReleased) return;


        if (Globals.LayerMask.CompareMask(collision.gameObject.layer, _collisionMask))
        {
            IDamageable obj = collision.GetComponent<IDamageable>();

            if(obj?.Hit(_damageInfo, _ownerObject) == true)
            {
                GameManager.Instance.FxPooler.GetFx(_hitFxPath, transform.position, Quaternion.identity, Vector3.one);

                _isReleased = true;
                _owner.Release(this);
            }
        }
    }
}
