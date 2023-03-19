using System.Collections.Generic;
using Assets.Scripts.Creatures.Abstracts;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Creatures.Objects;
using Assets.Scripts.Users.Controllers;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    internal class AIPetrolController : AAIConductionBaseController
    {
        [SerializeField]
        private AIActInfo[] actTrack;

        private Queue<AIActInfo> liveActTrack = new Queue<AIActInfo>();
        private bool isNavForward = false;
        private AIMoveInfo prevMove = null;
        private AIActInfo curActInfo, bumpForPause;

        private new void Awake()
        {
            base.Awake();
            conductionType = AIConductionType.Petrol;
        }

        public override void ActNext()
        {
            if (bumpForPause != null)
            {
                aiBase.ExecuteAct(bumpForPause);
                bumpForPause = null;
                return;
            }
            // 다음 단순 행동 유무 체크
            if (liveActTrack.TryDequeue(out curActInfo))
            {
                // 다음 단순 행동이 남아있음
                // 최초 단순 행동은 이동이어야 한다.
                if (prevMove != null || curActInfo.type == AIActType.Move)
                {
                    prevMove = curActInfo.GetMoveInfo();
                    aiBase.ExecuteAct(curActInfo);
                }
                return;
            }
            // 단순 행동 전부 종료됨
            // = 패트롤 종료
            prevMove = null;
            aiBase.ClearConduction();
        }

        public override void OnInitConduction()
        {
            liveActTrack.Clear();
            isNavForward = !isNavForward;
            if (isNavForward)
            {
                for (int i = 0; i < actTrack.Length; i++)
                {
                    liveActTrack.Enqueue(actTrack[i]);
                }
            }
            else
            {
                for (int i = actTrack.Length - 1; i >= 0; i--)
                {
                    liveActTrack.Enqueue(actTrack[i]);
                }
            }
        }

        public AIActInfo[] GetActTrack()
        {
            return actTrack;
        }

        public override void SaveBumpForPause()
        {
            bumpForPause = new AIActInfo(curActInfo);
        }
    }
}
