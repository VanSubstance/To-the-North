using System;
using System.Collections;
using Assets.Scripts.Battles;
using Assets.Scripts.Commons;
using Assets.Scripts.Components.Windows.Inventory;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Users;
using UnityEngine;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// 손에 장착 가능 + 무기 컨트롤러
    /// </summary>
    public class ItemWeaponController : MonoBehaviour, IItemHandable, IItemEquipable
    {
        [SerializeField]
        private SpriteRenderer handL;
        [SerializeField]
        private ItemWeaponInfo info;
        [SerializeField]
        private bool isAI = true;
        private Transform owner;
        public Transform Owner
        {
            set
            {
                owner = value;
            }
        }

        private Animator anim;

        public Animator Anim
        {
            get
            {
                return transform.parent.GetComponent<Animator>();
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
        [SerializeField]
        private SpriteRenderer sprite;

        private void Awake()
        {
            if (isAI)
            {
                TimeFocus = 0f;
                isAiming = false;
                isReloading = false;
                if (info)
                {
                    if (sprite)
                    {
                        sprite.sprite = Resources.Load<Sprite>(GlobalComponent.Path.GetImagePath(info));
                    }
                    delayAmongFire = info.delayAmongFire;

                    if (info.handType.Equals(EquipHandType.Clone))
                    {
                        if (handL)
                        {
                            handL.sprite = sprite.sprite;
                        }
                    }
                }
            }
            anim = transform.parent.GetComponent<Animator>();
        }

        private void Update()
        {
            if (info == null) return;
            TrackReleaseAiming();
            isAiming = false;
            if (!InGameStatus.User.isInAction &&
                Input.GetKey(KeyCode.R) && !isAI && !isReloading)
            {
                // 재장전
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
                    if (isAI) TryReload(Instantiate(GlobalComponent.Path.GetMonsterMagazineInfo(info.bulletType, 1)));
                    return;
                }
                else
                {
                    ProjectileManager.Instance.GetNewProjectile().Fire(projInfo, new Vector3(transform.position.x, owner.GetComponent<ICreatureBattle>().GetHeight(), transform.position.z),
                        CalculationFunctions.DirFromAngle(CalculationFunctions.AngleFromDir(new Vector2(targetDir.x, targetDir.z)) + UnityEngine.Random.Range(-InGameStatus.User.Detection.Sight.DegreeError,
                        InGameStatus.User.Detection.Sight.DegreeError)),
                        owner,
                        info.bulletType);
                    if (anim)
                    {
                        if (info.bulletType.Equals(ItemBulletType.None))
                        {
                            // 근접
                            switch (info.TrajectoryType)
                            {
                                case TrajectoryType.Straight:
                                    anim.SetTrigger("Step");
                                    break;
                                case TrajectoryType.Curve:
                                    anim.SetTrigger("Swing");
                                    break;
                            }
                        }
                        else
                        {
                            anim.SetTrigger("Shot");
                        }
                    }
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
                }
                catch (InvalidCastException)
                {
                    // 장비 정보가 오염됨
                }
            }
        }

        public bool IsEmpty()
        {
            return info == null;
        }

        private void TryReload(ItemMagazineInfo newMagazine)
        {
            isReloading = true;
            if (isAI)
            {
                // AI인 경우 =-> 걍 장전
                newMagazine.LoadMagazine(Instantiate(GlobalComponent.Path.GetMonsterBulletInfo(newMagazine.bulletType, 1)));
                StartCoroutine(CoroutineReload(newMagazine));
                return;
            }
            if (InGameStatus.User.isInAction) return;
            StartCoroutine(CoroutineReload(newMagazine));
        }

        private IEnumerator CoroutineReload(ItemMagazineInfo newMagazine)
        {
            owner.GetComponent<ISoundable>().PlaySoundByType(Creatures.SoundType.Reload);
            if (!isAI)
            {
                InGameStatus.User.isInAction = true;
                UserBaseController.Instance.progress.CurProgress = 0;
            }
            float w = 1;
            if (!isAI && InGameStatus.User.IsConditionExist(ConditionConstraint.PerformanceLack.SpeedReload))
            {
                w *= 1.5f;
            }
            float tRemaining = info.timeReload * w, p = 1;
            while (tRemaining > 0f)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                p = Time.deltaTime / tRemaining;
                if (!isAI)
                    UserBaseController.Instance.progress.CurProgress += p;
                tRemaining -= Time.deltaTime;
            }
            ItemMagazineInfo oldMagazine = info.ReloadMagazine(newMagazine);
            owner.GetComponent<ISoundable>().StopSound();
            if (!isAI)
            {
                if (oldMagazine != null)
                {
                    // 총에 꽃혀있던 탄창이 있음
                    // = 인벤토리에 넣어야 함
                    // -> 풀링에서 하나 가져와서 신규 생성 및 넣어주기
                    WindowInventoryController.Instance.GenerateItemObjectWithAuto(ContentType.Inventory, oldMagazine);
                }
                else
                {
                    // 꽃혀있던 탄창이 없음
                }
                InGameStatus.User.isInAction = false;
            }
            isReloading = false;
        }
    }
}
