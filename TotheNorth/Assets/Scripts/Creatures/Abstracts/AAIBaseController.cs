using System.Collections;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Creatures.Objects;
using Assets.Scripts.Users.Controllers;
using UnityEngine;

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
        protected Vector3 curTargetDir, curTargetPoint;

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
        }

        public void ExecuteAct(AIActInfo info)
        {
            switch (info.type)
            {
                case AIActType.Move:
                    Move(info.GetMoveInfo());
                    break;
                case AIActType.Gaze:
                    Gaze(info.GetGazeInfo());
                    break;
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
        private IEnumerator CoroutineMove(AIMoveInfo info)
        {
            curStatus = 3;
            curTargetPoint = new Vector3(info.point().x, info.point().y, info.spdMove);
            //StartCoroutine(CoroutineGaze(
            //    new AIGazeInfo(
            //        (int)CalculationFunctions.AngleFromDir(info.point() - new Vector2(transform.localPosition.x, transform.localPosition.y)),
            //        0,
            //        0
            //        ),
            //    true
            //    ));
            while (Vector2.Distance(transform.localPosition, info.point()) > 0.2f)
            {
                curTargetDir = new Vector3(info.point().x, info.point().y, 0f) - transform.localPosition;
                curTargetDir.z = info.spdMove;
                GetDetectionSightController().SetRotationDegree((int)CalculationFunctions.AngleFromDir(info.point() - new Vector2(transform.localPosition.x, transform.localPosition.y)));
                yield return new WaitForSeconds(Time.deltaTime);
                transform.Translate(new Vector2(curTargetDir.x, curTargetDir.y).normalized * Time.deltaTime * curTargetDir.z);
            }
            curTargetPoint = Vector3.zero;
            curStatus = 2;
        }

        private IEnumerator CoroutineGaze(AIGazeInfo info, bool isAsync = false)
        {
            if (!isAsync)
                curStatus = 3;
            // 이동해야 할 각도
            int degreeToRotate = (int)(info.degree - GetDetectionSightController().curDegree);
            degreeToRotate += 360 * 3;
            degreeToRotate %= 360;
            bool isClockwise;
            degreeToRotate = (isClockwise = degreeToRotate >= 180) ? 360 - degreeToRotate : degreeToRotate;
            // 해당 방향으로 회전
            while (degreeToRotate != 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                GetDetectionSightController().AddRotationDegree(isClockwise ? -1 : 1);
                degreeToRotate -= 1;
            }
            // 응시
            while (info.secWait > 0)
            {
                info.secWait -= Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            // 종료
            if (!isAsync)
                curStatus = 2;
        }

        public void Gaze(AIGazeInfo info)
        {
            StartCoroutine(CoroutineGaze(info));
        }

        public void Move(AIMoveInfo info)
        {
            StartCoroutine(CoroutineMove(info));
        }

        public Vector3 GetCurTargetPoint()
        {
            return curTargetPoint;
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
                StopAllCoroutines();
            }
            else
            {
                curStatus = 2;
            }
        }
    }
}
