using UnityEngine;

public static class VectorExtension
{
    public static float AngleZ(this Vector2 angle)
    {
        if (angle == Vector2.zero)
        {
            return 0f;
        }
        return Vector2.Angle(Vector2.up, angle) * Mathf.Sign(-angle.x);
    }
    public static float AngleZ(this Vector3 angle)
    {
        if (angle == Vector3.zero)
        {
            return 0f;
        }
        return Vector2.Angle(Vector2.up, angle) * Mathf.Sign(-angle.x);
    }
    public static Vector2 RotateZ(this Vector2 v, float angle)
    {
        float num = Mathf.Sin(angle * 0.017453292f);
        float num2 = Mathf.Cos(angle * 0.017453292f);
        float x = v.x;
        float y = v.y;
        return new Vector2(num2 * x - num * y, num2 * y + num * x);
    }
    public static Vector3 RotateZ(this Vector3 v, float angle)
    {
        float num = Mathf.Sin(angle * 0.017453292f);
        float num2 = Mathf.Cos(angle * 0.017453292f);
        float x = v.x;
        float y = v.y;
        return new Vector3(num2 * x - num * y, num2 * y + num * x, v.z);
    }
    public static Vector2 Rotate90(this Vector2 v)
    {
        return new Vector2(-v.y, v.x);
    }
}

