using TMPro;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    WeaponHand _playerHand;

    WeaponSlot[] _slots;
    WeaponSlot _currentSlot = null;
    [SerializeField] private WeaponSlot _defaultSlot;

    public void SetPlayerHand(WeaponHand hand)
    {
        Debug.Log("SetPlayerHand");
        _playerHand = hand;
        if (_currentSlot == null)
        {
            _slots = GetComponentsInChildren<WeaponSlot>();
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].IndexText.text = (i + 1).ToString();
            }

            _currentSlot = _defaultSlot;
            _playerHand.EquipWeapon(WeaponManager.Instance.GetWeapon(_defaultSlot.Data.Id, _playerHand.transform));
            _currentSlot.Select();
        }
        else
        {
            _playerHand.EquipWeapon(WeaponManager.Instance.GetWeapon(_currentSlot.Data.Id, _playerHand.transform));
        }
    }
 

    private void Update()
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            if (Input.GetKeyDown(_slots[i].Key))
            {
                if (_currentSlot == _slots[i])
                {
                    _currentSlot?.UnSelect();
                    _playerHand.UnEquipWeapon();
                    _currentSlot = null;
                    break;
                }
                _playerHand.UnEquipWeapon();
                _currentSlot?.UnSelect();
                _currentSlot = _slots[i];
                _currentSlot?.Select();
                _playerHand.EquipWeapon(WeaponManager.Instance.GetWeapon(_slots[i].Data.Id, _playerHand.transform));
            }
        }
    }
}
