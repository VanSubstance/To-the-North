using Assets.Scripts.Creatures.Bases;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Creatures
{
    internal class MonsterBaseController : AIBaseController
    {
        /// <summary>
        /// 유저를 식별하였을 때 작동하는 함수
        /// </summary>
        /// <param name="targetTf">Null이 아니다 = 유저가 눈에 보인다, Null이다 = 유저가 안보인다</param>
        public override void OnDetectUser(Transform targetTf)
        {
            Debug.Log("User Detect!! ");
        }
    }
}
