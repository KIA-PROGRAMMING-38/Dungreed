using System;
using UnityEngine;
using UnityEngine.Rendering;

public class WeaponHand : MonoBehaviour
{
    [field: SerializeField] public Transform Owner { get; private set; }
    private PlayerController _ownerController;
    private PlayerStatus _ownerStatus;

    public PlayerStatus OwnerStatus 
    { 
        get 
        {
            if(_ownerStatus == null)
            {
                _ownerStatus = Owner.GetComponent<PlayerData>().Status;
            }
            return _ownerStatus;
        } 
    }

    [SerializeField] private SortingGroup _sortingGroup;

    private SpriteRenderer _ownerRenderer;

    [SerializeField] private WeaponBase _equippedWeapon;
    [SerializeField] private WeaponData _equippedWeaponData;

    private float _faceDirX;
    private Vector2 _mouseDir;
    private bool _isFlip = false;
    public void SetOwner(Transform owner) => Owner = owner;

    private float _attackTimer;
    private float _attackSpeedPerSecond;
    private bool _canAttack;

    public event Action<float> OnReload;
    public event Action OnWeaponChanged;

    public void Reload(float reloadTime)
    {
        OnReload?.Invoke(reloadTime);
    }


    private void Awake()
    {
        _ownerRenderer = Owner.GetComponentAllCheck<SpriteRenderer>();
        _ownerController = Owner.GetComponent<PlayerController>();
        _sortingGroup = GetComponent<SortingGroup>();
        _canAttack = true;
        _attackTimer = 0f;
    }

    private void Start()
    {
      
    }

    private void Update()
    {
        if (_ownerController.StopControll) return;

        if (_canAttack == false)
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer < 0f)
            {
                _canAttack = true;
                _attackTimer = _attackSpeedPerSecond;
            }
        }

        if (Input.GetMouseButtonDown(0) && _canAttack == true)
        {
            _equippedWeapon?.Attack();
            FlipTriggerOn();
            _canAttack = false;
        }



        HandRotate();
        _equippedWeapon?.WeaponHandle();
    }

    public void EquipWeapon(WeaponBase weapon)
    {
        if (_equippedWeapon != null)
        {
            UnEquipWeapon();
        }

        SetWeaponData(weapon);
        InitWeapon(weapon);
    }

    public void UnEquipWeapon()
    {
        if (_equippedWeapon != null)
        {
            OnWeaponChanged?.Invoke();
            WeaponManager.Instance.ReleaseWeapon(_equippedWeapon);
        }

        _equippedWeaponData = null;
        _equippedWeapon = null;
    }


    public void SetWeaponData(WeaponBase weapon)
    {
        _equippedWeaponData = weapon.Data;
    }

    public void InitWeapon(WeaponBase weapon)
    {
        _equippedWeapon = weapon;
        _equippedWeapon.transform.localPosition = _equippedWeaponData.OffsetInitPosition;
        _equippedWeapon.SetHand(this);
        _equippedWeapon.Initialize();

        _canAttack = true;
        _attackSpeedPerSecond = 1f / weapon.Data.AttackSpeedPerSecond;
        _attackTimer = _attackSpeedPerSecond;

        if (_equippedWeaponData?.AttackType == EnumTypes.WeaponAttackType.Ranged)
        {
            _sortingGroup.sortingOrder = _ownerRenderer.sortingOrder + 1;
        }
    }

    private void HandRotate()
    {
        _faceDirX = Mathf.Sign(Owner.localScale.x);
        _mouseDir = Owner.transform.position.MouseDir();

        transform.right = transform.position.MouseDir();

        if (_equippedWeaponData?.AttackType == EnumTypes.WeaponAttackType.Melee)
        {
            float offsetAngle = _equippedWeaponData.RotateAngleOffset;
            transform.Rotate(0, 0, offsetAngle);
            Vector3 scale = new Vector2(_faceDirX, 1);
            scale.y = _mouseDir.x < 0 ? -1 : 1;
            transform.localScale = _isFlip ? -(scale) : scale;
        }
        else if (_equippedWeaponData?.AttackType == EnumTypes.WeaponAttackType.Ranged)
        {
            transform.localScale = new Vector3(_faceDirX, _faceDirX);
        }
    }

    private void FlipTriggerOn()
    {
        if (_equippedWeaponData?.AttackType == EnumTypes.WeaponAttackType.Melee)
        {
            _isFlip = !_isFlip;
            _sortingGroup.sortingOrder = _isFlip ? _ownerRenderer.sortingOrder + 1 : _ownerRenderer.sortingOrder - 1;
        }
    }

    private void OnDestroy()
    {
        WeaponManager.Instance.ReleaseWeapon(_equippedWeapon);
    }
}
