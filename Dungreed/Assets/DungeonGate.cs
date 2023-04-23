using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonGate : MonoBehaviour
{
    [SerializeField] private Trigger _trigger;

    private void Awake()
    {
        _trigger = GetComponentInChildren<Trigger>();
        gameObject.SetActive(false);
    }

    public void OnTriggerEnterDungeonZone()
    {
        GameManager.Instance.Player.GetComponent<PlayerController>().StopController();
        gameObject.SetActive(true);
    }

    public void FadeOutPlayer()
    {
        GameManager.Instance.Player.SetActive(false);
    }

    public void EnterDungeon()
    {
        GameManager.Instance.Player.GetComponent<PlayerController>().PlayController();
        GameManager.Instance.CameraManager.Effecter.PlayTransitionEffect(()=>{ SceneManager.LoadScene(1); },
        true);
        Debug.Log("던전 입장");
    }
}
