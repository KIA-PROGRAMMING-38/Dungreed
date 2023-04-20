using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class BelialBullet : MonoBehaviour
{
    private ObjectPool<BelialBullet> _owner;
    [SerializeField] private float _lifeTime;
    [SerializeField] private string _hitFxPath;
    private float _elapsedTime;
    [SerializeField] private float _speed;
    private Vector2 _direction;
    private DamageInfo _damageInfo;
    private Vector2 _startPosition;
    private bool _isReleased;

    public void SetOwner(ObjectPool<BelialBullet> owner) => _owner = owner;

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        if(_elapsedTime > _lifeTime)
        {
            if(_isReleased == false)
            {
                _isReleased = true;
                _owner.Release(this);
            }
        }
    }

    private void FixedUpdate()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReleased) return;


        if (Globals.LayerMask.CompareMask(collision.gameObject.layer, Globals.LayerMask.Player))
        {
            IDamageable obj = collision.GetComponent<IDamageable>();
            obj?.Hit(_damageInfo, gameObject);

            Vector2 direction = ((Vector2)transform.position - _startPosition).normalized;
            float angle = Utils.Utility2D.DirectionToAngle(direction.x, direction.y);
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            GameManager.Instance.FxPooler.GetFx(_hitFxPath, collision.ClosestPoint(transform.position), rot, Vector3.one);

            _isReleased = true;
            _owner.Release(this);
        }
    }
}
