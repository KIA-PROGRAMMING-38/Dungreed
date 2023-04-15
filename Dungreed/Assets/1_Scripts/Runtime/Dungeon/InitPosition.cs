using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class InitPosition : MonoBehaviour
{
    public Vector2 Position { get { return transform.position; } }
    CircleCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _collider.enabled = false;
        gameObject.layer = LayerMask.NameToLayer(Globals.LayerName.NonCollision);
    }
}
