using UnityEngine;
using UnityEngine.Rendering;

public class WeaponHand : MonoBehaviour
{
    // test
    public int initId = 0;
    [field: SerializeField] public Transform Owner { get; private set; }
    

    [SerializeField] private SortingGroup _sortingGroup;

    private SpriteRenderer _ownerRenderer;

    [SerializeField] private WeaponBase _equippedWeapon;
    [SerializeField] private WeaponData _equippedWeaponData;

    private float _faceDirX;
    private Vector2 _mouseDir;
    private bool _isFlip = false;
    void SetOwner(Transform owner) => Owner = owner;

    private float _attackTimer;
    private float _attackSpeedPerSecond;
    private bool _canAttack;

    private void Awake()
    {
        _ownerRenderer = Owner.GetComponentAllCheck<SpriteRenderer>();
        _sortingGroup = GetComponent<SortingGroup>();
        _canAttack = true;
        _attackTimer = 0f;
    }

    private void Start()
    {
        if (_equippedWeaponData == null)
            EquipWeapon(GameManager.Instance.WeaponDataManager.GetWeaponData(initId));
    }

    private void Update()
    {
        if(_canAttack == false)
        {
            _attackTimer -= Time.deltaTime;
            if(_attackTimer < 0f)
            {
                _canAttack = true;
                _attackTimer = _attackSpeedPerSecond;
            }
        }

        if(Input.GetMouseButtonDown(0) && _canAttack == true)
        {
            _equippedWeapon.Attack();
            FlipTriggerOn();
            _canAttack = false;
        }

        // Test Code
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log(0);
            EquipWeapon(GameManager.Instance.WeaponDataManager.GetWeaponData(0));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(GameManager.Instance.WeaponDataManager.GetWeaponData(1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(GameManager.Instance.WeaponDataManager.GetWeaponData(2));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EquipWeapon(GameManager.Instance.WeaponDataManager.GetWeaponData(4));
        }

        HandRotate();
        _equippedWeapon?.WeaponHandle();
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
        _equippedWeapon.Initialize();

        _canAttack = true;
        _attackSpeedPerSecond = 1f / data.AttackSpeedPerSecond;
        _attackTimer = _attackSpeedPerSecond;

        if (_equippedWeaponData?.AttackType == EnumTypes.WeaponAttackType.Ranged)
        {
            _sortingGroup.sortingOrder = _ownerRenderer.sortingOrder + 1;
        }
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
            transform.localScale = new Vector3(_faceDirX, _faceDirX);
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
