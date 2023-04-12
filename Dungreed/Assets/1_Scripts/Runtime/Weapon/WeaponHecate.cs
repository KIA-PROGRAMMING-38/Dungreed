using UnityEngine;

public class WeaponHecate : WeaponTwoHandGun
{
    [SerializeField] private ParticleSystem _laserHitParticle;
    [SerializeField] private Transform _laserTransform;
    [SerializeField] LayerMask _laserCollisionMask;
    [SerializeField] string _fireAfterFxName;
    [SerializeField] string _fireFxName;

    private LineRenderer _laser;
    private Vector2 _laserStartPoint;
    private Vector2 _laserEndPoint;

    public override void Initialize()
    {
        base.Initialize();
        _laser = _laserTransform.GetComponent<LineRenderer>();
    }

    private void UpdateLaserPosition()
    {
        _laser.SetPosition(0, _laserStartPoint);
        _laser.SetPosition(1, _laserEndPoint);
    }
    public override void Attack()
    {
        base.Attack();
    }

    protected override void Fire()
    {
        CreateFx();
        base.Fire();
    }

    public override void WeaponHandle()
    {
        base.WeaponHandle();

        _laserStartPoint = _laserTransform.position;
        RaycastHit2D hit = Physics2D.Raycast(_laserStartPoint, transform.right, 50f, _laserCollisionMask);

        if (hit.collider != null)
        {
            _laserEndPoint = hit.point;
            _laserHitParticle.transform.position = hit.point;
            if (_laserHitParticle.isStopped == true)
            {
                _laserHitParticle.Play();
            }
        }
        else
        {
            _laserEndPoint = _laserStartPoint + (Vector2)transform.right * 100f;
            if (_laserHitParticle.isPlaying == true)
            {
                _laserHitParticle.Stop();
            }
        }

        UpdateLaserPosition();
    }

    private void CreateFx()
    {
        Vector2 mouseDir = transform.position.MouseDir();
        float angle = -90f + Utils.Utility2D.DirectionToAngle(mouseDir.x, mouseDir.y);
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        GameManager.Instance.FxPooler.GetFx(_fireFxName, _firePosition.position, rot);
        GameManager.Instance.FxPooler.GetFx(_fireAfterFxName, _firePosition.position, rot);
    }

    protected override void CameraEffect()
    {
        GameManager.Instance.CameraEffectManager.PlayChromaticAbberation(1f/ Data.AttackSpeedPerSecond, 0.7f);
        GameManager.Instance.CameraEffectManager.PlayScreenShake(0.15f, 0.3f);
    }
}
