using UnityEngine;

public class test : MonoBehaviour
{
    public Health player;
    public void Awake()
    {
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("HealthChanged");
            int t = Random.Range(5, 30);
            player.Hit(t);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("HealthChanged");
            player.Heal(30);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("HealthChanged");
            player.Revive();
        }
    }
}
