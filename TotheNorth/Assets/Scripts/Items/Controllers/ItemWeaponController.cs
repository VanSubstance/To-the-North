using System;
using Assets.Scripts.Battles;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Users;
using UnityEngine;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// 손에 장착 가능 + 무기 컨트롤러
    /// </summary>
    internal class ItemWeaponController : MonoBehaviour, IItemHandable, IItemEquipable
    {
        [SerializeField]
        private ItemWeaponInfo info;
        [SerializeField]
        private bool isAI = true;
        private Transform owner;

        private float delayAmongFire, timeFocus, timeFocusFull = 3f;
        private bool isAiming;
        private SpriteRenderer sprite;
        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            if (isAI)
            {
                timeFocus = 0f;
                isAiming = false;
                owner = transform.parent.parent.parent;
                if (info)
                {
                    sprite.sprite = Resources.Load<Sprite>(GlobalComponent.Path.GetImagePath(info));
                    delayAmongFire = info.delayAmongFire;
                }
            }
        }

        private void Update()
        {
            if (info == null) return;
            if (delayAmongFire >= info.delayAmongFire) return;
            delayAmongFire += Time.deltaTime;
            if (!isAiming)
            {
                timeFocus = Mathf.Max(timeFocus - Time.deltaTime, 0);
            }
            isAiming = false;
        }
        public void Aim(Vector3 targetDir)
        {
            isAiming = true;
            float weight = 1;
            if ( !isAI &&
                InGameStatus.User.IsConditionExist(ConditionConstraint.PerformanceLack.Accuracy))
            {
                weight = 2;
            }
            timeFocus = Mathf.Min(timeFocus + (Time.deltaTime / weight), timeFocusFull / weight);
        }

        public void Use(Vector3 targetDir)
        {
            // 투사체 발사
            //Debug.Log(info);
            if (delayAmongFire >= info.delayAmongFire)
            {
                float randRange = (3 - timeFocus) / 3f;
                ProjectileInfo projInfo = new();
                switch (info.bulletType)
                {
                    case ItemBulletType.None:
                        // 총알 필요 없음
                        projInfo = info.GetProjectileInfo();
                        break;
                    case ItemBulletType.Arrow:
                        // 탄환 필요함
                        // => 이게 이렇게 되면 안됨, 탄환 정보가 필요함 + 탄환 소비가 필요
                        if (!isAI)
                        {
                            projInfo = info.GetProjectileInfo(InGameStatus.Item.LookforBullet(info.bulletType));
                        }
                        else
                        {
                            projInfo = info.GetProjectileInfo(Instantiate(
                                GlobalComponent.Path.GetMonsterBulletInfo(ItemBulletType.Arrow, 1)
                                ));
                        }
                        break;
                }
                if (projInfo == null) return;
                ProjectileManager.Instance.GetNewProjectile().Fire(projInfo, transform.position,
                    new Vector2(targetDir.x + randRange * UnityEngine.Random.Range(-1f, 1f), targetDir.y + randRange * UnityEngine.Random.Range(-1f, 1f))
                    , owner);
                delayAmongFire = 0f;
            }
        }

        public float Range()
        {
            return info.range;
        }

        public void ChangeEquipment(ItemEquipmentInfo _info)
        {
            try
            {
                info = (ItemWeaponInfo)_info;
                timeFocus = 0f;
                isAiming = false;
                owner = transform.parent.parent.parent;
                sprite.sprite = Resources.Load<Sprite>(GlobalComponent.Path.GetImagePath(info));
                delayAmongFire = info.delayAmongFire;
            }
            catch (InvalidCastException)
            {
                // 장비 정보가 오염됨
            }
        }

        public bool IsEmpty() => info == null;
    }
}
