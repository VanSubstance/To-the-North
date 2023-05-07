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
            /// <summary>
            /// 틱 데미지
            /// </summary>
            public static readonly ConditionType[] Damage = { ConditionType.Bleeding_Light, ConditionType.Bleeding_Heavy, ConditionType.Infection };
            /// <summary>
            /// 화면 진동
            /// </summary>
            public static readonly ConditionType[] Vibrating = { ConditionType.Bleeding_Light, ConditionType.Fracture, ConditionType.Hunger };
            /// <summary>
            /// 화면 붉어짐
            /// </summary>
            public static readonly ConditionType[] Blurred = { ConditionType.Fracture, ConditionType.Bleeding_Heavy, ConditionType.Infection };
            /// <summary>
            /// 상태이상 별 틱 데미지
            /// </summary>
            public static readonly Dictionary<ConditionType, int> TickAmountForCondition = new Dictionary<ConditionType, int>()
            {
                {ConditionType.Bleeding_Light, 2},
                {ConditionType.Infection, 2},
                {ConditionType.Bleeding_Heavy, 6},
            };
        }

        /// <summary>
        /// 컨트롤(행동) 관련
        /// </summary>
        public static class UtilBlock
        {
            /// <summary>
            /// 달리기 불가
            /// </summary>
            public static readonly ConditionType[] Run = { ConditionType.Fracture };
            /// <summary>
            /// 닷지(구르기) 불가
            /// </summary>
            public static readonly ConditionType[] Dodge = { ConditionType.Fracture };
            /// <summary>
            /// 조준 불가
            /// </summary>
            public static readonly ConditionType[] Aim = { };
        }

        /// <summary>
        /// 능력치 관련
        /// </summary>
        public static class PerformanceLack
        {
            /// <summary>
            /// 스테미나 회복
            /// </summary>
            public static readonly ConditionType[] RecoveryStamina = { ConditionType.Fracture, ConditionType.Bleeding_Heavy, ConditionType.Hunger, ConditionType.Thirst };
            /// <summary>
            /// 무기 정확도
            /// </summary>
            public static readonly ConditionType[] Accuracy = { ConditionType.Fracture };
            /// <summary>
            /// 스테미나 소모 속도
            /// </summary>
            public static readonly ConditionType[] SpeedUseStamina = { ConditionType.Hunger, ConditionType.Thirst };
            /// <summary>
            /// 카메라 이동 속도
            /// </summary>
            public static readonly ConditionType[] SpeedCameraTracking = { ConditionType.Dizziness };
            /// <summary>
            /// 이동 속도
            /// </summary>
            public static readonly ConditionType[] SpeedMove = { ConditionType.Pain, ConditionType.Infection };
            /// <summary>
            /// 재장전 속도
            /// </summary>
            public static readonly ConditionType[] SpeedReload = { };
            /// <summary>
            ///  시야 각도
            /// </summary>
            public static readonly ConditionType[] DegreeSight = { };
            /// <summary>
            /// 시야 거리
            /// </summary>
            public static readonly ConditionType[] RangeSight = { };
        }
    }
}
