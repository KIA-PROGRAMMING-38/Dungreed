public class WeaponTwoHandSword : WeaponMelee
{

    public override void Attack()
    {
        base.Attack();
        GameManager.Instance.CameraEffectManager.PlayChromaticAbberation(0.1f, 0.3f);
        GameManager.Instance.CameraEffectManager.PlayScreenShake(0.13f, 0.1f);
    }
}
