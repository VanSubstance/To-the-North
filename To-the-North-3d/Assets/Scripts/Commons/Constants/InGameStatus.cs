using System.Collections.Generic;
using Assets.Scripts.Components.Windows.Inventory;
using Assets.Scripts.Components.Progress;
using Assets.Scripts.Items;
using Assets.Scripts.Users;
using Assets.Scripts.Users.Objects;

public static class InGameStatus
{
    public const string TAG = "인게임 정보";
    public static class User
    {
        public static bool isPause = false;
        public static bool isInAction = false;

        public static class status
        {
            public static BarBaseController hpBar;
            public static BarBaseController staminaBar;
            public static BarBaseController hungerBar;
            public static BarBaseController thirstBar;
            public static BarBaseController temperatureBar;

            /// <summary>
            /// 체력 조절 함수:
            /// 기본적으로 해당 수치만큼 "깎인다"
            /// </summary>
            /// <param name="amount">"깎을" 수치</param>
            public static void ApplyDamage(float amount)
            {
                hpBar.LiveInfo = -amount;
                if (hpBar.LiveInfo <= 0)
                {
                    //InGameStatus.User.isPause = true;
                }
            }


            /// <summary>
            /// 체력 조절 함수:
            /// 기본적으로 해당 수치만큼 "오른다"
            /// </summary>
            /// <param name="amount">"오를" 수치</param>
            public static void ApplyHunger(float amount)
            {
                hungerBar.LiveInfo = amount;
                if (hungerBar.LiveInfo <= 0)
                {
                    //InGameStatus.User.isPause = true;
                }
            }


            /// <summary>
            /// 체력 조절 함수:
            /// 기본적으로 해당 수치만큼 "오른다"
            /// </summary>
            /// <param name="amount">"오를" 수치</param>
            public static void ApplyThirst(float amount)
            {
                thirstBar.LiveInfo = amount;
                if (thirstBar.LiveInfo <= 0)
                {
                    //InGameStatus.User.isPause = true;
                }
            }


            /// <summary>
            /// 체력 조절 함수:
            /// 기본적으로 해당 수치만큼 "오른다"
            /// </summary>
            /// <param name="amount">"오를" 수치</param>
            public static void ApplyTemperature(float amount)
            {
                temperatureBar.LiveInfo = amount;
                if (temperatureBar.LiveInfo <= 0)
                {
                    //InGameStatus.User.isPause = true;
                }
            }
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
                public static float range = 3.0f;
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
                case EquipBodyType.BackPack:
                    break;
                case EquipBodyType.Hand:
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
                case EquipBodyType.Hand:
                    return (ItemWeaponInfo)curEquipments[targetPartType];
                case EquipBodyType.BackPack:
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
                    if  (
                            ((ItemBulletInfo)inven.itemInfo).bulletType.Equals(type) &&
                            ((ItemBulletInfo)inven.itemInfo).AmountCount > 0
                        )
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
                if (inven.itemInfo is ItemMagazineInfo magInfo &&
                    magInfo.bulletType.Equals(type)
                    )
                {
                    return (ItemMagazineInfo)PullItemFromInventory(inven);
                }
            }
            return null;
        }

        /// <summary>
        /// 인벤토리에서 아이템 꺼내기:
        /// 아이템 인벤토리 오브젝트 제거 후 아이템 정보만 반환
        /// </summary>
        /// <param name="itemFromInventory"></param>
        /// <returns></returns>
        public static ItemBaseInfo PullItemFromInventory(ItemInventoryInfo itemFromInventory)
        {
            itemFromInventory.itemInfo.Ctrl.ItemDetach();
            itemFromInventory.itemInfo.Ctrl.ItemTruncate();
            inventory.Remove(itemFromInventory);
            return itemFromInventory.itemInfo;
        }

        /// <summary>
        /// 인벤토리에 아이템 넣기:
        /// 아이템 오브젝트 생성 후 자동정렬로 들어감
        /// </summary>
        /// <param name="baseInfo"></param>
        public static void PushItemToInventory(ItemBaseInfo baseInfo)
        {
            WindowInventoryController.Instance.GenerateItemObjectWithAuto(ContentType.Inventory, baseInfo);
        }
    }
}
