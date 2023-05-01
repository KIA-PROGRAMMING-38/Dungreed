using TMPro;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    WeaponHand _playerHand;

    WeaponSlot[] _slots;
    WeaponSlot _currentSlot = null;
    [SerializeField] private WeaponSlot _defaultSlot;
    WeaponBase _currentWeapon;

    private static readonly string SlotSwapSound = "SlotSwapSound";

    public void SetPlayerHand(WeaponHand hand)
    {
        _playerHand = hand;

        if (_currentSlot == null)
        {
            _slots = GetComponentsInChildren<WeaponSlot>();
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].IndexText.text = (i + 1).ToString();
                _slots[i].AmmoText.gameObject.SetActive(false);
            }

            _currentSlot = _defaultSlot;
            _currentWeapon = WeaponManager.Instance.GetWeapon(_defaultSlot.Data.Id, _playerHand.transform);
        }
        else
        {
            _currentWeapon = WeaponManager.Instance.GetWeapon(_currentSlot.Data.Id, _playerHand.transform);
        }

        _playerHand.EquipWeapon(_currentWeapon);
        _currentSlot.Select();

        if (_currentWeapon.Data.AttackType == EnumTypes.WeaponAttackType.Ranged)
        {
            _currentSlot?.AmmoText.gameObject.SetActive(true);
        }
    }
 

    private void Update()
    {
        if (_slots == null) return;
        if (_playerHand == null) return;
        
        for (int i = 0; i < _slots.Length; i++)
        {
            if (Input.GetKeyDown(_slots[i].Key))
            {
                if (_currentSlot == _slots[i])
                {
                    _currentSlot?.UnSelect();
                    _playerHand.UnEquipWeapon();

                    if (_currentSlot?.Data.AttackType == EnumTypes.WeaponAttackType.Ranged)
                    {
                        _currentSlot?.AmmoText.gameObject.SetActive(false);
                    }

                    _currentSlot = null;
                    break;
                }

                _playerHand.UnEquipWeapon();
                _currentSlot?.UnSelect();

                if (_currentSlot?.Data.AttackType == EnumTypes.WeaponAttackType.Ranged)
                {
                    _currentSlot?.AmmoText.gameObject.SetActive(false);
                }

                _currentSlot = _slots[i];
                _currentSlot?.Select();
                SoundManager.Instance.EffectPlay(SlotSwapSound, Vector3.zero);
                if (_currentSlot?.Data.AttackType == EnumTypes.WeaponAttackType.Ranged)
                {
                    _currentSlot?.AmmoText.gameObject.SetActive(true);
                }

                _currentWeapon = WeaponManager.Instance.GetWeapon(_currentSlot.Data.Id, _playerHand.transform);
                _playerHand.EquipWeapon(_currentWeapon);
            }
        }

        if (_currentSlot?.Data.AttackType == EnumTypes.WeaponAttackType.Ranged)
        {
            var range = _currentWeapon as WeaponRanged;
            int maxAmmo = range.Data.MaxAmmoCount;
            int curAmmo = range.CurrentAmmoCount;
            _currentSlot.AmmoText.text = $"{curAmmo} / {maxAmmo}";
        }
    }
}
