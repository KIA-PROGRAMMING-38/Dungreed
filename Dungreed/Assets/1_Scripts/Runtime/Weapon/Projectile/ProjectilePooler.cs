using UnityEngine.Pool;
using UnityEngine;
using System.IO;

public class ProjectilePooler : MonoBehaviour
{
    public ObjectPool<Projectile> pool;

    Projectile _defaultProjectTilePrefab;

    private void Awake()
    {
        _defaultProjectTilePrefab = ResourceCache.GetResource<Projectile>(Path.Combine(ResourcePath.DefaultPrefabsPath, "DefaultProjectile"));
         pool = new ObjectPool<Projectile>(CreateProjectTile, ActionOnGet, ActionOnRelease, ActionOnDestroy, true, 20, 100);
    }

    public Projectile Get()
    {
        return pool.Get();
    }

    public Projectile CreateProjectTile()
    {
        var pt = Instantiate(_defaultProjectTilePrefab, Vector3.zero, Quaternion.identity, transform);
        pt.SetOwner(pool);
        pt.gameObject.SetActive(false);
        return pt;
    }

    public void ActionOnGet(Projectile projectTile) 
    {
        projectTile.gameObject.SetActive(true);
    }

    public void ActionOnRelease(Projectile projectTile) 
    {
        projectTile.Reset();
        projectTile.gameObject.SetActive(false);
    }
    public void ActionOnDestroy(Projectile projectTile) 
    {
        projectTile.gameObject.SetActive(false);
    }
}
