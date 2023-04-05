using UnityEngine;
using UnityEngine.Pool;

public class FxPooler : MonoBehaviour
{
    private void Awake()
    {
        FxPool = new ObjectPool<FxObject>(CreateFx, ActionOnGet, ActionOnRelease, ActionOnDestroy,
  true, 100, 1000);

        DefaultFxObject = Resources.Load<FxObject>("Prefabs/DefaultFxObject");
    }

    public FxObject GetFx(string name, Vector3 position, Quaternion rotate, Vector3 scale)
    {
        FxObject pooledObject = GetFx(name, position, rotate);
        pooledObject.transform.localScale = scale;

        return pooledObject;
    }

    public FxObject GetFx(string name, Vector3 position, Quaternion rotate)
    {
        var clip = AnimationClipPath.GetAnimationClipResource(name);
        var pooledObject = FxPool.Get();
        pooledObject.SetAnimationClip(clip);
        pooledObject.SetTransform(position, rotate);

        return pooledObject;
    }

    public void Release(FxObject element)
    {
        FxPool.Release(element);
    }

    public ObjectPool<FxObject> FxPool { get; private set; }

    private FxObject DefaultFxObject;


    public FxObject CreateFx()
    {
        FxObject obj = MonoBehaviour.Instantiate<FxObject>(DefaultFxObject, Vector3.zero, Quaternion.identity, transform);        
        obj.PoolOwner = this;
        return obj;
    }

    public void ActionOnGet(FxObject fx)
    {
        fx.SetActive(true);
        fx.GetComponent<SpriteRenderer>().color = Color.white;
        fx.Reset();
    }
    public void ActionOnRelease(FxObject fx)
    {
        fx.Reset();
        fx.SetActive(false);
    }
    public void ActionOnDestroy(FxObject fx)
    {
        fx.DestroyFx();
    }
}