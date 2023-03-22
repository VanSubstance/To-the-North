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
        private DetectionPassiveController passiveController;
        private DetectionSightController sightController;
        private bool isPause = false;

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
            }
        }

        private void ControllMovement()
        {
            if (targetToMove != null)
            {
                MoveToTarget();
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
        /// 타겟 방향으로 시야를 회전하는 함수 (전부가 아닌 조금씩)
        /// </summary>
        private void GazeTarget()
        {
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
                targetToGaze = null;
                return;
            }
        }

        /// <summary>
        /// 타겟 방향으로 이동하는 함수 (전부가 아닌 조금씩)
        /// </summary>
        private void MoveToTarget()
        {
            if (Vector2.Distance(transform.position, (Vector3)targetToMove) < 0.1f)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                targetToMove = null;
                return;
            }
            GetComponent<Rigidbody2D>().velocity = ((Vector3)targetToMove - transform.position).normalized * 2;
        }

        public bool isAllActDone()
        {
            return
                targetToMove == null &&
                targetToGaze == null &&
                true;
        }

        public void PauseOrResumeAct(bool toPause)
        {
            isPause = toPause;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        public abstract void OnDetectUser(Transform targetTf);
    }
}
