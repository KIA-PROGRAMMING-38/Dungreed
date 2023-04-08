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
    }

    private void UpdateLaserPosition()
    {
        _laserStartPoint = _laserTransform.position;
        _laser.SetPosition(0, _laserStartPoint);
        _laser.SetPosition(1, _laserEndPoint);
    }
    public override void Attack()
    {
        base.Attack();

        if (_isReloading == true) return;
            CreateFx();
    }
    public override void WeaponHandle()
    {
        base.WeaponHandle();
        UpdateLaserPosition();
        RaycastHit2D hit = Physics2D.Raycast(_laserStartPoint, _hand.transform.right, 10f, _laserCollisionMask);

        if(hit.collider != null)
        {
            _laserEndPoint = hit.point;
        }
        else
        {
            _laserEndPoint = _laserStartPoint + _laserStartPoint.MouseDir() * 100f;
        }
    }

    private void CreateFx()
    {
        Vector2 mouseDir =  _hand.transform.position.MouseDir();
        float angle = -90f + Utils.Utility2D.DirectionToAngle(mouseDir.x, mouseDir.y);
        Debug.Log($"Angle : {angle}");
        Quaternion rot = Quaternion.Euler(0, 0, angle);
        var fx = GameManager.Instance.FxPooler.GetFx(_fireFxName, _firePosition.position, rot);
        var fx2 = GameManager.Instance.FxPooler.GetFx(_fireAfterFxName, _firePosition.position, rot);
    }
}
