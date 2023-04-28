using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    public static partial class Utility2D
    {
        public static Vector2 GetMousePosition()
        {
            return GameManager.Instance.CameraManager.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        }
        public static float GetAngle(Vector2 lhs, Vector2 rhs){
            float angle = Vector2.Angle(lhs - rhs, Vector2.up);
            if(lhs.x > rhs.x)
            {
                angle *= -1f;
            }
            return angle;
        }
        public static float GetAngleFromVector(Vector2 vec)
        {
            return GetAngle(vec, Vector2.zero);
        }
        public static Vector3 GetVectorFromAngle(float angle)
        {
            Vector3 res = Vector3.zero;
            res.x = -Mathf.Sin(angle / 180f * Mathf.PI);
            res.y = Mathf.Cos(angle / 180f * Mathf.PI);
            return res;
        }

        public static float DirectionToAngle(float x, float y)
        {
            float cos = x;
            float sin = y;
            return Mathf.Atan2(sin, cos) * Mathf.Rad2Deg;
        }

        public static void SetTimeScale(float timescale)
        {
            Time.timeScale = timescale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        public static int GenerateID<T>()
        {
            return GenerateID(typeof(T));
        }
        public static int GenerateID(System.Type type)
        {
            return Animator.StringToHash(type.Name);
        }
    }

}
