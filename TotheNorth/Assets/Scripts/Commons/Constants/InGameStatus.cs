using System.Collections.Generic;
using Assets.Scripts.Components.Progress;
using Assets.Scripts.Items;
using Assets.Scripts.Users.Objects;

namespace Assets.Scripts.Commons.Constants
{
    public static class InGameStatus
    {
        public static class User
        {
            public static bool isPause = false;

            public static class status
            {
                public static BarBaseController hpBar;
                public static BarBaseController staminaBar;
            }
            public static class Movement
            {
                /** 현재 바라보고 있는 시야 방향 각도 */
                public static int curdegree = 0;
                public static float spdWalk = 4f;
                public static float weightRun = 7f;
                public static float weightCrouch = 2.5f;
                public static MovementType curMovement = MovementType.WALK;
            }
            public static class Detection
            {
                public static float distanceInteraction = 2f;
                public static class Instinct
                {
                    public static float range = 2.0f;
                }
                public static class Sight
                {
                    public static float rangeMin = 45.0f;
                    public static float rangeMax = 60.0f;
                    public static float range = 45.0f;
                    public static int degree = 90;
                    public static bool isControllInRealTime = false;
                }
            }
        }

        public static class Item
        {
            /// <summary>
            /// 현재 착용하고 있는 장비 정보
            /// </summary>
            private static Dictionary<EquipPartType, ItemEquipmentInfo> curEquipments = new Dictionary<EquipPartType, ItemEquipmentInfo>();
            public static List<ItemInventoryInfo> inventory = new List<ItemInventoryInfo>();

            /// <summary>
            /// 착용 장비 정보 할당 함수
            /// </summary>
            /// <param name="targetPartType">착용할 신체 부위</param>
            /// <param name="itemInfo">착용할 아이템 정보</param>
            public static void SetEquipmentInfo(EquipPartType targetPartType, ItemEquipmentInfo itemInfo)
            {
                switch (targetPartType)
                {
                    case EquipPartType.Helmat:
                    case EquipPartType.Head:
                    case EquipPartType.Mask:
                    case EquipPartType.Body:
                    case EquipPartType.Leg:
                        try
                        {
                            itemInfo = (ItemArmorInfo)itemInfo;
                        }
                        catch (System.InvalidCastException)
                        {
                            // 목표 착용 부위와 장비가 안맞음
                        }
                        break;
                    case EquipPartType.Back:
                        break;
                    case EquipPartType.Right:
                    case EquipPartType.Left:
                        try
                        {
                            itemInfo = (ItemWeaponInfo)itemInfo;
                        }
                        catch (System.InvalidCastException)
                        {
                            // 목표 착용 부위와 장비가 안맞음
                        }
                        break;
                }
                if (targetPartType != itemInfo.equipPartType)
                {
                    // 목표 착용 부위와 장비가 안맞음
                }
                curEquipments[targetPartType] = itemInfo;
            }

            /// <summary>
            /// 착용 정보 함수 반환 함수
            /// </summary>
            /// <param name="targetPartType">목표 부위</param>
            /// <returns></returns>
            public static ItemEquipmentInfo GetEquipmentInfo(EquipPartType targetPartType)
            {
                switch (targetPartType)
                {
                    case EquipPartType.Helmat:
                    case EquipPartType.Head:
                    case EquipPartType.Mask:
                    case EquipPartType.Body:
                    case EquipPartType.Leg:
                        return (ItemArmorInfo)curEquipments[targetPartType];
                    case EquipPartType.Right:
                    case EquipPartType.Left:
                        return (ItemWeaponInfo)curEquipments[targetPartType];
                    case EquipPartType.Back:
                        return (ItemEquipmentInfo)curEquipments[targetPartType];
                }
                return null;
            }
        }
    }
}
