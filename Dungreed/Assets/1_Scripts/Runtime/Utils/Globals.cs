namespace Globals
{
    public class LayerName
    {
        // Layer name
        public static readonly string Default = "Default";
        public static readonly string Platform = "Platform";
        public static readonly string OnewayPlatform = "OnewayPlatform";
        public static readonly string Player = "Player";
        public static readonly string Enemy = "Enemy";
        public static readonly string Prop = "Prop";
        public static readonly string UI = "UI";
        public static readonly string Item = "Item";
        public static readonly string NonCollision = "NonCollision";
    }

    public class LayerMask
    {
        // LayerMask Bit
        public static readonly int Default          = UnityEngine.LayerMask.GetMask(LayerName.Default);
        public static readonly int Platform         = UnityEngine.LayerMask.GetMask(LayerName.Platform);
        public static readonly int OnewayPlatform   = UnityEngine.LayerMask.GetMask(LayerName.OnewayPlatform);
        public static readonly int Player           = UnityEngine.LayerMask.GetMask(LayerName.Player);
        public static readonly int Enemy            = UnityEngine.LayerMask.GetMask(LayerName.Enemy);
        public static readonly int Prop             = UnityEngine.LayerMask.GetMask(LayerName.Prop);
        public static readonly int Item             = UnityEngine.LayerMask.GetMask(LayerName.Item);
        public static readonly int UI               = UnityEngine.LayerMask.GetMask(LayerName.UI);
        public static readonly int NonCollision     = UnityEngine.LayerMask.GetMask(LayerName.NonCollision);


        /// <summary>
        /// 레이어 비교 결과를 반환합니다.
        /// </summary>
        /// <param name="layerIndex">레이어 인덱스 번호</param>
        /// <param name="rhs">레이어 마스크</param>
        /// <returns>레이어 비교 결과</returns>
        public static bool CompareMask(int layerIndex, UnityEngine.LayerMask rhs)
        {
            int a = 1 << layerIndex;
            int b = rhs.value;

            return (a & b) != 0;
        }

        /// <summary>
        /// 레이어 비교 결과를 반환합니다.
        /// </summary>
        /// <param name="layerIndex">레이어 마스크</param>
        /// <param name="rhs">레이어 마스크</param>
        /// <returns>레이어 비교 결과</returns>
        public static bool CompareMask(UnityEngine.LayerMask lhs, UnityEngine.LayerMask rhs)
        {
            return (lhs & rhs) != 0;
        }

        public static bool CompareLayerIsPlayer(int index)
        {
            return CompareMask(index, LayerMask.Player);
        }
        public static bool CompareLayerIsPlayer(UnityEngine.LayerMask comp)
        {
            return CompareMask(comp, LayerMask.Player);
        }
    }

    public class TagLiteral
    {
        public static readonly string Player = "Player";
        public static readonly string OnewayPlatform = "OnewayPlatform";
    }
}
