using JetBrains.Annotations;
using UnityEngine;

public class test : MonoBehaviour
{
    public Health player;


    public void Awake()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameManager.Instance.Player.GetComponent<Health>().Hit(Random.Range(5, 20), gameObject);
        }
    }

    public void Test() { }

}
