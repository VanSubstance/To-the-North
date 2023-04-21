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
    public abstract class AIBaseController : MonoBehaviour, ICreatureBattle
    {
        [SerializeField]
        private CreatureInfo info;

        public CreatureInfo Info
        {
            set
            {
                info = CreatureInfo.GetClone(value);
                if (info == null) return;
                sightCtrl.range = Info.sightRange;
                agent.speed = info.moveSpd;
                agent.stoppingDistance = WeaponRange - 0.5f;
            }
            get
            {
                return info;
            }
        }

        private NavMeshAgent agent;

        /// <summary>
        /// agent가 추적할 위치 설정 (절대 좌표)
        /// </summary>
        private Vector3 TargetMove
        {
            set
            {
                agent.SetDestination(value);
                agent.stoppingDistance = WeaponRange * 0.8f;
            }
            get
            {
                return agent.destination;
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
        /// 현재 목표 타겟 좌표 (절대 좌표) 설정
        /// 이동 종료 후 대기 시간 설정
        /// 이동 시, 해당 방향을 무조건 바라봄
        /// </summary>
        /// <param name="target">목표 좌표: 절대 좌표 기준</param>
        /// <param name="timeToStay">도착 후 대기 시간</param>
        /// <param name="isRandom">무작위성이 있는지</param>
        public void SetTargetToMove(Vector3? target, float timeToStay, bool isRandom = false)
        {
            if (target == null) return;
            Debug.DrawLine(transform.position, (Vector3)target, Color.green, 10f);
            TargetMove = CalculationFunctions.GetDetouredPositionIfInCollider(transform.position, (Vector3)target);
            isMoveOrderDone = false;
            timeStayAfterMove = timeToStay;
            SetTargetToGaze(TargetMove - transform.position, 0, false);
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
        /// 바라볼 방향 (상대 좌표) 설정 함수
        /// </summary>
        /// <param name="target">바라볼 방향 (상대 좌표)</param>
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

        public AIStatusType statusType;
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
            hitDir = hitDir.normalized;
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
            if (Info)
            {
                Info.LiveHp = -damage[0];
                if (Info.LiveHp <= 0)
                {
                    gameObject.SetActive(false);
                }
            }
            if (IsRunaway)
            {

            }
            else
            {
                statusType = AIStatusType.Combat;
                OnDetectUser(hitDir + transform.position);
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

        /// <summary>
        /// 예하 행동 컨트롤러들
        /// </summary>
        protected AbsAIStatusController[] statusCtrls;

        protected void Awake()
        {
            statusCtrls = GetComponents<AbsAIStatusController>();
            foreach (AbsAIStatusController ctrl in statusCtrls)
            {
                ctrl.BaseCtrl = this;
            }

            statusType = AIStatusType.None;
            agent = GetComponent<NavMeshAgent>();

            equipableBodies[EquipBodyType.Helmat] = hitTf.GetChild(0).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Mask] = hitTf.GetChild(1).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Head] = hitTf.GetChild(2).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Body] = hitTf.GetChild(3).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Leg] = hitTf.GetChild(4).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Right] = weaponL;
            equipableBodies[EquipBodyType.Left] = weaponR;

            if (Info == null) OnDisable();
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
            Info = info;
            isInit = true;
        }

        private void OnDisable()
        {
            sightCtrl.range = 0;
            Info = null;
            isInit = false;
        }

        /// <summary>
        /// 감지된 위치 절대 좌표 기준
        /// </summary>
        /// <param name="targetPos"></param>
        public abstract void OnDetectUser(Vector3? targetPos);
    }
}

