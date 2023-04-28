using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    public Dictionary<int, WeaponBase> _weapons;

    new protected void Awake()
    {
        base.Awake();
        WeaponBase[] weapons = Resources.LoadAll<WeaponBase>(Path.Combine(ResourcePath.DefaultPrefabsPath, "Weapon"));
        _weapons = new Dictionary<int, WeaponBase>();
        foreach(var weapon in weapons)
        {
            WeaponData data = weapon.Data;
            WeaponBase inst = Instantiate(weapon, Vector3.zero, Quaternion.identity, transform);
            inst.gameObject.SetActive(false);
            _weapons.Add(data.Id, inst);
        }
    }

    public void ReleaseWeapon(WeaponBase weapon)
    {
        weapon.transform.SetParent(transform);
        weapon.gameObject.SetActive(false);
    }

    // WeaponData의 ID를 통해 가져옴
    public WeaponBase GetWeapon(int id, Transform parent)
    {
        bool success = _weapons.TryGetValue(id, out var weapon);
        Debug.Assert(success == true, "DD");
        weapon.gameObject.SetActive(true);
        weapon.transform.SetParent(parent);
        weapon.transform.position = Vector3.zero;
        weapon.transform.rotation = Quaternion.Euler(0, 0, 0);
        weapon.transform.localRotation = Quaternion.Euler(0, 0, 0);
        weapon.transform.localScale = Vector3.one;
        return weapon;
    }


    public void OnDestroy()
    {
        _weapons?.Clear();  
    }
}
