using System;
using System.Collections;
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
        private bool isAiming, isReloading;
        private SpriteRenderer sprite;

        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            if (isAI)
            {
                timeFocus = 0f;
                isAiming = false;
                isReloading = false;
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
            if (Input.GetKeyDown(KeyCode.R) && !isAI)
            {
                // 재장전
                if (info.isMagazineRequired())
                {
                    TryReload(InGameStatus.Item.LookForMagazine(info.bulletType));
                }
            }
        }
        public void Aim(Vector3 targetDir)
        {
            isAiming = true;
            float weight = 1;
            if (!isAI &&
                InGameStatus.User.IsConditionExist(ConditionConstraint.PerformanceLack.Accuracy))
            {
                weight = 2;
            }
            timeFocus = Mathf.Min(timeFocus + (Time.deltaTime / weight), timeFocusFull / weight);
        }

        public void Use(Vector3 targetDir)
        {
            if (isReloading) return; // 재장전중
            // 투사체 발사
            if (delayAmongFire >= info.delayAmongFire)
            {
                float randRange = (3 - timeFocus) / 3f;
                ProjectileInfo projInfo = info.GetProjectileInfo(isAI);
                if (projInfo == null)
                {
                    // 탄환이 없는 원거리 무기
                    // = 재장전 필요
                    Debug.Log("탄환 또는 탄창이 없습니다! 재장전이 필요합니다!");
                    if (isAI) TryReload(GlobalComponent.Path.GetMonsterMagazineInfo(info.bulletType, 1));
                    return;
                }
                else
                {
                    ProjectileManager.Instance.GetNewProjectile().Fire(projInfo, transform.position,
                        new Vector2(targetDir.x + randRange * UnityEngine.Random.Range(-1f, 1f), targetDir.y + randRange * UnityEngine.Random.Range(-1f, 1f))
                        , owner);
                }
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

        private void TryReload(ItemMagazineInfo newMagazine)
        {
            isReloading = true;
            StartCoroutine(CoroutineReload(newMagazine));
        }

        private IEnumerator CoroutineReload(ItemMagazineInfo newMagazine)
        {
            yield return new WaitForSeconds(info.timeReload);
            info.ReloadMagazine(newMagazine);
            isReloading = false;
        }
    }
}
