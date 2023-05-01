using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D _cursor;
    private Vector2 hotspot;
    private void Start()
    {
        hotspot = new Vector2( _cursor.width / 2, _cursor.height / 2 );
        Cursor.SetCursor(_cursor, hotspot, CursorMode.ForceSoftware);
    }

}
