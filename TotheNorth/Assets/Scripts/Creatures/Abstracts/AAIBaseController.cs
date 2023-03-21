using System.Collections;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Creatures.Objects;
using Assets.Scripts.Creatures.Controllers;
using Assets.Scripts.Users.Controllers;
using UnityEngine;

namespace Assets.Scripts.Creatures.Abstracts
{
    internal class AAIBaseController : MonoBehaviour, IAIAct
    {
        [SerializeField]
        private DetectionPassiveController detectionPassiveController;
        [SerializeField]
        private DetectionSightController detectionSightController;

        private AAIConductionBaseController conductionController;
        public AIConductionType curConductionType;
        protected int pausePhase = 0;
        private Vector3 targetDirection;
        private AIMoveInfo moveTarget = null;
        private AIGazeInfo gazeTarget = null;
        private bool isMoveApplied = false, isGazeApplied = false;
        private void Awake()
        {
            curConductionType = AIConductionType.Petrol;
        }

        private void Update()
        {
            switch (pausePhase)
            {
                case 0:
                    // 행동강령 자유 상태
                    InitConduction();
                    break;
                case -1:
                    // 행동 일시 정지
                    break;
                case -2:
                    // 범프 저장 완료
                    break;
                case 1:
                    // 초기화 시작
                    break;
                case 2:
                    // 행동 명령 입력 가능 상태
                    break;
            }
            if (moveTarget != null)
            {
                // 이동
                if (Vector2.Distance(transform.localPosition, moveTarget.point()) <= 0.1f)
                {
                    //  이돌 종료
                    ClearMove();
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
        /// 디폴트 행동강령 실행: 패트롤
        /// </summary>
        public void InitConduction()
        {
            pausePhase = 1;
            switch (curConductionType)
            {
                case AIConductionType.None:
                    break;
                case AIConductionType.Petrol:
                    conductionController = GetComponent<AIPetrolController>();
                    break;
                default:
                    break;
            }
            conductionController.InitConduction();
            pausePhase = 2;
        }

        /// <summary>
        /// 현재 행동강령 초기화
        /// </summary>
        public void ClearConduction()
        {
            curConductionType = AIConductionType.None;
            pausePhase = 0;
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

        private void ClearMove()
        {
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            targetDirection = Vector3.zero;
            moveTarget = null;
            isMoveApplied = false;
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

        public int GetPausePhase()
        {
            return pausePhase;
        }

        public void SetPausePhase(int _pausePhase)
        {
            pausePhase = _pausePhase;
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
                pausePhase = -1;
                ClearMove();
                StopAllCoroutines();
            }
            else
            {
                gazeTarget = null;
                isGazeApplied = false;
                moveTarget = null;
                isMoveApplied = false;
                pausePhase = 2;
            }
        }

        public bool IsExecutable()
        {
            return
                moveTarget == null &&
                gazeTarget == null &&
                pausePhase == 2 &&
                true;
        }
    }
}
