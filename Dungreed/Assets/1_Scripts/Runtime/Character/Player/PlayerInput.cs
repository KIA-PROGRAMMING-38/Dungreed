using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private bool PermitVerticalMovement = false;
    public Vector2 InputVec { get; private set; }
    public float X { get=> InputVec.x;} 
    public float Y { get=> InputVec.y; }
    public bool IsInputXY
    {
        get
        {
            return InputVec != Vector2.zero;
        }
    }

    void OnMove(InputValue val)
    {
        Vector2 input = val.Get<Vector2>();
        input.y = PermitVerticalMovement == false ? 0 : input.y;
        InputVec = input;
    }
}