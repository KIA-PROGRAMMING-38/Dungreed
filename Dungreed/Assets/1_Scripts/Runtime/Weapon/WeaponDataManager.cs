using UnityEngine;

public class WeaponDataManager : MonoBehaviour
{
    public WeaponData[] WeaponData;

    public void Awake()
    {
        WeaponData = Resources.LoadAll<WeaponData>("ScriptableObjects/WeaponData");
    }
}
