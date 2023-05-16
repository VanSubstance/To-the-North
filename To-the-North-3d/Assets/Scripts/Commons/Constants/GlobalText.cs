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
    }
}
