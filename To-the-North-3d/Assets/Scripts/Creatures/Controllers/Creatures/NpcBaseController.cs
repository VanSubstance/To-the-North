using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Creatures
{
    internal class NpcBaseController : AIBaseController
    {
        /// <summary>
        /// 유저를 식별하였을 때 작동하는 함수
        /// NPC는 유저가 시야를 벗어나도 마저 쳐다본다
        /// </summary>
        /// <param name="targetTf">유저 Transform</param>
        public override void OnDetectUser(Transform targetTf)
        {
            Debug.Log("User Detect!!");
        }
    }
}
