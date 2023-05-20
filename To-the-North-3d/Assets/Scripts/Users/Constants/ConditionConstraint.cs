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
            /// 스테미나 데미지
            /// </summary>
            public static readonly ConditionType[] Stamina = { ConditionType.Exhaust, ConditionType.Hot };
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
                {ConditionType.Exhaust, 2},
                {ConditionType.Hot, 2},
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
            public static readonly ConditionType[] Run = { ConditionType.Fracture, ConditionType.Overweight_Heavy };
            /// <summary>
            /// 닷지(구르기) 불가
            /// </summary>
            public static readonly ConditionType[] Dodge = { ConditionType.Fracture, ConditionType.Overweight_Heavy };
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
            public static readonly ConditionType[] RecoveryStamina = { ConditionType.Fracture, ConditionType.Bleeding_Heavy, ConditionType.Hunger, ConditionType.Thirst, ConditionType.Overweight_Heavy };
            /// <summary>
            /// 무기 정확도
            /// </summary>
            public static readonly ConditionType[] Accuracy = { ConditionType.Fracture };
            /// <summary>
            /// 스테미나 소모 속도
            /// </summary>
            public static readonly ConditionType[] SpeedUseStamina = { ConditionType.Hunger, ConditionType.Thirst, ConditionType.Overweight_Heavy };
            /// <summary>
            /// 갈증 소모속도
            /// </summary>
            public static readonly ConditionType[] SpeedThirst = { ConditionType.Hot, ConditionType.Overweight_Heavy, ConditionType.Overweight_Light };
            /// <summary>
            /// 허기 소모속도
            /// </summary>
            public static readonly ConditionType[] SpeedHunger = { ConditionType.Cold, ConditionType.Overweight_Heavy };
            /// <summary>
            /// 카메라 이동 속도
            /// </summary>
            public static readonly ConditionType[] SpeedCameraTracking = { ConditionType.Dizziness };
            /// <summary>
            /// 이동 속도
            /// </summary>
            public static readonly ConditionType[] SpeedMove = { ConditionType.Pain, ConditionType.Infection, ConditionType.Cold, ConditionType.Overweight_Heavy, ConditionType.Overweight_Light };
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
            /// <summary>
            /// 화면 울렁거림
            /// </summary>
            public static readonly ConditionType[] Dizziness = { ConditionType.Thirst, ConditionType.Dizziness };
        }

        /// <summary>
        /// 확률 관련
        /// </summary>
        public static class Possibility
        {
            /// <summary>
            /// 어지러움 발생 확률 상승
            /// </summary>
            public static readonly ConditionType[] Dizziness = { ConditionType.Hot };
            /// <summary>
            /// 얕은 출혈 발생 가능성 상승
            /// </summary>
            public static readonly ConditionType[] Bleeding_Light = { ConditionType.Cold };
        }
    }
}
