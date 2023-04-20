using UnityEngine;

[CreateAssetMenu(fileName ="Projectile", menuName = "Data/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public EnumTypes.ProjectileType Type;
    public string SpritePath;
    public string HitFxPath;
    public string PrefabPath;

    public float SpriteAngleOffset;
    public int Speed;
    public int Range;
}
