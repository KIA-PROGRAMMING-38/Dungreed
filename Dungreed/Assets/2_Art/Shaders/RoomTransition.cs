using System;
using System.Collections;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    [SerializeField]
    private Material roomTransitionMaterial;

    [SerializeField]
    private float transitionTime = 1f;

    [SerializeField]
    private string propertyName = "_Progress";

    public event Action OnTransitionDone;

    private IEnumerator _transitionCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        _transitionCoroutine = TransitionCoroutine();
        StartCoroutine(_transitionCoroutine);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(_transitionCoroutine);
        }
    }
    private IEnumerator TransitionCoroutine()
    {
        while(true)
        {
            float elapsedTime = 0f;
            roomTransitionMaterial.SetFloat(propertyName, 0f);
            yield return YieldCache.WaitForSeconds(0.1f);
            while (elapsedTime < transitionTime)
            {
                elapsedTime += Time.deltaTime;
                roomTransitionMaterial.SetFloat(propertyName, Mathf.Clamp01(elapsedTime / transitionTime));
                yield return null;
            }
            OnTransitionDone?.Invoke();
            StopCoroutine(_transitionCoroutine);
            yield return null;
        }
    }
}
