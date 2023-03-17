using System;
using System.Collections;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Creatures.Objects;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    internal class AIBaseController : MonoBehaviour, IAIAct
    {
        public AIConductionType curConductionType;
        public int curStatus = 0;
        public AIMoveInfo curTargetMoveInfo;
        private Vector3 curTargetVector;

        private void Update()
        {
            switch (curStatus)
            {
                case 0:
                    // 행동강령 자유 상태
                    GetComponent<AIPetrolBaseController>().InitConduction();
                    break;
            }
        }
        public void Move(AIMoveInfo info)
        {
            StartCoroutine(CoroutineMove(info));
        }


        public void Gaze(AIGazeInfo info)
        {
            StartCoroutine(CoroutineGaze(info));
        }

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

        public void ClearConduction()
        {
            curConductionType = AIConductionType.None;
            curStatus = 0;
        }
    }
}
