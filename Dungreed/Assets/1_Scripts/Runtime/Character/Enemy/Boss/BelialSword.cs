﻿using UnityEngine;
using UnityEngine.Pool;

public class BelialSword : MonoBehaviour
{
    [SerializeField] private GameObject ChargeEffectObject;
    [SerializeField] private BossBelial _body;
    [SerializeField] private string _spawnFxPath;
    [SerializeField] private string _hitFxPath;
    [SerializeField] private float _disappearTime;

    private SpriteRenderer _renderer;
    private Animator _anim;

    private static readonly int ID_AttackTrigger = Animator.StringToHash("Attack");
    private static readonly int ID_ResetTrigger = Animator.StringToHash("Reset");

    private bool _attackTrigger;
    private bool _chargeTrigger;
    private bool _isCollided;
    private float _collidedElapsedTime;
    private const float SWORD_SPEED = 30f;

    private Vector2 _direction;
    private DamageInfo _damageInfo;
    private Vector2 _startPosition;
    private Vector3 _spawnFxScale;

    private Color _color;

    private Transform _target;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _damageInfo = new DamageInfo() { Damage = 8 };
        _spawnFxScale = new Vector3(1.5f, 1.5f, 1f);
    }

    private void OnEnable()
    {

        if (_body != null)
        {
            _body.OnDieAction -= OnDie;
            _body.OnDieAction += OnDie;
        }

    }

    private void OnDisable()
    {
        if (_body != null)
        { 
            _body.OnDieAction -= OnDie;
        }
        ResetSword();
    }

    public void SetOwner(BossBelial body)
    {
        _body = body;
        _body.OnDieAction -= OnDie;
        _body.OnDieAction += OnDie;
    }

    // 초기 위치 설정
    public void SetStartPosition(Vector2 start)
    {
        _target = GameManager.Instance.Player.transform;
        _startPosition = start;
        transform.position = start;
        gameObject.SetActive(false);
    }

    // 비활성화돼었을 때
    public void ResetSword()
    {
        transform.position = _startPosition;
        _isCollided = false;
        _chargeTrigger = false;
        _attackTrigger = false;
        _color = Color.white;
        _renderer.color = _color;
        _collidedElapsedTime = 0f;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        ChargeEffectObject.SetActive(false);
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
        GameManager.Instance.FxPooler.GetFx(_spawnFxPath, transform.position, Quaternion.identity, _spawnFxScale);
        ChargeSword();
    }

    public void FireSword()
    {
        _chargeTrigger = false;
        _attackTrigger = true;
        _anim.SetTrigger(ID_AttackTrigger);
    }

    public void ChargeSword()
    {
        _chargeTrigger = true;

    }

    public void Update()
    {
        if (_chargeTrigger == true)
        {
            _direction = ((Vector2)_target.position - _startPosition).normalized;
            float angle = Utils.Utility2D.DirectionToAngle(_direction.x, _direction.y);
            angle += -90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (_attackTrigger == true && _isCollided == false)
        {
            transform.position += (Vector3)(_direction * SWORD_SPEED * Time.deltaTime);
        }

        if (_attackTrigger == false && _isCollided == true)
        {
            _collidedElapsedTime += Time.deltaTime;
            _color.a = Mathf.Lerp(1f, 0f, _collidedElapsedTime / _disappearTime);
            _renderer.color = _color;
            if (_collidedElapsedTime > _disappearTime)
            {
                _collidedElapsedTime = 0f;
                ResetSword();
                gameObject.SetActive(false);
                
            }
        }
    }

    public void OnDie()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isCollided == true) return;

        if (Globals.LayerMask.CompareMask(collision.gameObject.layer, Globals.LayerMask.Player))
        {
            IDamageable obj = collision.GetComponent<IDamageable>();
            obj?.Hit(_damageInfo, gameObject);
        }
        else if (Globals.LayerMask.CompareMask(collision.gameObject.layer, Globals.LayerMask.Platform))
        {
            Vector2 direction = ((Vector2)_target.position - _startPosition).normalized;
            float angle = Utils.Utility2D.DirectionToAngle(direction.x, direction.y);
            angle += 90;
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            GameManager.Instance.FxPooler.GetFx(_hitFxPath, collision.ClosestPoint(transform.position), rot, Vector3.one);
            _attackTrigger = false;
            _isCollided = true;
            _anim.SetTrigger(ID_ResetTrigger);
        }
    }
}
