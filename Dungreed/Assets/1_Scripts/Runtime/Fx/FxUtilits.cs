using UnityEngine;

public class FxUtilits : MonoBehaviour
{
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer= GetComponent<SpriteRenderer>();
    }
    public void ChangeLayerOrder(int order)
    {
        _spriteRenderer.sortingOrder = order;
    }
    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
    public void DestroyFx()
    {
        Destroy(gameObject);
    }    
    public void DestroyParent()
    {
        Destroy(transform.gameObject);
    }

    public void DestroyRealParent()
    {
        Destroy(transform.parent.gameObject);
    }

    public void ResetFx()
    {
        _animator?.SetTrigger(ID_AnimationResetTrigger);
    }

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    static int ID_AnimationResetTrigger = Animator.StringToHash("Reset");
}
