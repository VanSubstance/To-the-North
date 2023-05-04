using System.Collections.Generic;
using Assets.Scripts.Battles;
using Assets.Scripts.Commons;
using Assets.Scripts.SoundEffects;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    public abstract class AbsCreatureBaseController : MonoBehaviour, ICreatureBattle, ISoundable
    {

        [SerializeField]
        protected ItemArmorController helmet, mask, body, backpack;
        [SerializeField]
        protected ItemWeaponController weapon;

        protected Dictionary<EquipBodyType, IItemEquipable> equipableBodies = new Dictionary<EquipBodyType, IItemEquipable>();
        protected ItemEquipmentInfo[] weapons = new ItemEquipmentInfo[2];
        /// <summary>
        /// 현재 장착중인 무기 인덱스; 0 <- 없음; 1 <- 주무기; 2 <- 부무기
        /// </summary>
        protected int curWeapon = 0;

        protected void Awake()
        {
            SoundEffectManager.AddAudioSource(transform, true, out Speaker);
            Speaker.maxDistance = 15;

            equipableBodies[EquipBodyType.Helmat] = helmet;
            equipableBodies[EquipBodyType.Mask] = mask;
            equipableBodies[EquipBodyType.Body] = body;
            equipableBodies[EquipBodyType.BackPack] = backpack;
            equipableBodies[EquipBodyType.Hand] = weapon;
            weapon.Owner = transform;
        }

        private Collider bushHidden = null;
        public Collider BushHidden
        {
            get
            {
                return bushHidden;
            }
        }

        [HideInInspector]
        private AudioSource Speaker;
        [SerializeField]
        protected AudioClip
            audEat, audDrink,
            audBandage,
            audReload
            ;

        /// <summary>
        /// 부쉬 상태 체크
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Hide"))
            {
                Bounds bounds = other.bounds;
                if (bounds.Contains(transform.position))
                {
                    bushHidden = other;
                    return;
                }
                if (bushHidden != null && bushHidden.Equals(other))
                {
                    bushHidden = null;
                }
                return;
            }
        }

        public void PlaySound(AudioClip _clip = null)
        {
            if (_clip != null)
            {
                Speaker.clip = _clip;
            }
            Speaker.loop = true;
            Speaker.Play();
        }

        public void PlaySoundByType(SoundType _type)
        {
            if (_type.Equals(IsSoundInPlaying())) return;
            switch (_type)
            {
                case SoundType.Eat:
                    PlaySound(audEat);
                    break;
                case SoundType.Drink:
                    PlaySound(audDrink);
                    break;
                case SoundType.Bandage:
                    PlaySound(audBandage);
                    break;
                case SoundType.Reload:
                    PlaySound(audReload);
                    break;
                case SoundType.None:
                    StopSound();
                    break;
            }
        }

        public SoundType IsSoundInPlaying()
        {
            if (!Speaker.isPlaying) return SoundType.None;
            AudioClip c = Speaker.clip;
            if (c.Equals(audDrink)) return SoundType.Drink;
            if (c.Equals(audEat)) return SoundType.Eat;
            if (c.Equals(audBandage)) return SoundType.Bandage;
            if (c.Equals(audReload)) return SoundType.Reload;
            return SoundType.None;
        }

        public void StopSound()
        {
            Speaker.Stop();
        }

        public abstract void OnHit(EquipBodyType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, int[] damage, Vector3 hitDir);
    }
}
