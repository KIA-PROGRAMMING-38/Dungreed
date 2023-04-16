using UnityEngine;

public abstract class WeaponMelee : WeaponBase
{
    protected Collider2D[] _hit = new Collider2D[20];

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Attack() 
    {
        Vector3 mouseDir = _hand.Owner.position.MouseDir();
        float angle = Utils.Utility2D.DirectionToAngle(mouseDir.x, mouseDir.y);
        float offsetAngle = Data.SpriteDefaultRotateAngle;
        angle += offsetAngle;
        Vector2 fxPos = _hand.transform.position + (mouseDir * Data.MeleeAttackRange);
        GameManager.Instance.FxPooler.GetFx(Data.SwingFxName, fxPos, Quaternion.Euler(0, 0, angle));

        EnemyHitCheck(ref mouseDir);
        PlayCameraEffect();
    }

    // 공격 시 근접 공격 범위안에 적이 있는지
    protected virtual void EnemyHitCheck(ref Vector3 mouseDirection)
    {
        float halfRange = Data.MeleeAttackRange * 0.5f;
        Vector2 point = _hand.transform.position + (mouseDirection * halfRange);
        int hitCount = Physics2D.OverlapCircleNonAlloc(point, halfRange, _hit, Globals.LayerMask.Enemy | Globals.LayerMask.Prop);
        for (int i = 0; i < hitCount; i++)
        {
            IDamageable obj = _hit[i].GetComponent<IDamageable>();
            
            int weaponDamage = Random.Range(Data.MinDamage, Data.MaxDamage + 1);
            // 플레이어의 위력을 가져온다
            // int playerState =  _hand.Owner.Status.
            int totalDamage = weaponDamage /* + AddtionalDamage */;

            obj?.Hit(totalDamage, _hand.Owner.gameObject);
        }
    }
}
