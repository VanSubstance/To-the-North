using System;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Controllers;
using Assets.Scripts.Creatures.Detections;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Items;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creatures.Bases
{
    public abstract class AIBaseController : AbsCreatureBaseController, IInteractionWithSight
    {

        [SerializeField]
        protected Transform visualTf;
        private Animator animCtrl;
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
                float r = WeaponRange > 1 ? WeaponRange : 1;
                if (Vector3.Distance(value, transform.position) > r)
                {
                    animCtrl.SetFloat("StatusMove", 1);
                    agent.SetDestination(value);
                    agent.stoppingDistance = r;
                } else
                {
                    SetTargetToGaze(value - transform.position, 0, false);
                }
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
                if (animCtrl.GetFloat("StatusMove").Equals(1))
                {
                    animCtrl.SetFloat("StatusMove", 0);
                }
                isMoveOrderDone = true;
                return;
            }
            // 안끝남
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
        public void SetTargetToMove(Vector3 target, float timeToStay, bool isRandom = false)
        {
            TargetMove = CalculationFunctions.GetDetouredPositionIfInCollider(transform.position, target);
            isMoveOrderDone = false;
            timeStayAfterMove = timeToStay;
            // 진행방향 응시
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

        /// <summary>
        /// 현재 행동 상태
        /// </summary>
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

        private float WeaponRange
        {
            get
            {
                if (weaponL == null && weaponR == null) return 1;
                float l = weaponL.Range(), r = weaponR.Range();
                return Mathf.Max(Mathf.Max(l, r), 1);
            }
        }

        public override void OnHit(EquipBodyType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, int[] damage, Vector3 hitDir)
        {
            try
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
                    statusType = AIStatusType.Runaway;
                }
                else
                {
                    if (!Info.IsActiveBehaviour) Info.IsActiveBehaviour = true;
                    statusType = AIStatusType.Combat;
                }
                OnDetectPosition(hitDir + transform.position);

                // 상태 이상 부여 심사
                if (damage[1] > 0)
                {
                    // 관통당했다
                }
            }
            catch (NullReferenceException)
            {

            }
        }

        public bool IsRunaway
        {
            get
            {
                return info.IsRunAway;
            }
        }

        public Transform targetTf;

        /// <summary>
        /// 공격할 대상 설정 함수
        /// </summary>
        /// <param name="_targetTf"></param>
        public void SetTargetToAttack(Transform _targetTf)
        {
            targetTf = _targetTf;
        }

        /// <summary>
        /// 공격 가능 여부 판단 함수
        /// </summary>
        public void CheckAim()
        {
            if (targetTf == null)
            {
                return;
            }
            if (Physics.Raycast(transform.position, targetTf.position, WeaponRange, GlobalStatus.Constant.obstacleMask))
            {
                targetTf = null;
                return;
            }
            if (Vector3.Distance(transform.position, targetTf.position) < Mathf.Min(WeaponRange, Info.sightRange))
            {
                SetTargetToGaze(targetTf.position - transform.position, 0, false);
                if (!weaponL.IsEmpty())
                {
                    animCtrl.SetFloat("StatusMove", 2);
                    weaponL.Use(targetTf.position - transform.position);
                }
                if (!weaponR.IsEmpty())
                {
                    animCtrl.SetFloat("StatusMove", 2);
                    weaponR.Use(targetTf.position - transform.position);
                }
                return;
            }
        }

        /// <summary>
        /// 예하 행동 컨트롤러들
        /// </summary>
        protected AbsAIStatusController[] statusCtrls;

        protected new void Awake()
        {
            statusCtrls = GetComponents<AbsAIStatusController>();
            foreach (AbsAIStatusController ctrl in statusCtrls)
            {
                ctrl.BaseCtrl = this;
            }

            statusType = AIStatusType.None;
            agent = GetComponent<NavMeshAgent>();
            animCtrl = visualTf.GetComponent<Animator>();
            base.Awake();

            if (Info == null) OnDisable();
            else OnEnable();
        }

        private void Update()
        {
            if (!isPause)
            {
                CheckMove();
                CheckGaze();
                CheckStatus();
                CheckParticle();
            }
        }

        private void CheckStatus()
        {
            if (statusType == AIStatusType.None)
            {
                statusType = AIStatusType.Wander;
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
        public abstract void OnDetectPosition(Vector3 targetPos);

        /// <summary>
        /// 감지된 유저
        /// </summary>
        /// <param name="userTf"></param>
        public abstract void OnDetectUser(Transform userTf);

        private float timeParticle = 0;

        private void CheckParticle()
        {
            if (timeParticle > 0)
            {
                timeParticle -= Time.deltaTime;
                return;
            }
        }
        protected void ChangeVisualOpacity(float _opacity)
        {
            Color c;
            SpriteRenderer s;
            for (int i = 0; i < visualTf.childCount; i++)
            {
                try
                {
                    c = (s = visualTf.GetChild(i).GetComponent<SpriteRenderer>()).color;
                    s.color = new Color(c.r, c.g, c.b, _opacity);
                }
                catch (MissingComponentException)
                {

                }
            }
        }

        public abstract void DetectFull();

        public abstract void DetectHalf();

        public abstract void DetectNone();

        public abstract void DetectSound(Vector3 _pos);
    }
}

