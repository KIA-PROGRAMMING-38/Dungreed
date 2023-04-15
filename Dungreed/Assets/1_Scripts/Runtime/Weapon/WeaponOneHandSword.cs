public class WeaponOneHandSword : WeaponMelee
{
    public override void Initialize()
    {
        base.Initialize();
        _abberationDuration = 1 / _data.AttackSpeedPerSecond;
        _cameraShakeDuration = 0.1f;
    }
    protected override void PlayCameraEffect()
    {
        GameManager.Instance.CameraManager.Effecter.PlayScreenShake(_cameraShakeDuration, 1.5f);
        GameManager.Instance.CameraManager.Effecter.PlayChromaticAbberation(_abberationDuration, 0.1f);
    }
}
