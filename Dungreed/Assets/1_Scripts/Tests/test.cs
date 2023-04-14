using UnityEngine;

public class test : MonoBehaviour
{
    public Health player;

    public void Awake()
    {
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)) 
        {
            DamageObject();
        }
    }
    public void DamageObject()
    {
        Debug.Log("HealthChanged");
        int t = Random.Range(5, 30);
        GameManager.Instance.Player.GetComponentInChildren<Health>().Hit(t);
    }
}
