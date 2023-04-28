using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    private void Awake()
    {
        SoundManager.Instance.BGMPlay("Title_BGM");
    }
}
