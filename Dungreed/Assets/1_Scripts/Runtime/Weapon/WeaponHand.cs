using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class WeaponHand : MonoBehaviour
{
    [field: SerializeField] public Transform Owner { get; private set; }
    

    [SerializeField] private SortingGroup _sortingGroup;

    private SpriteRenderer _ownerRenderer;

    [SerializeField] private WeaponBase _equippedWeapon;
    [SerializeField] private WeaponData _equippedWeaponData;

    private float _faceDirX;
    private Vector2 _mouseDir;
    private bool _isFlip = false;
    void SetOwner(Transform owner) => Owner = owner;
    

    private void Awake()
    {
        _ownerRenderer = Owner.GetComponentAllCheck<SpriteRenderer>();
        _sortingGroup = GetComponent<SortingGroup>();
    }

    private void Start()
    {
        if (_equippedWeaponData == null)
            EquipWeapon(GameManager.Instance.WeaponDataManager.GetWeaponData(2));
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _equippedWeapon.Attack();
            FlipTriggerOn();
        }
        HandRotate();
    }

    // 무기 장착을 위한 메서드
    public void EquipWeapon(WeaponData weaponData)
    {
        if(_equippedWeapon != null)
        {
            Destroy(_equippedWeapon.gameObject);
        }

        SetWeaponData(weaponData);
        InitWeapon(weaponData);
    }

    public void SetWeaponData(WeaponData data)
    {
        _equippedWeaponData = data;
    }

    public void InitWeapon(WeaponData data)
    {
        _equippedWeapon = Instantiate(data.Prefab, transform).GetComponent<WeaponBase>();
        _equippedWeapon.transform.localPosition = _equippedWeaponData.OffsetInitPosition;
        _equippedWeapon.SetHand(this);
    }

    // 마우스 방향으로 무기를 회전시킬 메서드
    private void HandRotate()
    {
        // -1 : 왼쪽
        // 1 : 오른쪽
        _faceDirX  = Mathf.Sign(Owner.localScale.x);
        _mouseDir = Owner.position.MouseDir();
        transform.right = _mouseDir;

        if (_equippedWeaponData?.AttackType == EnumTypes.WeaponAttackType.Melee)
        {
            float offsetAngle = _equippedWeaponData.RotateAngleOffset;
            transform.Rotate(0, 0, offsetAngle);
            Vector3 scale = new Vector2(_faceDirX, 1);
            scale.y = _mouseDir.x < 0 ? -1 : 1;
            transform.localScale = _isFlip ? -(scale) : scale;
        }
        else if(_equippedWeaponData?.AttackType == EnumTypes.WeaponAttackType.Ranged)
        {
            transform.localScale = new Vector3(_faceDirX, 1);
        }
    }

    private void FlipTriggerOn()
    {
        if(_equippedWeaponData?.AttackType == EnumTypes.WeaponAttackType.Melee)
        {
            _isFlip = !_isFlip;
            _sortingGroup.sortingOrder = _isFlip ? _ownerRenderer.sortingOrder + 1 : _ownerRenderer.sortingOrder - 1;
        }
    }
}
