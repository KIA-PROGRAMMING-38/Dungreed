using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))] 
public class LevelBounds : MonoBehaviour
{
    public bool OnDrawGizemos = true;

    [SerializeField] Bounds _boundsInfo;

    private Vector2[] _points = new Vector2[4];
    public Bounds   Bounds{get {return _boundsInfo;}}
    public Vector2  BottomLeft   { get { return new Vector2(_boundsInfo.min.x, _boundsInfo.min.y); } }
    public Vector2  BottomRight  { get { return new Vector2(_boundsInfo.max.x, _boundsInfo.min.y); } }
    public Vector2  TopLeft      { get { return new Vector2(_boundsInfo.min.x, _boundsInfo.max.y); } }
    public Vector2  TopRight     { get { return new Vector2(_boundsInfo.max.x, _boundsInfo.max.y); } }

    private PolygonCollider2D _polygonCollider2D;


    private void Awake()
    {
        _polygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void OnDrawGizmos()
    {
        if (this.OnDrawGizemos == false || Application.isPlaying == false) return;

        _boundsInfo = _polygonCollider2D.bounds;

        // $0  $1 
        // $2  $3
        _points[0] = new Vector2(_boundsInfo.min.x, _boundsInfo.max.y);
        _points[1] = new Vector2(_boundsInfo.max.x, _boundsInfo.max.y);
        _points[2] = new Vector2(_boundsInfo.min.x, _boundsInfo.min.y);
        _points[3] = new Vector2(_boundsInfo.max.x, _boundsInfo.min.y);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(_points[0], _points[1]);
        Gizmos.DrawLine(_points[0], _points[2]);
        Gizmos.DrawLine(_points[2], _points[3]);
        Gizmos.DrawLine(_points[1], _points[3]);
    }
}
