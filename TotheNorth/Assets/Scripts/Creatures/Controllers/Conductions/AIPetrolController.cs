using System.Collections.Generic;
using Assets.Scripts.Creatures.Abstracts;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Creatures.Objects;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    internal class AIPetrolController : AAIConductionBaseController
    {
        [SerializeField]
        private AIMoveInfo[] moveTrack;
        [SerializeField]
        private AIGazeInfo[] gazeTrack;

        private Queue<AIMoveInfo> liveMoveTrack = new Queue<AIMoveInfo>();
        private Queue<AIGazeInfo> liveGazeTrack = new Queue<AIGazeInfo>();
        private bool isNavForward = true;

        private void Awake()
        {
            base.Awake();
            conductionType = AIConductionType.Petrol;
        }

        public override void ActNext()
        {
            // 이동 체크
            AIMoveInfo targetMoveInfo;
            if (liveMoveTrack.TryDequeue(out targetMoveInfo))
            {
                // 이동할 포인트가 남았음
                // 다음 이동 포인트로 이동
                aiBase.ExecuteAct(targetMoveInfo);
                return;
            }

            // 응시 체크
            AIGazeInfo targetGazeInfo;
            if (liveGazeTrack.TryDequeue(out targetGazeInfo))
            {
                // 바라볼 응시 포인트가 남았음
                // 다음 응시
                aiBase.ExecuteAct(targetGazeInfo);
                return;
            }

            // 이동, 응시 전부 종료됨
            // = 패트롤 종료
            aiBase.ClearConduction();
        }

        public override void OnInitConduction()
        {
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
