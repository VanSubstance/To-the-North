using System.Collections.Generic;
using Assets.Scripts.Creatures.Objects;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    internal class AIPetrolBaseController : MonoBehaviour
    {
        [SerializeField]
        private AIMoveInfo[] moveTrack;
        [SerializeField]
        private AIGazeInfo[] gazeTrack;

        private Queue<AIMoveInfo> liveMoveTrack = new Queue<AIMoveInfo>();
        private Queue<AIGazeInfo> liveGazeTrack = new Queue<AIGazeInfo>();
        private bool isNavForward = true;
        private AIBaseController aiBase;


        private void Awake()
        {
            aiBase = GetComponent<AIBaseController>();
        }

        private void Update()
        {
            // 패트롤 상태인지
            if (aiBase.curConductionType == AIConductionType.Petrol)
            {
                switch (aiBase.curStatus)
                {
                    case 0:
                        // 대기 상태
                        break;
                    case 1:
                        // 행동 강령 초기화중
                        break;
                    case 2:
                        // 행동강령 대기상태
                        // = 다음 행동 실행
                        ActNext();
                        break;
                    case 3:
                        // 단순 행동 실행중 (방해 X)
                        break;
                }
            }
        }

        /// <summary>
        /// 패트롤 초기화
        /// </summary>
        public void InitConduction()
        {
            aiBase.curStatus = 1;
            aiBase.curConductionType = AIConductionType.Petrol;
            isNavForward = Vector2.Distance(transform.localPosition, moveTrack[0].point()) <= Vector2.Distance(transform.localPosition, moveTrack[moveTrack.Length - 1].point());
            if (isNavForward)
            {
                // 정방향으로 큐 채우기
                for (int i = 0; i < moveTrack.Length; i++)
                {
                    liveMoveTrack.Enqueue(moveTrack[i]);
                }
                for (int i = 0; i < gazeTrack.Length; i++)
                {
                    liveGazeTrack.Enqueue(gazeTrack[i]);
                }
            }
            else
            {
                // 역방향으로 큐 채우기
                for (int i = moveTrack.Length - 1; i >= 0; i--)
                {
                    liveMoveTrack.Enqueue(moveTrack[i]);
                }
                for (int i = gazeTrack.Length - 1; i >= 0; i--)
                {
                    liveGazeTrack.Enqueue(gazeTrack[i]);
                }
            }
            aiBase.curStatus = 2;
        }

        /// <summary>
        /// 다음 행동 실행
        /// </summary>
        /// <returns></returns>
        private void ActNext()
        {
            // 이동 체크
            AIMoveInfo targetMoveInfo;
            if (liveMoveTrack.TryDequeue(out targetMoveInfo))
            {
                // 이동할 포인트가 남았음
                // 다음 이동 포인트로 이동
                aiBase.Move(targetMoveInfo);
                return;
            }

            // 응시 체크
            AIGazeInfo targetGazeInfo;
            if (liveGazeTrack.TryDequeue(out targetGazeInfo))
            {
                // 바라볼 응시 포인트가 남았음
                // 다음 응시
                aiBase.Gaze(targetGazeInfo);
                return;
            }

            // 이동, 응시 전부 종료됨
            // = 패트롤 종료
            aiBase.ClearConduction();
        }

        public AIMoveInfo[] GetMoveTrack()
        {
            return moveTrack;
        }

        public AIGazeInfo[] GetGazeTrack()
        {
            return gazeTrack;
        }
    }
}
