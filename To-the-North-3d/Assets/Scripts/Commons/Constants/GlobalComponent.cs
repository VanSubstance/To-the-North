using System.Collections.Generic;
using Assets.Scripts.Items;
using Assets.Scripts.Users;
using UnityEngine;

public static class GlobalComponent
{
    public static class Common
    {
        public static class Info
        {
            public static InfoMessageManager controller;
        }
        public static class Event
        {
            public static MouseCursorManager mouseCursorManager;
        }
    }

    public static class Path
    {
        public static class Image
        {
            public static readonly string Root = $"Images/";
            public static string Condition(ConditionType type) => $"{Root}Conditions/{type}";
        }
        public static class DataObject
        {
            public static readonly string Root = $"DataObjects/";
        }
        public static class Item
        {
            //public static string[] Categories = new string[] {
            //    typeof(ItemMaterialInfo).Name,

            //    typeof(ItemWeaponInfo).Name,
            //    typeof(ItemArmorInfo).Name,
            //    typeof(ItemMagazineInfo).Name,

            //    typeof(ItemFoodInfo).Name,
            //    typeof(ItemBulletInfo).Name,
            //};
            private static readonly string Root = $"Items/";
            public static class Consumbable
            {
                private static readonly string Root = $"{Item.Root}Consumables/";
                /// <summary>
                /// 탄환 이미지 경로
                /// </summary>
                /// <param name="name">탄환 이름</param>
                /// <returns></returns>
                public static string Bullet(string name) => $"{Root}Bullets/{name}";
                /// <summary>
                /// 음식 이미지 경로
                /// </summary>
                /// <param name="name">음식 이름</param>
                /// <returns></returns>
                public static string Food(string name) => $"{Root}Foods/{name}";
            }
            public static class Equipment
            {
                private static readonly string Root = $"{Item.Root}Equipments/";
                /// <summary>
                /// 방어구 이미지 경로
                /// </summary>
                /// <param name="name">방어구 이름</param>
                /// <returns></returns>
                public static string Armor(string name) => $"{Root}Armors/{name}";
                /// <summary>
                /// 무기 이미지 경로
                /// </summary>
                /// <param name="name">무기 이름</param>
                /// <returns></returns>
                public static string Weapon(string name) => $"{Root}Weapons/{name}";
                /// <summary>
                /// 탄창 경로
                /// </summary>
                /// <param name="name">탄창 이름</param>
                /// <returns></returns>
                public static string Magazine(string name) => $"{Root}Magazines/{name}";
            }
            /// <summary>
            /// 재료 이미지 경로
            /// </summary>
            /// <param name="name">무기 이름</param>
            /// <returns></returns>
            public static string Material(string name) => $"{Root}Materials/{name}";
        }

        public static ItemBulletInfo GetMonsterBulletInfo(ItemBulletType bulletType, int lv)
        {
            return Resources.Load<ItemBulletInfo>($"{DataObject.Root}Monsters/{Item.Consumbable.Bullet($"Monster{bulletType}Lv{lv}")}");
        }

        public static ItemMagazineInfo GetMonsterMagazineInfo(ItemBulletType bulletType, int lv)
        {
            return Resources.Load<ItemMagazineInfo>($"{DataObject.Root}Monsters/{Item.Equipment.Magazine($"Monster{bulletType}Lv{lv}")}");
        }

        /// <summary>
        /// 해당 아이템에 맞는 이미지 경로 반환 함수
        /// </summary>
        /// <param name="info">아이템 정보</param>
        /// <returns></returns>
        public static string GetImagePath(ItemBaseInfo info)
        {
            string comp = info.GetType().Name;
            switch (comp)
            {
                case "ItemWeaponInfo":
                    return $"{Image.Root}{Item.Equipment.Weapon(info.imagePath)}";
                case "ItemArmorInfo":
                    return $"{Image.Root}{Item.Equipment.Armor(info.imagePath)}";
                case "ItemMagazineInfo":
                    return $"{Image.Root}{Item.Equipment.Magazine(info.imagePath)}";
                case "ItemFoodInfo":
                    return $"{Image.Root}{Item.Consumbable.Food(info.imagePath)}";
                case "ItemBulletInfo":
                    return $"{Image.Root}{Item.Consumbable.Bullet(info.imagePath)}";
                case "ItemMaterialInfo":
                    return $"{Image.Root}{Item.Material(info.imagePath)}";
                default:
                    break;
            }
            return null;
        }
    }

    public static class Asset
    {

        public static Sprite GetImage(ItemBaseInfo info)
        {
            return Resources.Load<Sprite>(Path.GetImagePath(info));
        }
    }
}