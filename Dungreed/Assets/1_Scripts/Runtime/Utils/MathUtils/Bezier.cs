using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Math
{
    public static partial class Utility2D
    {
        // 1차 베지어 곡선
        public static Vector2 BezierCurve(Vector2 P0, Vector2 P1, float t)
        {
            return Vector2.Lerp(P0, P1, t);
        }

        // 2차 베지어 곡선
        public static Vector2 QuadraticBezierCurve(Vector2 P0, Vector2 P1, Vector2 P2, float t)
        {
            Vector2 m0 = BezierCurve(P0, P1, t);
            Vector2 m1 = BezierCurve(P1, P2, t);
            return BezierCurve(m0, m1, t);
        }

        // 3차 베지어 곡선
        public static Vector2 CubicBezierCurve(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, float t)
        {
            Vector2 m0 = BezierCurve(P0, P1, t);
            Vector2 m1 = BezierCurve(P1, P2, t);
            Vector2 m2 = BezierCurve(P2, P3, t);
            return QuadraticBezierCurve(m0, m1, m2, t);
        }


        // 4차 베지어 곡선
        public static Vector2 QuarticBezierCurve(Vector2 P0, Vector2 P1, Vector2 P2, Vector2 P3, Vector2 P4, float t)
        {
            Vector2 m0 = BezierCurve(P0, P1, t);
            Vector2 m1 = BezierCurve(P1, P2, t);
            Vector2 m2 = BezierCurve(P2, P3, t);
            Vector2 m3 = BezierCurve(P3, P4, t);
            return CubicBezierCurve(m0, m1, m2, m3, t);
        }
    }
}

