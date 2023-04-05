using UnityEngine;

public class WeaponOneHandSword : WeaponBase
{
    public override void Attack()
    {
        Vector3 mouseDir = _hand.Owner.position.MouseDir();
        float angle = Utils.Utility2D.DirectionToAngle(mouseDir.x, mouseDir.y);
        float offsetAngle = Data.SpriteDefaultRotateAngle;
        angle += offsetAngle;
        Vector2 fxPos = _hand.transform.position + (mouseDir * 1f);

        GameManager.Instance.FxPooler.GetFx("SwingFx", fxPos, Quaternion.Euler(0, 0, angle));
    }
}
