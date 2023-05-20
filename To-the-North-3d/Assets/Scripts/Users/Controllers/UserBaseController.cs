using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Components.Progress;
using Assets.Scripts.Creatures.Controllers;
using Assets.Scripts.Items;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Creatures;

namespace Assets.Scripts.Users
{
    public class UserBaseController : AbsCreatureBaseController
    {
        public ProgressBarSpriteController progress;


        public Vector3 position
        {
            set => transform.position = value;
            get => transform.position;
        }
        public float x
        {
            get => transform.position.x;
        }
        public float z
        {
            get => transform.position.z;
        }

        private float tickHealthCondition;

        [SerializeField]
        private Animator anim;

        private static UserBaseController _instance;
        // 인스턴스에 접근하기 위한 프로퍼티
        public static UserBaseController Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(UserBaseController)) as UserBaseController;

                    if (_instance == null)
                    {
                        // 아직 유저 오브젝트 없음
                    }
                }
                return _instance;
            }
        }

        private new void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
            DontDestroyOnLoad(gameObject);

            base.Awake();
            tickHealthCondition = 0;
        }

        private void Start()
        {
            //OccurCondition(ConditionType.Bleeding_Light);
            //OccurCondition(ConditionType.Dizziness, true);
        }

        private void Update()
        {
            tickHealthCondition += Time.deltaTime;
            if (tickHealthCondition > 1)
            {
                tickHealthCondition = 0;
                //TickHealthCondition();
            }
            CheckConditions();
            CheckSwapWeapon();
        }

        private void TickHealthCondition()
        {
            // 온도의 경우, 주변 환경에 따라서 오르거나 내리거나 유지되어야 할 듯
            //InGameStatus.User.status.temperatureBar.LiveInfo = +.5f;

            // 허기 속도
            if (InGameStatus.User.IsConditionExist(ConditionConstraint.PerformanceLack.SpeedHunger))
            {
                InGameStatus.User.status.ApplyHunger(-3);
            }
            else
            {
                InGameStatus.User.status.ApplyHunger(-1);
            }

            // 갈증 속도
            if (InGameStatus.User.IsConditionExist(ConditionConstraint.PerformanceLack.SpeedThirst))
            {
                InGameStatus.User.status.ApplyThirst(-3);
            } else
            {
                InGameStatus.User.status.ApplyThirst(-1);
            }

            // 스테미나 틱
            if (InGameStatus.User.IsConditionExist(ConditionConstraint.Tick.Stamina))
            {
                InGameStatus.User.status.ApplyStamina(-ConditionConstraint.Tick.TickAmountForCondition[ConditionType.Exhaust]);
            }
        }

        private void CheckSwapWeapon()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SwapWeapon();
            }
        }

        /// <summary>
        /// 주/부 무기 교체
        /// </summary>
        private void SwapWeapon()
        {
            if (curWeapon == 0) return;
            if (curWeapon == 1)
            {
                // 주 -> 부
                if (weapons[1] != null)
                {
                    curWeapon = 2;
                    equipableBodies[EquipBodyType.Hand].ChangeEquipment(weapons[1]);
                }
                return;
            }
            // 부 -> 주
            if (weapons[0] != null)
            {
                curWeapon = 1;
                equipableBodies[EquipBodyType.Hand].ChangeEquipment(weapons[0]);
            }
            return;
        }

        /// <summary>
        /// 장비 바꿔끼기 함수
        /// </summary>
        /// <param name="targetType">목표 부위</param>
        /// <param name="itemInfo">장착할 아이템 정보</param>
        public void ChangeEquipment(EquipBodyType targetType, ItemEquipmentInfo itemInfo)
        {
            switch (targetType)
            {
                case EquipBodyType.Primary:
                    weapons[0] = itemInfo;
                    if (weapons[0] != null)
                    {
                        // 장비 장착임
                        curWeapon = 1;
                        equipableBodies[EquipBodyType.Hand].ChangeEquipment(weapons[0]);
                    }
                    else
                    {
                        // 장비 해제임 -> 주무기가 비었다 -> 부무기 장착 시도
                        if (weapons[1] != null)
                        {
                            // 부무기는 있다 -> 장착
                            curWeapon = 2;
                            equipableBodies[EquipBodyType.Hand].ChangeEquipment(weapons[1]);
                        }
                        else
                        {
                            // 부무기도 없다 -> 걍 장착 해제임
                            curWeapon = 0;
                            equipableBodies[EquipBodyType.Hand].ChangeEquipment(null);
                        }
                    }
                    return;
                case EquipBodyType.Secondary:
                    weapons[1] = itemInfo;
                    if (weapons[0])
                    {
                        curWeapon = 1;
                    }
                    else if (weapons[1])
                    {
                        curWeapon = 2;
                    }
                    else
                    {
                        curWeapon = 0;
                    }
                    equipableBodies[EquipBodyType.Hand].ChangeEquipment(weapons[0] ?? weapons[1]);
                    return;
            }
            equipableBodies[targetType].ChangeEquipment(itemInfo);
        }

        /// <summary>
        /// 피격 시 효과 적용 함수
        /// </summary>
        /// <param name="partType"></param>
        /// <param name="armorInfo"></param>
        /// <param name="attackInfo"></param>
        /// <param name="damage"></param>
        /// <param name="hitDir"></param>
        public override void OnHit(EquipBodyType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, int[] damage, Vector3 hitDir)
        {
            if (InGameStatus.User.IsConditionExist(ConditionConstraint.Possibility.Dizziness))
            {
                if (Random.Range(0f, 1f) < .15f)
                {
                    // 피격 시 15% 확률 어지러움
                    OccurCondition(ConditionType.Dizziness);
                }
            }
            else
            {
                switch (partType)
                {
                    case EquipBodyType.Helmat:
                    case EquipBodyType.Mask:
                        if (Random.Range(0f, 1f) < .35f)
                        {
                            // 헬멧 또는 마스크 피격 시 50% 확률 어지러움
                            OccurCondition(ConditionType.Dizziness);
                        }
                        break;
                    case EquipBodyType.Body:
                        break;
                    case EquipBodyType.Leg:
                        break;
                }
            }

            if (damage[1] > 0)
            {
                // 관통당함
                // 심각한 출혈 심사
                if (Random.Range(0f, 1f) <= Mathf.Pow(damage[1] / 100f, 1.5f))
                {
                    // 심각한 출혈 발생
                    OccurCondition(ConditionType.Bleeding_Heavy);
                    // 통증 동시 발생
                    OccurCondition(ConditionType.Pain);
                }
            }

            if (damage[2] > 0)
            {
                // 충격 데미지
                // 얉은 출혈 심사
                float w = InGameStatus.User.IsConditionExist(ConditionConstraint.Possibility.Bleeding_Light) ? .1f : 0;
                if (Random.Range(0f, 1f) - w <= damage[2] / 100f)
                {
                    // 얕은 출혈 발생
                    OccurCondition(ConditionType.Bleeding_Light);
                }
                // 골절 심사
                if (Random.Range(0f, 1f) <= Mathf.Pow(damage[1] / 100f, 1.7f))
                {
                    // 골절 발생
                    OccurCondition(ConditionType.Fracture);
                    // 통증 동시 발생
                    OccurCondition(ConditionType.Pain);
                }
                if (damage[2] > 25)
                {
                    OccurCondition(ConditionType.Dizziness);
                }
            }

            // 피격 이벤트 처리
            anim.SetTrigger("Hit");
            CommonGameManager.Instance.OnHit(CalculationFunctions.AngleFromDir(new Vector2(hitDir.x, hitDir.z)), damage);
            return;
        }

        /// <summary>
        /// 상태 이상 발생 함수
        /// </summary>
        /// <param name="targetCondition">발생할 상태 이상</param>
        public void OccurCondition(ConditionType targetCondition, bool isSingle = false)
        {
            if (isSingle && InGameStatus.User.conditions[targetCondition] > 0) return;
            InGameStatus.User.conditions[targetCondition]++;
            ConditionManager.Instance.AwakeCondition(targetCondition);
        }

        public void CureCondition(ConditionType targetCondition, int cnt)
        {
            InGameStatus.User.conditions[targetCondition] -= cnt;
            if (InGameStatus.User.conditions[targetCondition] <= 0)
            {
                InGameStatus.User.conditions[targetCondition] = 0;
                ConditionManager.Instance.AsleepCondition(targetCondition);
            }
        }

        public override float GetHeight()
        {
            return GetComponent<AbsCreatureActionController>().CurHeight;
        }

        private float timeForStamina = 0;
        private bool isTriggerStamina = false;

        /// <summary>
        /// 지속적으로 상태이상 발생/소멸 조건 여부를 판별하는 함수
        /// </summary>
        private void CheckConditions()
        {
            // 1. 어지러움
            if (InGameStatus.User.IsConditionExist(ConditionConstraint.PerformanceLack.Dizziness))
            {
                CommonGameManager.Instance.IsDizziness = true;
            }
            else
            {
                CommonGameManager.Instance.IsDizziness = false;
            }

            // 2. 피로
            if (InGameStatus.User.status.staminaBar.LivePercent < .25f)
            {
                timeForStamina += Time.deltaTime;
                if (timeForStamina > 10)
                {
                    if (!isTriggerStamina)
                    {
                        isTriggerStamina = true;
                        OccurCondition(ConditionType.Exhaust, true);
                    }
                }
            } else
            {
                if (isTriggerStamina)
                {
                    if (InGameStatus.User.status.staminaBar.LivePercent > .99f)
                    {
                        CureCondition(ConditionType.Exhaust, 1);
                    }
                } else
                {
                    timeForStamina = 0;
                }
            }

            // 3. 체온
            if (InGameStatus.User.status.temperatureBar.LivePercent >= .65f)
            {
                // 체온 65% 이상 -> 더위 발생
                OccurCondition(ConditionType.Hot, true);
            } else if (InGameStatus.User.status.temperatureBar.LivePercent <= .6f)
            {
                // 60% 이하 -> 더위 제거
                CureCondition(ConditionType.Hot, 1);
                if (InGameStatus.User.status.temperatureBar.LivePercent >= .4f)
                {
                    // 40% 이상 -> 추위 제거
                    CureCondition(ConditionType.Cold, 1);
                } else if (InGameStatus.User.status.temperatureBar.LivePercent <= .35f)
                {
                    // 35% 이하 -> 추위 발생
                    OccurCondition(ConditionType.Cold, true);
                }
            }
        }

        public override void OnDied()
        {
        }
    }
}
