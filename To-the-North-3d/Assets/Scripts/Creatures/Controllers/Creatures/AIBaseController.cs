using System;
using Assets.Scripts.Commons;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Controllers;
using Assets.Scripts.Creatures.Detections;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Creatures.AI.Status;
using Assets.Scripts.Items;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Creatures.Bases
{
    public abstract class AIBaseController : AbsCreatureBaseController, IInteractionWithSight
    {

        [SerializeField]
        protected Transform visualTf, handTfIfExternal;
        private Animator animCtrl;
        public Animator AnimCtrl;
        [SerializeField]
        private float height = 1.5f;
        [SerializeField]
        private CreatureInfo info;

        public CreatureInfo Info
        {
            set
            {
                if (value == null) return;
                info = CreatureInfo.GetClone(value);
                agent.speed = info.moveSpd;
                agent.stoppingDistance = WeaponRange;
            }
            get
            {
                return info;
            }
        }

        private NavMeshAgent agent;
        public NavMeshAgent Agent
        {
            get
            {
                return agent;
            }
        }
        private Vector3? curTarget;
        public Vector3? CurTarget
        {
            get
            {
                return curTarget;
            }
        }
        public Transform targetTf;

        public int SightDirection;

        /// <summary>
        /// agent가 추적할 위치 설정 (절대 좌표)
        /// </summary>
        public Vector3 TargetMove
        {
            set
            {
                value = CalculationFunctions.GetDetouredPositionIfInCollider(transform.position, value);
                SightDirection = value.x - transform.position.x > 0 ? 0 : 180;
                animCtrl.SetBool("isMove", true);
                agent.SetDestination(value);
            }
            get
            {
                return agent.destination;
            }
        }

        private bool isInit = false;

        private IAIStatus aiStatus;
        public IAIStatus AiStatus
        {
            set
            {
                aiStatus = value;
                curTarget = null;
                targetTf = null;
            }
        }

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

        public float WeaponRange
        {
            get
            {
                if (weapon == null) return 1;
                return Mathf.Max(weapon.Range(), 1);
            }
        }

        public override void OnHit(EquipBodyType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, int[] damage, Vector3 hitDir)
        {
            animCtrl.SetTrigger("Hit");
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
                    // 죽음
                    // -> 루팅 프리펩 생성 필요
                    OnDied();
                }
            }
            OnDetectPosition(hitDir + transform.position);
            curTarget = hitDir + transform.position;
            // 전투 상태로 전환
            if (aiStatus is not CombatStatus)
            {
                agent.stoppingDistance = info.sightRange - 1;
                AiStatus = new CombatStatus();
            }
            // 상태 이상 부여 심사
            if (damage[1] > 0)
            {
                // 관통당했다
            }
            try
            {
            }
            catch (NullReferenceException)
            {
            }
        }

        /// <summary>
        /// 공격 가능 여부 판단 함수
        /// </summary>
        public bool CheckAim()
        {
            if (targetTf == null)
            {
                return false;
            }
            if (Physics.Raycast(transform.position, targetTf.position, WeaponRange, GlobalStatus.Constant.obstacleMask))
            {
                targetTf = null;
                return false;
            }
            if (Vector3.Distance(transform.position, targetTf.position) < Mathf.Min(WeaponRange, Info.sightRange))
            {
                if (!weapon.IsEmpty())
                {
                    foreach (AnimatorControllerParameter param in animCtrl.parameters)
                    {
                        if (param.Equals("Attack"))
                        {
                            animCtrl.SetTrigger("Attack");
                            break;
                        }
                    }
                    weapon.Use(targetTf.position - transform.position);
                    return true;
                }
            }
            return false;
        }

        protected new void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            animCtrl = visualTf.GetComponent<Animator>();
            base.Awake();
            AiStatus = new IdleStatus();
            SightDirection = 1;

            if (Info == null) OnDisable();
            else OnEnable();
        }

        private void Update()
        {
            if (!isPause)
            {
                // 시야를 통한 타겟 업데이트
                if (DetectionController.TryGetTarget(transform.position, Info.sightRange, GlobalStatus.Constant.userMask, GlobalStatus.Constant.obstacleMask, out Transform res))
                {
                    // 시야 안에 타겟 존재 = 타겟 업데이트
                    curTarget = res.position;
                    if (info.IsActiveBehaviour && aiStatus is not CombatStatus)
                    {
                        AiStatus = new CombatStatus();
                    }
                    if (aiStatus is CombatStatus)
                    {
                        // 공격용 타겟 추가 설정
                        targetTf = res;
                    }
                }
                else
                {
                    curTarget = null;
                    targetTf = null;
                }

                if (agent.isStopped)
                {
                    animCtrl.SetBool("isMove", false);
                }
            }
        }

        private void LateUpdate()
        {
            if (!isPause)
            {
                aiStatus.UpdateAction(this, targetTf ? targetTf.position : curTarget);
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
            Info = null;
            isInit = false;
        }

        /// <summary>
        /// 감지된 위치 절대 좌표 기준
        /// </summary>
        /// <param name="targetPos"></param>
        public abstract void OnDetectPosition(Vector3 targetPos);

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
                if ((s = visualTf.GetChild(i).GetComponent<SpriteRenderer>()) != null)
                {
                    c = s.color;
                    s.color = new Color(c.r, c.g, c.b, _opacity);
                }
            }
            if (handTfIfExternal)
            {
                c = (s = handTfIfExternal.GetComponent<SpriteRenderer>()).color;
                s.color = new Color(c.r, c.g, c.b, _opacity);
            }
        }

        public override float GetHeight()
        {
            return height;
        }

        public abstract void DetectFull();

        public abstract void DetectHalf();

        public abstract void DetectNone();

        public abstract void DetectSound(Vector3 _pos);
    }
}

