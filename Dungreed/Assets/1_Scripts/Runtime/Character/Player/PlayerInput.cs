using UnityEngine;
public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private bool PermitVerticalMovement = false;
    public Vector2 InputVec { get; private set; }
    public float X { get=> InputVec.x;} 
    public float Y { get=> InputVec.y; }
    private static readonly string HorizontalKey = "Horizontal";
    private static readonly string VerticalKey = "Vertical";
    public bool IsInputXY
    {
        get
        {
            return InputVec != Vector2.zero;
        }
    }

    private void Update()
    {
        float x = Input.GetAxisRaw(HorizontalKey);
        float y = Input.GetAxisRaw(VerticalKey);
        Vector2 input = new Vector2(x, y);
        input.y = PermitVerticalMovement == false ? 0 : input.y;
        InputVec = input;
    }
}