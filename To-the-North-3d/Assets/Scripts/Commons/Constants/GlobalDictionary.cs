using System.Collections.Generic;
using Assets.Scripts.Components.Hovers;
using Assets.Scripts.Creatures;
using Assets.Scripts.Users;
using UnityEngine;

public static class GlobalDictionary
{
    public static class Sound
    {
        public static string PathRoot = $"Sounds";
        public static string PathMove = $"{PathRoot}/Move";
        public static Dictionary<CreatureType, SoundMove> Move = new()
        {
            {
                CreatureType.Human,
                new SoundMove()
            },
            {
                CreatureType.FourLeg,
                new SoundMove()
            },
            {
                CreatureType.Slime,
                new SoundMove()
            },
        };
        public class SoundMove
        {
            public AudioClip Walk, Run;
        }
        public static class Interaction
        {
            public static string PathRoot = $"{Sound.PathRoot}/Interaction";
            public static class Consumable
            {
                public static string PathRoot = $"{Interaction.PathRoot}/Consumable";
                public static class Food
                {
                    public static string PathRoot = $"{Consumable.PathRoot}/Food";
                    public static AudioClip Drink, Chip, Burgur;
                }
                public static class Medicine
                {
                    public static string PathRoot = $"{Consumable.PathRoot}/Medicine";
                    public static AudioClip Bandage, Injection, Swallow;
                }
            }
            public static class Equipment
            {
                public static string PathRoot = $"{Interaction.PathRoot}/Equipment";
                public static AudioClip Equip, Unequip, Reload;
            }
            public static class Door
            {
                public static string PathRoot = $"{Interaction.PathRoot}/Door";
                public static AudioClip Open, Close, Lock;
            }
        }

        public static class Battle
        {
            public static string PathRoot = $"{Sound.PathRoot}/Battle";
            public static class Weapon
            {
                public static string PathRoot = $"{Battle.PathRoot}/Weapon";
                public static class Melee
                {
                    public static string PathRoot = $"{Weapon.PathRoot}/Melee";
                    public static AudioClip Swing, Step;
                }
                public static class Range
                {
                    public static string PathRoot = $"{Weapon.PathRoot}/Range";
                    public static AudioClip Arrow;
                    public static class Gun
                    {
                        public static string PathRoot = $"{Range.PathRoot}/Gun";
                        public static AudioClip Small, Big;
                    }
                }
            }
        }
    }
    public static class Text
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

        public static class System
        {
            public static string
                ItemGet,
                ItemPay,
                QuestGet,
                QuestClear,
                CurrencyGet,
                CurrencyPay;
        }

        public static class Inventory
        {
            public static string
                Inven,
                Looting,
                Commerce,
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