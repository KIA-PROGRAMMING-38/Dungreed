public class WeaponTwoHandSword : WeaponMelee
{
    public override void Initialize()
    {
        base.Initialize();
        _abberationDuration = 1 / _data.AttackSpeedPerSecond * 0.5f;
        _cameraShakeDuration = 0.2f;
    }
    protected override void PlayCameraEffect()
    {
        GameManager.Instance.CameraManager.Effecter.PlayScreenShake(_cameraShakeDuration, 3f);
        GameManager.Instance.CameraManager.Effecter.PlayChromaticAbberation(_abberationDuration, 0.3f);
    }
}
