using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public void AddWeapon(WeaponData weapon)
    {
        Container.Add(new InventorySlot(weapon));
    }
}

[Serializable]
public class InventorySlot
{
    public WeaponData item;
    public InventorySlot(WeaponData weapon)
    {
        item = weapon;
    }
}
