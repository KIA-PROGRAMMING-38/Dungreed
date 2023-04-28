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
        SoundManager.Instance.EffectPlay("DungeonEatOn", transform.position);
    }

    public void FadeOutPlayer()
    {
        GameManager.Instance.Player.SetActive(false);
        SoundManager.Instance.EffectPlay("DungeonEatOff", transform.position);
    }

    public void EnterDungeon()
    {
        GameManager.Instance.Player.GetComponent<PlayerController>().PlayController();
        GameManager.Instance.CameraManager.Effecter.PlayTransitionEffect(()=>{ SceneManager.LoadScene(2); },
        true);
    }
}
