using System.Collections.Generic;
using Assets.Scripts.Components.Progress;
using Assets.Scripts.Items;
using Assets.Scripts.Users;
using Assets.Scripts.Users.Objects;

namespace Assets.Scripts.Commons.Constants
{
    public static class InGameStatus
    {
        public const string TAG = "인게임 정보";
        public static class User
        {
            public static bool isPause = false;

            public static class status
            {
                public static BarBaseController hpBar;
                public static BarBaseController staminaBar;
                public static BarBaseController hungerBar;
                public static BarBaseController thirstBar;
                public static BarBaseController temperatureBar;
            }

            public static Dictionary<ConditionType, int> conditions = new Dictionary<ConditionType, int>() {
                { ConditionType.Bleeding_Light, 0},
                { ConditionType.Bleeding_Heavy, 0},
                { ConditionType.Fracture, 0},
                { ConditionType.Test, 0},
            };

            public static bool IsConditionExist(ConditionType[] targetTypes)
            {
                foreach (ConditionType type in targetTypes)
                {
                    if (conditions[type] > 0) return true;
                }
                return false;
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
                    private static float degreeError = 10;
                    public static float DegreeError
                    {
                        get
                        {
                            return degreeError;
                        }
                        set
                        {
                            degreeError = value > .5f ? value : .5f;
                        }
                    }
                    public static float Range
                    {
                        get
                        {
                            float w = 1;
                            if (IsConditionExist(ConditionConstraint.PerformanceLack.RangeSight))
                            {
                                w /= 2;
                            }
                            return 45 / w;
                        }
                    }
                    public static int Degree
                    {
                        get
                        {
                            int w = 1;
                            if (IsConditionExist(ConditionConstraint.PerformanceLack.DegreeSight))
                            {
                                w /= 2;
                            }
                            return 90 / w;
                        }
                    }
                    public static bool isControllInRealTime = false;
                }
            }
        }

        public static class Item
        {
            /// <summary>
            /// 현재 착용하고 있는 장비 정보
            /// </summary>
            private static Dictionary<EquipBodyType, ItemEquipmentInfo> curEquipments = new Dictionary<EquipBodyType, ItemEquipmentInfo>();
            public static List<ItemInventoryInfo> inventory = new List<ItemInventoryInfo>();

            /// <summary>
            /// 착용 장비 정보 할당 함수
            /// </summary>
            /// <param name="targetPartType">착용할 신체 부위</param>
            /// <param name="itemInfo">착용할 아이템 정보</param>
            public static void SetEquipmentInfo(EquipBodyType targetPartType, ItemEquipmentInfo itemInfo)
            {
                if (itemInfo == null || UserBaseController.Instance == null)
                {
                    // 아이템 정보가 없거나 유저 오브젝트가 준비되지 않음
                    return;
                }
                switch (targetPartType)
                {
                    case EquipBodyType.Helmat:
                    case EquipBodyType.Head:
                    case EquipBodyType.Mask:
                    case EquipBodyType.Body:
                    case EquipBodyType.Leg:
                        try
                        {
                            ItemArmorInfo armorInfo = (ItemArmorInfo)itemInfo;
                            if (targetPartType != armorInfo.equipPartType)
                            {
                                // 목표 착용 부위와 장비가 안맞음
                                throw new System.InvalidCastException();
                            }
                        }
                        catch (System.InvalidCastException)
                        {
                            // 목표 착용 부위와 장비가 안맞음
                        }
                        break;
                    case EquipBodyType.Back:
                        break;
                    case EquipBodyType.Right:
                    case EquipBodyType.Left:
                        try
                        {
                            ItemWeaponInfo weaponInfo = (ItemWeaponInfo)itemInfo;
                            switch (weaponInfo.handType)
                            {
                                case EquipHandType.Single:
                                    // 한손 무기일 경우
                                    break;
                                case EquipHandType.Multiple:
                                    // 양손 무기일 경우
                                    break;
                            }
                        }
                        catch (System.InvalidCastException)
                        {
                            // 목표 착용 부위와 장비가 안맞음
                        }
                        break;
                    default:
                        break;
                }
                curEquipments[targetPartType] = itemInfo;
                UserBaseController.Instance.ChangeEquipment(targetPartType, itemInfo);
            }

            /// <summary>
            /// 착용 정보 함수 반환 함수
            /// </summary>
            /// <param name="targetPartType">목표 부위</param>
            /// <returns></returns>
            public static ItemEquipmentInfo GetEquipmentInfo(EquipBodyType targetPartType)
            {
                switch (targetPartType)
                {
                    case EquipBodyType.Helmat:
                    case EquipBodyType.Head:
                    case EquipBodyType.Mask:
                    case EquipBodyType.Body:
                    case EquipBodyType.Leg:
                        return (ItemArmorInfo)curEquipments[targetPartType];
                    case EquipBodyType.Right:
                    case EquipBodyType.Left:
                        return (ItemWeaponInfo)curEquipments[targetPartType];
                    case EquipBodyType.Back:
                        return (ItemEquipmentInfo)curEquipments[targetPartType];
                }
                return null;
            }

            public static ItemBulletInfo LookforBullet(ItemBulletType type)
            {
                foreach (ItemInventoryInfo inven in inventory)
                {
                    if (inven.itemInfo is ItemBulletInfo)
                    {
                        if (((ItemBulletInfo)inven.itemInfo).AmountCount > 0)
                        {
                            return (ItemBulletInfo)inven.itemInfo;
                        }
                    }
                }
                return null;
            }

            public static ItemMagazineInfo LookForMagazine(ItemBulletType type)
            {
                foreach (ItemInventoryInfo inven in inventory)
                {
                    if (inven.itemInfo is ItemMagazineInfo)
                    {
                        return (ItemMagazineInfo)PullItemFromInventory(inven);
                    }
                }
                return null;
            }

            public static ItemBaseInfo PullItemFromInventory(ItemInventoryInfo itemFromInventory)
            {
                UnityEngine.Debug.Log("강민준:: Inventory에 해당 아이템 찾아서 삭제하기");
                inventory.Remove(itemFromInventory);
                return itemFromInventory.itemInfo;
            }

            public static void PutItemToInventory(ItemBaseInfo _info)
            {
                ItemInventoryInfo newItem = new ItemInventoryInfo(_info);
                UnityEngine.Debug.Log("강민준:: Inventory에 알아서 빈 자리 찾아서 들어가기");
            }
        }
    }
}
