public class WeaponTwoHandGun : WeaponRanged
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
        GameManager.Instance.CameraEffectManager.PlayChromaticAbberation(0.25f, 0.4f);
        GameManager.Instance.CameraEffectManager.PlayScreenShake(0.25f, 0.3f);
    }
}
