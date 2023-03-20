using System.Collections;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Creatures.Objects;
using Assets.Scripts.Users.Controllers;
using UnityEngine;
using static GlobalComponent.Common;

namespace Assets.Scripts.Creatures.Abstracts
{
    internal class AAIBaseController : MonoBehaviour, IAIAct
    {
        [SerializeField]
        private AAIConductionBaseController defaultConductionController;
        [SerializeField]
        private DetectionPassiveController detectionPassiveController;
        [SerializeField]
        private DetectionSightController detectionSightController;

        protected AIConductionType curConductionType;
        protected int curStatus = 0;
        private Vector3 targetDirection;
        private AIMoveInfo moveTarget = null;
        private AIGazeInfo gazeTarget = null;
        private bool isMoveApplied = false, isGazeApplied = false;

        private void Update()
        {
            switch (curStatus)
            {
                case 0:
                    // 행동강령 자유 상태
                    InitDefaultConduction();
                    break;
                case -1:
                    // 행동 일시 정지
                    break;
            }
            if (moveTarget != null)
            {
                // 이동
                if (Vector2.Distance(transform.localPosition, moveTarget.point()) <= 0.1f)
                {
                    //  이돌 종료
                    //transform.GetComponent<Rigidbody2D>().AddForce(-targetDirection);
                    transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    targetDirection = Vector3.zero;
                    moveTarget = null;
                    isMoveApplied = false;
                }
                else
                {
                    if (!isMoveApplied)
                    {
                        isMoveApplied = true;
                        targetDirection = new Vector3(moveTarget.point().x, moveTarget.point().y, 0f) - transform.localPosition;
                        //transform.GetComponent<Rigidbody2D>().AddForce(targetDirection * moveTarget.spdMove);
                        transform.GetComponent<Rigidbody2D>().velocity = targetDirection.normalized * moveTarget.spdMove;
                        if (gazeTarget == null || gazeTarget.secWait == 0)
                        {
                            //Debug.Log("이동 방향으로 응시:: " + (int)CalculationFunctions.AngleFromDir(targetDirection));
                            Gaze(new AIGazeInfo((int)CalculationFunctions.AngleFromDir(targetDirection), 0.1f, 0.5f));
                        }
                    }
                }
            }
            if (gazeTarget != null)
            {
                if (!isGazeApplied)
                {
                    isGazeApplied = true;
                    StartCoroutine(CoroutineGaze());
                }
            }
        }
        private IEnumerator CoroutineGaze()
        {
            int degreeToRotate = (int)(gazeTarget.degree - detectionSightController.curDegree);
            degreeToRotate += 360 * 3;
            degreeToRotate %= 360;
            bool isClockwise;
            degreeToRotate = (isClockwise = degreeToRotate >= 180) ? 360 - degreeToRotate : degreeToRotate;
            // 해당 방향으로 회전
            while (degreeToRotate != 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                detectionSightController.AddRotationDegree(isClockwise ? -1 : 1);
                degreeToRotate -= 1;
            }
            // 응시
            while (gazeTarget.secWait > 0)
            {
                gazeTarget.secWait -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            gazeTarget = null;
            isGazeApplied = false;
        }

        public void ExecuteAct(AIActInfo info)
        {
            if (info.moveInfo != null && info.moveInfo.spdMove != 0)
            {
                Move(info.moveInfo);
            }
            if (info.gazeInfo != null && info.gazeInfo.secWait != 0)
            {
                Gaze(info.gazeInfo);
            }
        }

        /// <summary>
        /// 디폴트 행동강령 실행
        /// </summary>
        public void InitDefaultConduction()
        {
            defaultConductionController.InitConduction();
        }

        /// <summary>
        /// 현재 행동강령 초기화
        /// </summary>
        public void ClearConduction()
        {
            curConductionType = AIConductionType.None;
            curStatus = 0;
        }

        public void Gaze(AIGazeInfo info)
        {
            gazeTarget = new AIGazeInfo(info);
        }

        public void Move(AIMoveInfo info)
        {
            if (moveTarget != null)
            {
                transform.GetComponent<Rigidbody2D>().AddForce(-targetDirection);
                targetDirection = Vector3.zero;
            }
            moveTarget = info;
        }

        public AIMoveInfo GetCurMoveTarget()
        {
            return moveTarget;
        }

        public AIConductionType GetCurConductionType()
        {
            return curConductionType;
        }

        public void SetCurConductionType(AIConductionType _type)
        {
            curConductionType = _type;
        }

        public int GetCurStatus()
        {
            return curStatus;
        }

        public void SetCurStatus(int _curStatus)
        {
            curStatus = _curStatus;
        }

        public DetectionPassiveController GetDetectionPassiveController()
        {
            return detectionPassiveController;
        }

        public DetectionSightController GetDetectionSightController()
        {
            return detectionSightController;
        }

        public void PauseOrResumeAct(bool isPause)
        {
            if (isPause)
            {
                curStatus = -1;
                // 모든 코루틴 종료
                Debug.Log("일시 정지");
                StopAllCoroutines();
            }
            else
            {
                curStatus = 2;
            }
        }

        public bool IsExecutable()
        {
            return moveTarget == null && gazeTarget == null;
        }
    }
}
