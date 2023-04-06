using UnityEngine;

public class WeaponTwoHandSword : WeaponMelee
{
    protected void Start()
    {

    }

    public override void Attack()
    {
        Vector3 mouseDir = _hand.Owner.position.MouseDir();
        float angle = Utils.Utility2D.DirectionToAngle(mouseDir.x, mouseDir.y);
        float offsetAngle = Data.SpriteDefaultRotateAngle;
        angle += offsetAngle;
        Vector2 fxPos = _hand.transform.position + (mouseDir * Data.MeleeAttackRange);
        GameManager.Instance.FxPooler.GetFx(Data.SwingFxName, fxPos, Quaternion.Euler(0, 0, angle));

        OnAttack?.Invoke();
    }

    protected override void EnemyHitCheck()
    {
        throw new System.NotImplementedException();
    }
}
