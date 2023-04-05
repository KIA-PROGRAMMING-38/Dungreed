using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponDataManager : MonoBehaviour
{
    public WeaponData[] WeaponData;

    public Dictionary<int, WeaponData> _weaponDatas;

    public void Awake()
    {
        WeaponData = Resources.LoadAll<WeaponData>("ScriptableObjects/WeaponData");
        _weaponDatas = new Dictionary<int, WeaponData>();
        foreach(var data in WeaponData)
        {
            _weaponDatas.Add(data.Id, data);
        }
    }

    public WeaponData GetWeaponData(int id)
    {
        bool success = _weaponDatas.TryGetValue(id, out var weaponData);
        Debug.Assert(success == true, "DD");
        return weaponData;
    }


    public void OnDestroy()
    {
        _weaponDatas.Clear();  
    }
}
