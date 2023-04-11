using UnityEngine;

public class test : MonoBehaviour
{
    public void Awake()
    {
        gameObject.SetActive(false);
    }

    public void OnObject()
    {
        Debug.Log($"OnObject");
        gameObject.SetActive(true);
    }

    public void OffObject()
    {
        gameObject.SetActive(false);
    }
}
