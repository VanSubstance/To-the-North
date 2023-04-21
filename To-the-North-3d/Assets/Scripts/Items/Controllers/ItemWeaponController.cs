using System;
using System.Collections;
using Assets.Scripts.Battles;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Users;
using UnityEditor.Rendering;
using UnityEngine;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// 손에 장착 가능 + 무기 컨트롤러
    /// </summary>
    public class ItemWeaponController : MonoBehaviour, IItemHandable, IItemEquipable
    {
        [SerializeField]
        private ItemWeaponInfo info;
        [SerializeField]
        private bool isAI = true, isMagazineEmpty;
        private Transform owner;
        public Transform Owner
        {
            set
            {
                owner= value;
            }
        }

        private float delayAmongFire, timeFocusFull = 3f;
        private float timeFocus;
        private float TimeFocus
        {
            get
            {
                return timeFocus;
            }
            set
            {
                timeFocus = value;
                if (!isAI)
                    InGameStatus.User.Detection.Sight.DegreeError = 10 * (timeFocusFull - timeFocus) / 3f;
            }
        }
        private bool isAiming, isReloading;
        private SpriteRenderer sprite;

        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
            isMagazineEmpty = false;
            if (isAI)
            {
                TimeFocus = 0f;
                isAiming = false;
                isReloading = false;
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
            TrackReleaseAiming();
            isAiming = false;
            if (Input.GetKey(KeyCode.R) && !isAI && isMagazineEmpty && !isReloading)
            {
                // 재장전
                isReloading = true;
                TryReload(InGameStatus.Item.LookForMagazine(info.bulletType));
            }
            if (delayAmongFire >= info.delayAmongFire) return;
            delayAmongFire += Time.deltaTime;
        }

        private void TrackReleaseAiming()
        {
            if (!isAiming && TimeFocus > 0)
            {
                TimeFocus = Mathf.Max(TimeFocus - (3 * Time.deltaTime), 0);
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
            TimeFocus = Mathf.Min(TimeFocus + (Time.deltaTime / weight), timeFocusFull / weight);
        }

        public void Use(Vector3 targetDir)
        {
            if (isReloading) return; // 재장전중
            // 투사체 발사
            if (delayAmongFire >= info.delayAmongFire)
            {
                ProjectileInfo projInfo = info.GetProjectileInfo(isAI);
                if (projInfo == null)
                {
                    // 탄환이 없는 원거리 무기
                    // = 재장전 필요
                    Debug.Log("재장전 필요!");
                    if (isAI) TryReload(GlobalComponent.Path.GetMonsterMagazineInfo(info.bulletType, 1));
                    else
                        isMagazineEmpty = true;
                    return;
                }
                else
                {
                    ProjectileManager.Instance.GetNewProjectile().Fire(projInfo, transform.position,
                        CalculationFunctions.DirFromAngle(CalculationFunctions.AngleFromDir(new Vector2(targetDir.x, targetDir.z)) + UnityEngine.Random.Range(-InGameStatus.User.Detection.Sight.DegreeError,
                        InGameStatus.User.Detection.Sight.DegreeError))
                        , owner);
                }
                delayAmongFire = 0f;
            }
        }

        public float Range()
        {
            if (info == null) return 0;
            return info.range;
        }

        public void ChangeEquipment(ItemEquipmentInfo _info)
        {
            if (_info == null)
            {
                info = null;
                sprite.sprite = null;
                Debug.Log("장비 뺌 or null 들어옴");
            }
            else
            {
                try
                {
                    info = (ItemWeaponInfo)_info;
                    TimeFocus = 0f;
                    isAiming = false;
                    sprite.sprite = Resources.Load<Sprite>(GlobalComponent.Path.GetImagePath(info));
                    delayAmongFire = info.delayAmongFire;
                    Debug.Log("장비 착용함");
                }
                catch (InvalidCastException)
                {
                    // 장비 정보가 오염됨
                    Debug.Log("장비 정보가 오염 됨");
                }
            }
        }

        public bool IsEmpty() => info == null;

        private void TryReload(ItemMagazineInfo newMagazine)
        {
            StartCoroutine(CoroutineReload(newMagazine));
        }

        private IEnumerator CoroutineReload(ItemMagazineInfo newMagazine)
        {
            Debug.Log("Reload Starts ...");
            float w = 1;
            if (!isAI && InGameStatus.User.IsConditionExist(ConditionConstraint.PerformanceLack.SpeedReload))
            {
                w *= 1.5f;
            }
            yield return new WaitForSeconds(info.timeReload * w);
            ItemMagazineInfo temp = info.ReloadMagazine(newMagazine);
            if (temp != null)
            {
                InGameStatus.Item.PutItemToInventory(temp);
            }
            isReloading = false;
            if (!isAI)
            {
                isMagazineEmpty = false;
            }
            Debug.Log("Reload Complete!");
        }
    }
}
