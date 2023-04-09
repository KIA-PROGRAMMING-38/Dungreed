using UnityEngine;

public class WeaponHecate : WeaponTwoHandGun
{
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

        duration = (1f / Data.AttackSpeedPerSecond);
    }

    private void UpdateLaserPosition()
    {
        _laser.SetPosition(0, _laserStartPoint);
        _laser.SetPosition(1, _laserEndPoint);
    }
    public override void Attack()
    {
        base.Attack();
        if (_isReloading == true) return;
        CreateFx();
        GameManager.Instance.CameraEffectManager.PlayChromaticAbberation(0.25f, 0.4f);
        GameManager.Instance.CameraEffectManager.PlayScreenShake(0.25f, 0.3f);
        knockback = true;
        float x = Mathf.Sign(_hand.transform.localScale.x);
        float angle = x == 1f ? 40F : -40F;
        _hand.transform.rotation = _hand.transform.rotation * Quaternion.Euler(0,0,angle);
        p = _hand.transform.right;
    }
    Vector2 p;
    bool knockback = false;
    float duration;
    float t;
    public override void WeaponHandle()
    {
        base.WeaponHandle();


        if(knockback == true)
        {
            t += Time.deltaTime;
            Vector3 mouseVec = _hand.Owner.transform.position.MouseDir();
            _hand.transform.right = Utils.Math.Utility2D.EaseInOutBounce(p, mouseVec, t / duration);
            if(t > duration)
            {
                t = 0f;
                knockback = false;
            }
        }

        _laserStartPoint = _laserTransform.position;
        RaycastHit2D hit = Physics2D.Raycast(_laserStartPoint, transform.right, 50f, _laserCollisionMask);

        if (hit.collider != null)
        {
            _laserEndPoint = hit.point;
        }
        else
        {
            _laserEndPoint = _laserStartPoint + (Vector2)transform.right * 100f;
        }

        UpdateLaserPosition();
    }

    private void CreateFx()
    {
        Vector2 mouseDir = _hand.transform.position.MouseDir();
        float angle = -90f + Utils.Utility2D.DirectionToAngle(mouseDir.x, mouseDir.y);
        Debug.Log($"Angle : {angle}");
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        var fx = GameManager.Instance.FxPooler.GetFx(_fireFxName, _firePosition.position, rot);
        var fx2 = GameManager.Instance.FxPooler.GetFx(_fireAfterFxName, _firePosition.position, rot);
    }
}
