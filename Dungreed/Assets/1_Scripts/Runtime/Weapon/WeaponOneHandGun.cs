public class WeaponOneHandGun : WeaponRanged
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Attack()
    {
        base.Attack();
    }

    protected override void CameraEffect()
    {
        GameManager.Instance.CameraEffectManager.PlayScreenShake(0.1f, 0.1f);
        GameManager.Instance.CameraEffectManager.PlayChromaticAbberation(0.1f, 0.1f);
    }
}
