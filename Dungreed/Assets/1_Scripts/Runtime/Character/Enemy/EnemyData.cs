using EnumTypes;
using UnityEngine;

[CreateAssetMenu(fileName ="EnemyData", menuName = "Data/Enemy")]
public class EnemyData : ScriptableObject
{
    public string       Name;
    public int          MaxHp;
    public int          MoveSpeed;   
    public int          AttackDamage;
    public EnemyType    Type;
}
