using System.Collections;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Abstracts;
using Assets.Scripts.Creatures.Objects;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    internal class AIHoboController : AAIBaseController
    {
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
            degreeToRotate += 360;
            degreeToRotate %= 360;
            bool isClockwise;
            degreeToRotate = (isClockwise = degreeToRotate < 180) ? degreeToRotate : 360 - degreeToRotate;
            // 해당 방향으로 회전
            while (degreeToRotate != 0 && degreeToRotate != 360 && degreeToRotate != -360)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                GetDetectionSightController().AddRotationDegree(isClockwise ? 1 : -1);
                degreeToRotate += isClockwise ? -1 : 1;
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

        public override void Gaze(AIGazeInfo info)
        {
            StartCoroutine(CoroutineGaze(info));
        }

        public override void Move(AIMoveInfo info)
        {
            StartCoroutine(CoroutineMove(info));
        }
    }
}
