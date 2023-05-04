using Assets.Scripts.Events.Abstracts;
using UnityEngine;

namespace Assets.Scripts.Events.Controllers
{
    class EventRootingController : AbsEventBaseController
    {
        /// <summary>
        /// 아이템 어레이?
        /// 자료형이 뭐가 될지 나는 모름 @kang123077
        /// </summary>
        private object[] items;

        /// <summary>
        /// SpaceBar 눌렀을 때 작동하는 함수
        /// </summary>
        public override void OnInteraction()
        {
            Debug.Log("루팅 이벤트 발생!");
        }
    }
}
