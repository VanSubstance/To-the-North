using System.Collections.Generic;
using Assets.Scripts.Battles;
using Assets.Scripts.Commons;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    public abstract class AbsCreatureBaseController : MonoBehaviour, ICreatureBattle, ISoundable
    {

        [SerializeField]
        protected Transform hitTf;
        protected Dictionary<EquipBodyType, IItemEquipable> equipableBodies = new Dictionary<EquipBodyType, IItemEquipable>();
        [SerializeField]
        protected ItemWeaponController weaponL, weaponR;

        protected void Awake()
        {
            Speaker = gameObject.AddComponent<AudioSource>();
            Speaker.loop = true;
            Speaker.playOnAwake = false;

            equipableBodies[EquipBodyType.Helmat] = hitTf.GetChild(0).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Mask] = hitTf.GetChild(1).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Head] = hitTf.GetChild(2).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Body] = hitTf.GetChild(3).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Leg] = hitTf.GetChild(4).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Right] = weaponL;
            equipableBodies[EquipBodyType.Left] = weaponR;
            weaponL.Owner = transform;
            weaponR.Owner = transform;
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
