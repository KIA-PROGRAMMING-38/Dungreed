using UnityEngine;

public static class TransformExtension
{
    // 마우스가 내 왼쪽에 있는지 오른쪽에 있는지
    public static bool IsMouseOnLeft(this Transform pivot)
    {
        Vector3 mouseVec = Utils.Utility2D.GetMousePosition();
        Vector2 dir = (mouseVec - pivot.position).normalized;
        float dot = Vector2.Dot(pivot.right, dir);
        return dot < 0f;
    }
}