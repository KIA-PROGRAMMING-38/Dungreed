using UnityEngine;

public static class TransformExtension
{
    // 마우스가 내 왼쪽에 있는지 오른쪽에 있는지
    public static bool IsMouseOnLeft(this Transform pivot)
    {
        Vector2 dir = pivot.position.MouseDir();
        float dot = Vector2.Dot(pivot.right, dir);
        return dot < 0f;
    }
}