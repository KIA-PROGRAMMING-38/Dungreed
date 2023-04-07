using UnityEngine.Pool;
using UnityEngine;

public class ProjectTilePooler : MonoBehaviour
{
    public ObjectPool<ProjectTile> pool;

    [SerializeField] ProjectTile _defaultProjectTilePrefab;

    private void Awake()
    {
         pool = new ObjectPool<ProjectTile>(CreateProjectTile, ActionOnGet, ActionOnRelease, ActionOnDestroy, true, 20, 100);
    }

    public ProjectTile Get()
    {
        return pool.Get();
    }

    public ProjectTile CreateProjectTile()
    {
        var pt = Instantiate(_defaultProjectTilePrefab, Vector3.zero, Quaternion.identity, transform);
        pt.SetOwner(pool);
        pt.gameObject.SetActive(false);
        return pt;
    }

    public void ActionOnGet(ProjectTile projectTile) 
    {
        projectTile.gameObject.SetActive(true);
    }

    public void ActionOnRelease(ProjectTile projectTile) 
    {
        projectTile.Reset();
        projectTile.gameObject.SetActive(false);
    }
    public void ActionOnDestroy(ProjectTile projectTile) 
    {
        projectTile.gameObject.SetActive(false);
    }
}
