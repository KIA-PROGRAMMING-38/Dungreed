using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Globals
{
    public class LayerName
    {
        // Layer name
        public static readonly string Default = "Default";
        public static readonly string Platform = "Platform";
        public static readonly string OnewayPlatform = "OnewayPlatform";
        public static readonly string Slope = "Slope";
        public static readonly string Player = "Player";
        public static readonly string Enemy = "Enemy";
        public static readonly string Prop = "Prop";
        public static readonly string UI = "UI";

        public static readonly string NonCollision = "NonCollision";
    }

    public class LayerMask
    {
        // LayerMask Bit
        public static readonly int Default          = 1 << UnityEngine.LayerMask.NameToLayer(LayerName.Default);
        public static readonly int Platform         = 1 << UnityEngine.LayerMask.NameToLayer(LayerName.Platform);
        public static readonly int OnewayPlatform   = 1 << UnityEngine.LayerMask.NameToLayer(LayerName.OnewayPlatform);
        public static readonly int Slope            = 1 << UnityEngine.LayerMask.NameToLayer(LayerName.Slope);
        public static readonly int Player           = 1 << UnityEngine.LayerMask.NameToLayer(LayerName.Player);
        public static readonly int Enemy            = 1 << UnityEngine.LayerMask.NameToLayer(LayerName.Enemy);
        public static readonly int Prop             = 1 << UnityEngine.LayerMask.NameToLayer(LayerName.Prop);
        public static readonly int UI               = 1 << UnityEngine.LayerMask.NameToLayer(LayerName.UI);
        public static readonly int NonCollision     = 1 << UnityEngine.LayerMask.NameToLayer(LayerName.NonCollision);


        /// <summary>
        /// ���̾� �� ����� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="layerIndex">���̾� �ε��� ��ȣ</param>
        /// <param name="rhs">���̾� ����ũ</param>
        /// <returns>���̾� �� ���</returns>
        public static bool CompareMask(int layerIndex, UnityEngine.LayerMask rhs)
        {
            int a = 1 << layerIndex;
            int b = rhs.value;

            return (a & b) != 0;
        }

        /// <summary>
        /// ���̾� �� ����� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="layerIndex">���̾� ����ũ</param>
        /// <param name="rhs">���̾� ����ũ</param>
        /// <returns>���̾� �� ���</returns>
        public static bool CompareMask(UnityEngine.LayerMask lhs, UnityEngine.LayerMask rhs)
        {
            return (lhs & rhs) != 0;
        }
    }

    public class TagLiteral
    {
        public static readonly string InvisibleTile = "InvisibleTile";
        public static readonly string OnewayPlatform = "OnewayPlatform";
    }
}
