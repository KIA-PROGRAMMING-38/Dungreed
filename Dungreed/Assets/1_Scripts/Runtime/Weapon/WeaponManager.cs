using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Dictionary<int, WeaponBase> _weapons;

    private void Awake()
    {
        WeaponBase[] weapons = Resources.LoadAll<WeaponBase>(Path.Combine(ResourcePath.DefaultPrefabsPath, "Weapon"));
        _weapons = new Dictionary<int, WeaponBase>();
        foreach(var weapon in weapons)
        {
            WeaponData data = weapon.Data;
            _weapons.Add(data.Id, weapon);
        }
    }

    public WeaponBase GetWeapon(int id)
    {
        bool success = _weapons.TryGetValue(id, out var weaponData);
        Debug.Assert(success == true, "DD");
        return weaponData;
    }


    public void OnDestroy()
    {
        _weapons.Clear();  
    }
}
