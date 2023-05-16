using System.Collections.Generic;
using Assets.Scripts.Components.Hovers;
using Assets.Scripts.Users;

namespace Assets.Scripts.Commons
{
    public static class GlobalText
    {
        public static class Common
        {
            public static string
                ReturnToGame,
                GoToOption,
                SaveGame,
                LoadGame,
                StartGame,
                GoToDesktop,
                Loading,
                Back,
                Language;
        }

        public static class Inventory
        {
            public static string
                Inven,
                Looting,
                Equipment,
                Helmet,
                Mask,
                Body,
                Backpack,
                WeaponPri,
                WeaponSec;
        }

        public static Dictionary<ConditionType, ConditionInfo> Conditions = new();

        public static class Item
        {
            public static class Armor
            {
                public static string
                    tPenatration, tImpact, tHeat;
            }
            public static class Bullet
            {
                public static string
                    tBulletType, tAccelSpd;
            }
            public static class Damage
            {
                public static string
                    tPwPene, tPwImp, tPwKnock, tDmgPene, tDmgImp;
            }
            public static class Weapon
            {
                public static string
                    tAtkSpd, tHandType;
            }
            public static class WeaponRange
            {
                public static string 
                    tReload, tRange, tProjSpd, tBulletType;
            }
        }
    }
}
