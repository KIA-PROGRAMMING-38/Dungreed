public class WeaponOneHandSword : WeaponMelee
{
    protected override void CameraEffect()
    {
        GameManager.Instance.CameraEffectManager.PlayScreenShake(0.1f, 0.05f);
        GameManager.Instance.CameraEffectManager.PlayChromaticAbberation(0.1f, 0.1f);
    }
}
