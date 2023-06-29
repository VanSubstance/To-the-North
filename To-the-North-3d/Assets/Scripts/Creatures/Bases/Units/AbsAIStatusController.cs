
using Assets.Scripts.Creatures.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    public abstract class AbsAIStatusController : MonoBehaviour
    {
        [SerializeField]
        private float timeMaxMemory;
        private float timeLiveMemory;
        private void Awake()
        {
            timeLiveMemory = 0;
        }

        private void Update()
        {
            if (timeMaxMemory <= timeLiveMemory)
            {
                timeLiveMemory = 0;
                return;
            }
            OnDetectionUpdate();
            timeLiveMemory += Time.deltaTime;
        }

        protected AIBaseController baseCtrl;
        public AIBaseController BaseCtrl
        {
            set
            {
                baseCtrl = value;
            }
        }

        protected void ResetTimer()
        {
            timeLiveMemory = 0;
        }
        
        protected bool IsDuringAction
        {
            get
            {
                return timeLiveMemory != 0;
            }
        }

        public void DetectPosition(Vector3 detectPos)
        {
            if (IsDuringAction) ResetTimer();
            OnDetectPosition(detectPos);
        }

        public void DetectUser(Transform userTf)
        {
            if (IsDuringAction) ResetTimer();
            OnDetectUser(userTf);
        }

        protected abstract AIStatusType AIType();

        /// <summary>
        /// 위치를 적발하였을 때 해당 적발 위치를 행동 컨트롤러에 전달하는 함수;
        /// 절대 좌표 기준
        /// </summary>
        /// <param name="detectPos">적발한 위치 (절대 좌표)</param>
        protected abstract void OnDetectPosition(Vector3 detectPos);

        /// <summary>
        /// 유저를 적발하였을 때 Transform을 컨트롤러에 전달하는 함수
        /// </summary>
        /// <param name="userTf"></param>
        protected abstract void OnDetectUser(Transform userTf);

        /// <summary>
        /// 해당 행동 중 프레임마다 실행할 함수
        /// </summary>
        protected abstract void OnDetectionUpdate();
    }
}
