using UnityEngine;

[CreateAssetMenu(fileName ="ProjectTile", menuName = "Data/ProjectTileData")]
public class ProjectTileData : ScriptableObject
{
    public string SpritePath;
    public string HitFxPath;
    public int Speed;
    public int Range;
}
