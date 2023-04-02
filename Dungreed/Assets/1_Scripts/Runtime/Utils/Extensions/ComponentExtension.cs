using UnityEngine;

public static class ComponentExtension
{

    /// <summary>
    /// 현재 오브젝트 -> 자식 오브젝트 -> 부모 오브젝트 순으로 컴포넌트를 찾습니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="owner"></param>
    /// <returns></returns>
    public static T GetComponentAllCheck<T>(this Component owner) where T : Component
    {
        T comp = owner.gameObject.GetComponent<T>();

        if (comp == null)
        {
            comp = owner.gameObject.GetComponentInChildren<T>();

            if (comp == null)
            {
                comp = owner.gameObject.GetComponentInParent<T>();
            }
        }


        return comp;
    }
}
