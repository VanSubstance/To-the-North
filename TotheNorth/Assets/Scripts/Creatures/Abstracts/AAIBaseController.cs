using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Creatures.Objects;
using Assets.Scripts.Users.Controllers;
using UnityEngine;

namespace Assets.Scripts.Creatures.Abstracts
{
    internal abstract class AAIBaseController : MonoBehaviour, IAIAct
    {
        [SerializeField]
        private AAIConductionBaseController defaultConductionController;
        [SerializeField]
        private DetectionPassiveController detectionPassiveController;
        [SerializeField]
        private DetectionSightController detectionSightController;

        protected AIConductionType curConductionType;
        protected int curStatus = 0;
        protected Vector3 curTargetDir, curTargetPoint;

        private void Update()
        {
            switch (curStatus)
            {
                case 0:
                    // 행동강령 자유 상태
                    InitDefaultConduction();
                    break;
            }
        }

        public void ExecuteAct(AIActInfo info)
        {
            switch (info.type)
            {
                case AIActType.Move:
                    Move(info.GetMoveInfo());
                    break;
                case AIActType.Gaze:
                    Gaze(info.GetGazeInfo());
                    break;
            }
        }

        /// <summary>
        /// 디폴트 행동강령 실행
        /// </summary>
        public void InitDefaultConduction()
        {
            defaultConductionController.InitConduction();
        }

        /// <summary>
        /// 현재 행동강령 초기화
        /// </summary>
        public void ClearConduction()
        {
            curConductionType = AIConductionType.None;
            curStatus = 0;
        }

        public abstract void Gaze(AIGazeInfo info);

        public abstract void Move(AIMoveInfo info);

        public Vector3 GetCurTargetPoint()
        {
            return curTargetPoint;
        }

        public AIConductionType GetCurConductionType()
        {
            return curConductionType;
        }

        public void SetCurConductionType(AIConductionType _type)
        {
            curConductionType = _type;
        }

        public int GetCurStatus()
        {
            return curStatus;
        }

        public void SetCurStatus(int _curStatus)
        {
            curStatus = _curStatus;
        }

        public DetectionPassiveController GetDetectionPassiveController()
        {
            return detectionPassiveController;
        }

        public DetectionSightController GetDetectionSightController()
        {
            return detectionSightController;
        }
    }
}
