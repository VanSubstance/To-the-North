using System.Collections.Generic;

namespace Assets.Scripts.Users
{
    /// <summary>
    /// 각 상태이상 별 효과 적용을 위한 델리게이트
    /// </summary>
    /// <param name="amount">값</param>
    public delegate void ConditionControl(float amount);

    public static class ConditionConstraint
    {
        /// <summary>
        /// 틱 관련
        /// </summary>
        public static class Tick
        {
            public static readonly ConditionType[] Damage = { ConditionType.Bleeding_Light, ConditionType.Bleeding_Heavy };
            public static readonly ConditionType[] Vibrating = { ConditionType.Bleeding_Light, ConditionType.Fracture };
            public static readonly ConditionType[] Blurred = { ConditionType.Fracture, ConditionType.Bleeding_Heavy };
            public static readonly Dictionary<ConditionType, int> TickAmountForCondition = new Dictionary<ConditionType, int>()
            {
                {ConditionType.Bleeding_Light, 2},
                {ConditionType.Bleeding_Heavy, 6},
                {ConditionType.Fracture, 11},
            };
        }

        /// <summary>
        /// 컨트롤(행동) 관련
        /// </summary>
        public static class UtilBlock
        {
            public static readonly ConditionType[] Run = { ConditionType.Fracture };
            public static readonly ConditionType[] Dodge = { ConditionType.Fracture };
            public static readonly ConditionType[] Aim = { };
        }

        /// <summary>
        /// 능력치 관련
        /// </summary>
        public static class PerformanceLack
        {
            public static readonly ConditionType[] RecoveryStamina = { ConditionType.Fracture, ConditionType.Bleeding_Heavy };
            public static readonly ConditionType[] Accuracy = { ConditionType.Fracture };
            public static readonly ConditionType[] SpeedUseStamina = { };
            public static readonly ConditionType[] SpeedCameraTracking = { };
            public static readonly ConditionType[] SpeedMove = { };
            public static readonly ConditionType[] SpeedReload = { };
            public static readonly ConditionType[] DegreeSight = { };
            public static readonly ConditionType[] RangeSight = { };
        }
    }
}
