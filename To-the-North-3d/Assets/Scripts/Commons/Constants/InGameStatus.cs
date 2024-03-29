using System.Collections.Generic;
using Assets.Scripts.Components.Progress;
using Assets.Scripts.Components.Windows;
using Assets.Scripts.Components.Windows.Inventory;
using Assets.Scripts.Items;
using Assets.Scripts.Users;
using Assets.Scripts.Users.Objects;
using Assets.Scripts.Components.Infos;

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
                if (hungerBar.LivePercent < .25f)
                {
                    // 허기 25% 미만
                    UserBaseController.Instance.OccurCondition(ConditionType.Hunger, true);
                }
                else if (hungerBar.LivePercent < .50f)
                {
                    // 허기 25% 미만
                    UserBaseController.Instance.CureCondition(ConditionType.Hunger, 1);
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
                if (thirstBar.LivePercent < .25f)
                {
                    // 허기 25% 미만
                    UserBaseController.Instance.OccurCondition(ConditionType.Thirst, true);
                }
                else if (thirstBar.LivePercent < .50f)
                {
                    // 허기 25% 미만
                    UserBaseController.Instance.CureCondition(ConditionType.Thirst, 1);
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

            /// <summary>
            /// 스테미나 조절 함수:
            /// 기본적으로 해당 수치만큼 "오른다"
            /// </summary>
            /// <param name="amount">"오를" 수치</param>
            public static void ApplyStamina(float amount)
            {
                staminaBar.LiveInfo = amount;
                if (staminaBar.LiveInfo <= 0)
                {
                    //InGameStatus.User.isPause = true;
                }
            }
        }

        public static Dictionary<ConditionType, int> conditions;

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

        public static ItemBulletInfo LookforBullet(ItemBulletType type)
        {
            foreach (ItemInventoryInfo inven in WindowInventoryController.Instance.ContentInventory.itemsAttached)
            {
                if (inven.itemInfo is ItemBulletInfo info)
                {
                    if (
                            info.bulletType.Equals(type) &&
                            info.AmountCount > 0
                        )
                    {
                        return info;
                    }
                }
            }
            return null;
        }

        public static ItemMagazineInfo LookForMagazine(ItemBulletType type)
        {
            foreach (ItemInventoryInfo inven in WindowInventoryController.Instance.ContentInventory.itemsAttached)
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

        public static bool LookForItemByCode(string _code)
        {
            foreach (ItemInventoryInfo inven in WindowInventoryController.Instance.ContentInventory.itemsAttached)
            {
                if (inven.itemInfo.imagePath.Equals(_code))
                {
                    return true;
                }
            }
            return false;
        }

        public static int CountItemByCode(string _code)
        {
            int res = 0;
            foreach (ItemInventoryInfo inven in WindowInventoryController.Instance.ContentInventory.itemsAttached)
            {
                if (inven.itemInfo.imagePath.Equals(_code))
                {
                    res++;
                }
            }
            return res;
        }

        public static ItemBaseInfo PullItemFromInventoryByCode(string _code)
        {
            foreach (ItemInventoryInfo inven in WindowInventoryController.Instance.ContentInventory.itemsAttached)
            {
                if (inven.itemInfo.imagePath.Equals(_code))
                {
                    return PullItemFromInventory(inven);
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
            int idx = WindowInventoryController.Instance.ContentInventory.itemsAttached.IndexOf(itemFromInventory);
            ItemBaseInfo res = itemFromInventory.itemInfo;
            res.Ctrl.ItemTruncate();
            WindowInventoryController.Instance.ContentInventory.itemsAttached.RemoveAt(idx);
            return res;
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

    public static class Quest
    {
        public static List<string> Done = new List<string>();
        public static List<string> Progress = new List<string>();
    }

    private static int currency = 0;
    public static int Currency
    {
        get
        {
            return currency;
        }
        set
        {
            if (value == 0) return;
            currency += value;
            UIInfoTextContainerController.Instance.PrintText($"{(value > 0 ? GlobalText.System.CurrencyGet : GlobalText.System.CurrencyPay)}: {(value > 0 ? value : -value)} G");
            GlobalComponent.Common.Text.Inventory.currency.text = $"{currency} G";
        }
    }

    public static class Weight
    {
        private static int weight = 0, maxWeight = 100;
        public static int WeightC
        {
            get
            {
                return weight;
            }
            set
            {
                if (value == 0) return;
                weight += value;
                GlobalComponent.Common.Text.Inventory.weight.text = $"{weight} / {maxWeight}";
                CheckOverWeight();
            }
        }
        public static int MaxWeightC
        {
            get
            {
                return maxWeight;
            }
            set
            {
                if (value == 0) return;
                maxWeight += value;
                GlobalComponent.Common.Text.Inventory.weight.text = $"{weight} / {maxWeight}";
                CheckOverWeight();
            }
        }
        public static int OverweightAmount
        {
            get
            {
                return weight - maxWeight;
            }
        }

        /// <summary>
        /// 과정 평가 함수
        /// </summary>
        public static void CheckOverWeight()
        {
            if (OverweightAmount > 20)
            {
                // 초 과적 상태
                UserBaseController.Instance.OccurCondition(ConditionType.Overweight_Heavy, true);
                UserBaseController.Instance.CureCondition(ConditionType.Overweight_Light, 1);
                return;
            }
            if (OverweightAmount > 3)
            {
                // 과적 상태
                UserBaseController.Instance.CureCondition(ConditionType.Overweight_Heavy, 1);
                UserBaseController.Instance.OccurCondition(ConditionType.Overweight_Light, true);
                return;
            }
            // 정상
            UserBaseController.Instance.CureCondition(ConditionType.Overweight_Light, 1);
            UserBaseController.Instance.CureCondition(ConditionType.Overweight_Heavy, 1);
            return;
        }
    }
}
