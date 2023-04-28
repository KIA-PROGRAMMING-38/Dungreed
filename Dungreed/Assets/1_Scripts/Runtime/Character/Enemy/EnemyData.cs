using EnumTypes;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData", menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    public string       Name;
    public string       Desription;
    public string       AttackSoundName;
    public string       DieSoundName;
    public int          MaxHp;
    public int          MoveSpeed;   
    public int          AttackDamage;
    public ProjectileData ProjectileData;
    public EnemyType    Type;
}
