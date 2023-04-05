using UnityEngine;

public class WeaponHand : MonoBehaviour
{
    [SerializeField] private Transform _owner;

    private WeaponBase _currentWeapon;
    private bool _isFlip = false;
    void SetOwner(Transform owner) => _owner = owner;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _isFlip = !_isFlip;
            Vector3 mouseDir = _owner.position.MouseDir();
            float angle = Utils.Utility2D.DirectionToAngle(mouseDir.x, mouseDir.y);
            float offsetAngle = -90f;
            angle += offsetAngle;
            Vector2 fxPos = transform.position + (mouseDir * 1f);

            GameManager.Instance.FxPooler.GetFx("SwingFx", fxPos, Quaternion.Euler(0, 0, angle));
        }
        HandRotate();
    }

    // 무기 장착을 위한 메서드
    private void EquipWeapon()
    {

    }

    // 마우스 방향으로 무기를 회전시킬 메서드
    private void HandRotate()
    {
        // -1 : 왼쪽
        // 1 : 오른쪽
        float faceDir= Mathf.Sign(_owner.localScale.x);
        Vector2 mouseDir = _owner.position.MouseDir();
        //mouseDir = mouseDir.RotateZ(45f);
        transform.right = mouseDir;
        // transform.Rotate(0,0 )
        Vector2 scale = new Vector2(faceDir, 1);
        float y = mouseDir.x < 0 ? -1 : 1;
        scale.y = y;

        transform.localScale = _isFlip ? -1f * scale : scale;
    }
}
