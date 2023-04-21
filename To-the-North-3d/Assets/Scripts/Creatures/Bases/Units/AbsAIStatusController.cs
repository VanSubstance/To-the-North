
using UnityEngine;
namespace Assets.Scripts.Creatures.Bases
{
    public abstract class AbsAIStatusController : MonoBehaviour
    {
        protected AIBaseController baseCtrl;
        public AIBaseController BaseCtrl
        {
            set
            {
                baseCtrl = value;
            }
        }

        /// <summary>
        /// 위치를 적발하였을 때 해당 적발 위치를 행동 컨트롤러에 전달하는 함수
        /// 절대 좌표 기준
        /// </summary>
        /// <param name="detectPos">적발한 위치</param>
        public abstract void DetectPosition(Vector3 detectPos);

        /// <summary>
        /// 유저를 적발하였을 때 Transform을 컨트롤러에 전달하는 함수
        /// </summary>
        /// <param name="userTf"></param>
        public abstract void DetectUser(Transform userTf);
    }
}
