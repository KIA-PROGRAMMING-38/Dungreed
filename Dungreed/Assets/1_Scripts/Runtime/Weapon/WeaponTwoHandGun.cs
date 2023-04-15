public class WeaponTwoHandGun : WeaponRanged
{
    public override void Initialize()
    {
        base.Initialize();
        _abberationDuration = 1 / _data.AttackSpeedPerSecond * 0.7f;
        _cameraShakeDuration = 0.2f;
    }

    public override void Attack()
    {
        base.Attack();
    }

    protected override void PlayCameraEffect()
    {
        GameManager.Instance.CameraManager.Effecter.PlayScreenShake(_cameraShakeDuration, 0.3f);
        GameManager.Instance.CameraManager.Effecter.PlayChromaticAbberation(_abberationDuration, 0.4f);
    }
}
