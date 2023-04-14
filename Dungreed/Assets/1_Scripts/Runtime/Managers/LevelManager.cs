using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [field: SerializeField]
    LevelBounds CurrentLevelBounds { get; set; }
}
