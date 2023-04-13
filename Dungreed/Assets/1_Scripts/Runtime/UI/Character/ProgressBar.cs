using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] protected Image _progressBarImage;

    public virtual void UpdateProgressBar(float ratio) { }
    public virtual void UpdateProgressBar(int cur, int max) { }
}
