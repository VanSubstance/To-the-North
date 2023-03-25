using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Users.Controllers;
using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    internal abstract class AIBaseController : MonoBehaviour
    {
        public AIStatusType statusType = AIStatusType.Petrol;
        public Vector3? targetToMove, targetToGaze;
        protected AISquadBaseController squadBase;
        private DetectionPassiveController passiveController;
        private DetectionSightController sightController;
        private bool isPause = false;
        private float timeStayForMove = 0, timeStayForGaze = 0;
        public bool isOrderMoveDone = true, isOrderGazeDone = true;

        private void Awake()
        {
            Transform temp = transform.Find("Detection Controller");
            passiveController = temp.Find("Passive").GetComponent<DetectionPassiveController>();
            sightController = temp.Find("Sight").GetComponent<DetectionSightController>();
            passiveController.SetAIBaseController(this);
            sightController.SetAIBaseController(this);
        }

        private void Update()
        {
            if (!isPause)
            {
                ControllMovement();
                ControllGaze();
                ControllIdle();
            }
        }

        private void ControllMovement()
        {
            if (targetToMove != null)
            {
                MoveToTarget();
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }

        private void ControllGaze()
        {
            if (targetToGaze != null)
            {
                GazeTarget();
            }
        }

        /// <summary>
        /// 모든 행동이 끝나고 0.5초동안 아무런 명령을 받지 않았을 경우 자율 행동 제어
        /// </summary>
        private void ControllIdle()
        {

        }

        /// <summary>
        /// 타겟 방향으로 시야를 회전하는 함수 (전부가 아닌 조금씩)
        /// </summary>
        private void GazeTarget(bool isAuto = false)
        {
            if (!isAuto) isOrderGazeDone = false;
            float targetDegree = CalculationFunctions.AngleFromDir((Vector3)targetToGaze - transform.position);
            targetDegree += 360 * 3;
            targetDegree -= sightController.curDegree;
            targetDegree %= 360;
            if (targetDegree > 1)
            {
                if (targetDegree < 180)
                {
                    sightController.AddRotationDegree(1);
                }
                else
                {
                    sightController.AddRotationDegree(-1);
                }
            }
            else
            {
                if (timeStayForGaze > 0)
                {
                    timeStayForGaze -= Time.deltaTime;
                }
                else
                {
                    targetToGaze = null;
                    isOrderGazeDone = true;
                }
                return;
            }
        }

        /// <summary>
        /// 타겟 방향으로 이동하는 함수 (전부가 아닌 조금씩)
        /// </summary>
        private void MoveToTarget(bool isAuto = false)
        {
            if (!isAuto) isOrderMoveDone = false;
            if (Vector2.Distance(transform.position, (Vector3)targetToMove) < 0.1f)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (timeStayForMove > 0)
                {
                    timeStayForMove -= Time.deltaTime;
                }
                else
                {
                    targetToMove = null;
                    isOrderMoveDone = true;
                }
                return;
            }
            GetComponent<Rigidbody2D>().velocity = ((Vector3)targetToMove - transform.position).normalized * 2;
        }

        public bool isAllActDone()
        {
            return
                targetToMove == null &&
                timeStayForMove <= 0 &&
                targetToGaze == null &&
                timeStayForGaze <= 0 &&
                true;
        }

        public void PauseOrResumeAct(bool toPause)
        {
            isPause = toPause;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        public DetectionSightController GetDetectionSight()
        {
            return sightController;
        }

        public DetectionPassiveController GetDetectionPassive()
        {
            return passiveController;
        }

        public void SetTargetToMove(Vector3? target, float timeToStay)
        {
            targetToMove = target;
            timeStayForMove = timeToStay;
        }

        public void SetTargetToGaze(Vector3? target, float timeToStay, bool isRandom = false)
        {
            if (target == null)
            {
                targetToGaze = sightController.GetPositionOfLooking(Random.Range(-90, 90));
            }
            else
            {
                if (isRandom)
                {
                    targetToGaze = new Vector3(
                        ((Vector3)target).x + Random.Range(-2f, 2f),
                        ((Vector3)target).y + Random.Range(-2f, 2f),
                        0);
                }
                else
                {
                    targetToGaze = target;
                }
            }
            timeStayForGaze = timeToStay;
        }

        public void SetSquadBase(AISquadBaseController _squadBase)
        {
            squadBase = _squadBase;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("충돌!!");
            Debug.Log(GetComponent<Rigidbody2D>().velocity);
        }

        public abstract void OnDetectUser(Transform targetTf);
    }
}
