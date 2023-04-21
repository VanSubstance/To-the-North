using Assets.Scripts.Battles;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Detections;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creatures.Bases
{
    internal abstract class AIBaseController : MonoBehaviour, ICreatureBattle
    {
        [SerializeField]
        private CreatureInfo info;
        private NavMeshAgent agent;

        private Vector3 TargetMove
        {
            set
            {
                agent.SetDestination(value);
                agent.stoppingDistance = WeaponRange * 0.8f;
            }
        }

        private bool IsMoveDone
        {
            get
            {
                return agent.remainingDistance < WeaponRange;
            }
        }

        private float timeStayAfterMove = 0f;
        private bool isMoveOrderDone = false;

        private bool isInit = false;

        private void CheckMove()
        {
            if (isMoveOrderDone) return;
            if (!IsMoveDone) return;
            if (timeStayAfterMove <= 0)
            {
                // 끝남
                isMoveOrderDone = true;
                return;
            }
            timeStayAfterMove -= Time.deltaTime;
        }

        /// <summary>
        /// 현재 목표 타겟 좌표 설정
        /// 이동 종료 후 대기 시간 설정
        /// </summary>
        /// <param name="target">목표 좌표</param>
        /// <param name="timeToStay">도착 후 대기 시간</param>
        /// <param name="isRandom">무작위성이 있는지</param>
        public void SetTargetToMove(Vector3? target, float timeToStay, bool isRandom = false)
        {
            TargetMove = CalculationFunctions.GetDetouredPositionIfInCollider(transform.position, (Vector3)target);
            isMoveOrderDone = false;
            timeStayAfterMove = timeToStay;
        }

        [SerializeField]
        private DetectionSightController sightCtrl;

        private Vector3 TargetGaze
        {
            set
            {
                sightCtrl.Target = value;
            }
        }

        private bool IsGazeDone
        {
            get
            {
                return sightCtrl.IsGazeDone;
            }
        }

        private void CheckGaze()
        {
            if (isGazeOrderDone) return;
            if (!IsGazeDone) return;
            if (timeStayAfterGaze <= 0)
            {
                // 끝남
                isGazeOrderDone = true;
                return;
            }
            timeStayAfterGaze -= Time.deltaTime;
        }

        private float timeStayAfterGaze = 0f;
        private bool isGazeOrderDone = false;

        /// <summary>
        /// 바라볼 방향 (절대 좌표) 설정 함수
        /// </summary>
        /// <param name="target">바라볼 방향 (절대 좌표)</param>
        /// <param name="timeToStay">바라볼 시간</param>
        /// <param name="isRandom">무작위성이 있는지</param>
        public void SetTargetToGaze(Vector3 target, float timeToStay, bool isRandom = false)
        {
            TargetGaze = target;
            timeStayAfterGaze = timeToStay; ;
            isGazeOrderDone = false;
        }

        /// <summary>
        /// 모든 명령아 완료되었는지 확인 함수
        /// </summary>
        public bool IsOrderDone
        {
            get
            {
                return isGazeOrderDone && isMoveOrderDone;
            }
        }

        public float CurDegree
        {
            get
            {
                return sightCtrl.curDegree;
            }
        }

        public AIStatusType statusType = AIStatusType.None;
        private bool isPause = false;
        public bool IsPause
        {
            get
            {
                return isPause;
            }
            set
            {
                isPause = true;
            }
        }

        [SerializeField]
        private Transform hitTf;
        private Dictionary<EquipBodyType, IItemEquipable> equipableBodies = new Dictionary<EquipBodyType, IItemEquipable>();
        [SerializeField]
        private ItemWeaponController weaponL, weaponR;

        public CreatureInfo Info
        {
            get
            {
                return info;
            }
        }
        private float WeaponRange
        {
            get
            {
                if (weaponL == null && weaponR == null) return 0;
                float l = weaponL.Range(), r = weaponR.Range();
                return Mathf.Max(l, r);
            }
        }

        public void OnHit(EquipBodyType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, int[] damage, Vector3 hitDir)
        {
            transform.position = transform.position - (hitDir.normalized * 0.5f * attackInfo.powerKnockback);
            switch (partType)
            {
                case EquipBodyType.Helmat:
                    break;
                case EquipBodyType.Mask:
                    break;
                case EquipBodyType.Head:
                    break;
                case EquipBodyType.Body:
                    break;
                case EquipBodyType.Leg:
                    break;
            }
            // 계산 처리
            if (info)
            {
                info.LiveHp = -damage[0];
                if (info.LiveHp <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
            Debug.DrawLine(transform.position, hitDir + transform.position, Color.red, 10f);
            if (IsRunaway)
            {

            } else
            {
                SetTargetToGaze(hitDir, 3f, false);
            }
            //if (isRunAway)
            //{
            //    // 피격 반대 방향으로 개같이 런
            //    statusType = AIStatusType.Runaway;
            //    GetComponent<AIRunawayController>().RunawayFrom(hitDir);
            //}
            //else
            //{
            //    // 피격 당한쪽 바라보기
            //    SetTargetToGaze(hitDir, 3, false);
            //}

            // 상태 이상 부여 심사
            if (damage[1] > 0)
            {
                // 관통당했다
            }
        }

        private bool IsRunaway
        {
            get
            {
                return info.IsRunAway;
            }
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            sightCtrl.SetAIBaseController(this);

            equipableBodies[EquipBodyType.Helmat] = hitTf.GetChild(0).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Mask] = hitTf.GetChild(1).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Head] = hitTf.GetChild(2).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Body] = hitTf.GetChild(3).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Leg] = hitTf.GetChild(4).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Right] = weaponL;
            equipableBodies[EquipBodyType.Left] = weaponR;

            if (info == null) OnDisable();
            else OnEnable();
        }

        private void Update()
        {
            if (!isPause)
            {
                CheckMove();
                CheckGaze();
            }
        }

        private void OnEnable()
        {
            if (isInit) return;
            info = CreatureInfo.GetClone(info);
            sightCtrl.range = info.sightRange;
            isInit = true;
        }

        private void OnDisable()
        {
            sightCtrl.range = 0;
            info = null;
            isInit = false;
        }

        public abstract void OnDetectUser(Transform targetTf);
    }
}

