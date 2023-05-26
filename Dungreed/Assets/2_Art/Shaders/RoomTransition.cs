using Cysharp.Threading.Tasks;
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
    // Start is called before the first frame update
    void Start()
    {
        TransitionTask().Forget();
    }

    private async UniTaskVoid TransitionTask()
    {
        float elapsedTime = 0f;
        roomTransitionMaterial.SetFloat(propertyName, 0f);
        await UniTask.Delay(100);
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            roomTransitionMaterial.SetFloat(propertyName, Mathf.Clamp01(elapsedTime / transitionTime));
            await UniTask.Yield();
        }
        OnTransitionDone?.Invoke();
    }
}
