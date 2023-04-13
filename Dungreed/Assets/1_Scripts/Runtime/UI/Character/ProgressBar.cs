using UnityEngine;
using UnityEngine.UI;

public abstract class ProgressBar : MonoBehaviour, IProgressBar
{
    [SerializeField] protected Image _progressBarImage;

    public abstract void UpdateProgressBar(float ratio);
}
