using System;
using System.Collections;
using Assets.Scripts.Creatures.Abstracts;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Creatures.Objects;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    internal class AIHoboController : AAIBaseController
    {

        private IEnumerator CoroutineMove(AIMoveInfo info)
        {
            curStatus = 3;
            curTargetMoveInfo = info;
            Debug.Log("이동 시작");
            curTargetVector = new Vector3(info.point().x, info.point().y, 0f) - transform.localPosition;
            while (Vector2.Distance(transform.localPosition, info.point()) > 0.2f)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                Debug.Log("이동중 ...");
                transform.Translate(curTargetVector * Time.deltaTime * info.spdMove);
            }
            Debug.Log("이동 종료");
            curTargetMoveInfo = null;
            curStatus = 2;
        }

        private IEnumerator CoroutineGaze(AIGazeInfo info)
        {
            curStatus = 3;
            yield return new WaitForSeconds(2f);
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
