using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class BossBelial : BossBase
{
    private enum PatternType
    {
        BulletSpawn,
        SwordSpawn,
        Laser,
        Max
    }

    [SerializeField] private BossBelialHand _leftHand;
    [SerializeField] private BossBelialHand _rightHand;
    [SerializeField] private Bullet _bossBullet;
    [SerializeField] private Transform _firePosition;
    [SerializeField] private Transform _bulletPoolTransform;
    [SerializeField] private float _actPatternInterval;

    public bool isDie { get { return _isDie; } }
    public Action OnDieAction;
    private BoxCollider2D _collider;
    private Rigidbody2D _rig2D;
    private bool _bulletAttackStart;
    private Vector2 _bulletSpawnPosition;
    private DamageInfo _bulletDamage;
    ObjectPool<Bullet> _bulletPool;

    public static readonly string BelialLaserSoundName = "BelialLaser";
    public static readonly string BelialBulletSoundName = "BelialBullet";
    public static readonly string BelialSwordSoundName = "BelialSword";
    public static readonly string BelialLaughSoundName = "BelialLaugh";

    private static readonly int ID_AttackTrigger = Animator.StringToHash("Attack");
    private static readonly int ID_ResetTrigger = Animator.StringToHash("Reset");
    private static readonly int ID_DieTrigger = Animator.StringToHash("Die");


    [SerializeField] BelialSword _swordPrefab;
    private BelialSword[] _swords = new BelialSword[6];
    private Vector3[] _swordSpawnPositions = new Vector3[6];

    private CancellationTokenSource _cancelToken;

    public override void Initialize(BossRoom room)
    {
        base.Initialize(room);
        _bulletPool = new ObjectPool<Bullet>(CreateFunc, GetAction, ReleaseAction, DestroyAction, false, 20, 300);
        _bulletDamage = new DamageInfo() { Damage = 6 };
        _collider = GetComponent<BoxCollider2D>();

        _rig2D = GetComponent<Rigidbody2D>();

        _health.Initialize(_enemyData.MaxHp);

        _ownerRoom.OnBossBattleEnd -= SetDie;
        _ownerRoom.OnBossBattleEnd += SetDie;
        _isBattleStart = false;
        _cancelToken = new CancellationTokenSource();
        FindPlayer();
    }

    public void SetDie()
    {
        _anim.SetTrigger(ID_DieTrigger);
    }

    protected override void OnDie()
    {
        _cancelToken.Cancel();
        _rig2D.gravityScale = 6f;
        _collider.enabled = false;
        _bulletPool.Clear();
        _isDie = true;
        Destroy(_bulletPoolTransform.gameObject);
        _ownerRoom.IsBossCleared = true;
        OnDieAction?.Invoke();
    }

    private void BossDieAction()
    {
        _leftHand.gameObject.SetActive(false);
        _rightHand.gameObject.SetActive(false);
    }

    protected override void OnBattleStart()
    {
        base.OnBattleStart();
        SoundManager.Instance.EffectPlay(BelialLaughSoundName, transform.position);
    }

    private void Update()
    {
        if (_cancelToken.IsCancellationRequested) return;
        if (_ownerRoom.IsBattleStart == false) return;

        if (_isActPattern == false)
        {
            SelectPattern();
        }
    }

    private void SelectPattern()
    {
        int pattern = (int)PatternType.Max;
        pattern = Random.Range(0, pattern);

        switch ((PatternType)pattern)
        {
            case PatternType.Laser:
                LaserFirePattern().Forget();
                break;
            case PatternType.SwordSpawn:
                SwordSpawnPattern().Forget();
                break;
            case PatternType.BulletSpawn:
                BulletSpawnPattern().Forget();
                break;
        }

    }

    public void OnBulletAttack()
    {
        _bulletAttackStart = true;
        _bulletSpawnPosition = _firePosition.position;
    }

    private async UniTaskVoid BulletSpawnPattern()
    {
        _isActPattern = true;
        _anim.SetTrigger(ID_AttackTrigger);

        while (_bulletAttackStart == false)
        {
            await UniTask.Yield();
        }

        float patternTime = 7f;
        float patternElapsedTime = 0f;
        float spawnInterval = 0.15f;
        float spawnElapsedTime = 0f;
        float angle = 0f;

        Vector2 dirVec = transform.position - _player.transform.position;
        float dot = Vector2.Dot(dirVec, transform.right);
        float addAngleValue = dot > 0 ? -5f : 5f;
        _firePosition.transform.rotation = Quaternion.Euler(0, 0, angle);

        while (patternElapsedTime < patternTime)
        {
            if (_cancelToken.IsCancellationRequested) break;

            patternElapsedTime += Time.deltaTime;
            spawnElapsedTime += Time.deltaTime;
            if (spawnElapsedTime > spawnInterval)
            {
                SoundManager.Instance.EffectPlay(BelialBulletSoundName, transform.position);
                Vector3 left = _firePosition.right * -1f;
                Vector3 right = _firePosition.right;
                Vector3 up = _firePosition.up;
                Vector3 down = _firePosition.up * -1f;
                angle += addAngleValue;
                _firePosition.transform.rotation = Quaternion.Euler(0, 0, angle);
                var leftBullet = _bulletPool.Get();
                var rightBullet = _bulletPool.Get();
                var upBullet = _bulletPool.Get();
                var downBullet = _bulletPool.Get();
                leftBullet.InitBullet(_bulletSpawnPosition, left, _bulletDamage);
                rightBullet.InitBullet(_bulletSpawnPosition, right, _bulletDamage);
                upBullet.InitBullet(_bulletSpawnPosition, up, _bulletDamage);
                downBullet.InitBullet(_bulletSpawnPosition, down, _bulletDamage);

                spawnElapsedTime = 0f;
            }
            await UniTask.Yield();
        }
        _bulletAttackStart = false;
        _anim.SetTrigger(ID_ResetTrigger);
        await UniTask.Delay((int)(1000 * _actPatternInterval));
        _isActPattern = false;
    }

    private async UniTaskVoid SwordSpawnPattern()
    {
        // 8
        Bounds bound = _collider.bounds;
        Vector3 pivot = bound.center;
        pivot.y = bound.max.y;
        float minX = pivot.x - 6f;
        float maxX = pivot.x + 6f;
        float swordDistance = (maxX - minX) / (_swords.Length - 1);

        for (int i = 0; i < _swordSpawnPositions.Length; ++i)
        {
            if (_cancelToken.IsCancellationRequested) break;
            _swordSpawnPositions[i].x = minX + swordDistance * i;
            _swordSpawnPositions[i].y = pivot.y;
            _swords[i] = Instantiate(_swordPrefab, _bulletPoolTransform);
            _swords[i].SetOwner(this);
            _swords[i].SetStartPosition(_swordSpawnPositions[i]);
        }

        _isActPattern = true;

        for (int i = 0; i < _swords.Length; ++i)
        {
            if (_cancelToken.IsCancellationRequested) break;
            SoundManager.Instance.EffectPlay(BelialSwordSoundName, transform.position);
            _swords[i].Spawn();
            await UniTask.Delay(200);
        }
        // 스폰
        for (int i = 0; i < _swords.Length; ++i)
        {
            if (_cancelToken.IsCancellationRequested) break;
            _swords[i].FireSword();
            await UniTask.Delay(200);
        }

        await UniTask.Delay((int)(1000 * _actPatternInterval));
        _isActPattern = false;

    }

    private async UniTaskVoid LaserFirePattern()
    {
        _isActPattern = true;
        int laserAttackCount = 3;
        while (laserAttackCount > 0)
        {
            if (_cancelToken.IsCancellationRequested) break;
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                if (_leftHand.IsUseHand == false)
                {
                    _leftHand.Attack();
                    laserAttackCount--;
                }
                else if (_rightHand.IsUseHand == false)
                {
                    _rightHand.Attack();
                    laserAttackCount--;
                }
                else
                {
                    await UniTask.Yield();
                }
            }
            else
            {
                if (_rightHand.IsUseHand == false)
                {
                    _rightHand.Attack();
                    laserAttackCount--;
                }
                else if (_leftHand.IsUseHand == false)
                {
                    _leftHand.Attack();
                    laserAttackCount--;
                }
                else
                {
                    await UniTask.Yield();
                }
            }
            await UniTask.Delay(1000);
        }

        await UniTask.Delay((int)(1000 * _actPatternInterval));
        _isActPattern = false;
    }

    #region Pool Functinon

    private Bullet CreateFunc()
    {
        Bullet v = Instantiate(_bossBullet, _bulletPoolTransform);
        v.SetOwner(_bulletPool);
        v.SetOwnerObject(gameObject);
        v.gameObject.SetActive(false);
        return v;
    }

    private void GetAction(Bullet projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void ReleaseAction(Bullet projectile)
    {
        projectile.ResetBullet();
        projectile.gameObject.SetActive(false);
    }

    private void DestroyAction(Bullet projectile)
    {
        projectile.ResetBullet();
        projectile.gameObject.SetActive(false);
    }

    #endregion
}
