using JetBrains.Annotations;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject OBJ;
    public Transform Start;
    public Transform End;
    public AnimationCurve curve;

    [Range(0, 1)] public float t;

    public void Awake()
    {
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.X)) 
        {
           
        }
        OBJ.transform.position = Vector3.Lerp(Start.position, End.position, curve.Evaluate(t));
    }

}
