using UnityEngine;
using UnityEngine.Events;

public class test : MonoBehaviour
{
    public UnityEvent SaveEvent;
    public UnityEvent LoadEvent;

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SaveEvent?.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            LoadEvent?.Invoke();
        }
    }
}
