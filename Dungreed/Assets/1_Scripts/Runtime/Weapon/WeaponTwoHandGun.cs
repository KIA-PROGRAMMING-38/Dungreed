public class WeaponTwoHandGun : WeaponRanged
{
    public override void Initialize()
    {
        base.Attack();
        // 두손 총은 화면진동 더 쎄게
        // OnAttack =  ??;
    }
}
