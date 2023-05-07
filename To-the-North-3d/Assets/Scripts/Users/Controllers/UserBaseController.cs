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

        private void Update()
        {
            tickHealthCondition += Time.deltaTime;
            if (tickHealthCondition > 1)
            {
                tickHealthCondition = 0;
                TickHealthCondition();
            }
            CheckSwapWeapon();
        }

        private void TickHealthCondition()
        {
            InGameStatus.User.status.ApplyHunger(-2);
            InGameStatus.User.status.ApplyThirst(-2);
            // 온도의 경우, 주변 환경에 따라서 오르거나 내리거나 유지되어야 할 듯
            //InGameStatus.User.status.temperatureBar.LiveInfo = +.5f;
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

        public override void OnHit(EquipBodyType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, int[] damage, Vector3 hitDir)
        {
            switch (partType)
            {
                case EquipBodyType.Helmat:
                case EquipBodyType.Mask:
                    if (Random.Range(0f, 1f) < .5)
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
            // 상태 이상 부여 심사

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
                if (Random.Range(0f, 1f) <= damage[2] / 100f)
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

        private void Start()
        {
            OccurCondition(ConditionType.Infection);;
        }
    }
}
