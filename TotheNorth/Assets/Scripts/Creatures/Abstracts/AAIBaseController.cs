using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Creatures.Controllers;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Creatures.Objects;
using UnityEngine;

namespace Assets.Scripts.Creatures.Abstracts
{
    internal abstract class AAIBaseController : MonoBehaviour, IAIAct
    {
        public AIConductionType curConductionType;
        public int curStatus = 0;
        public AIMoveInfo curTargetMoveInfo;
        public Vector3 curTargetVector;
        public AAIConductionBaseController defaultConduction;

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

        public void ExecuteAct<T>(T info)
        {
            if (typeof(T) == typeof(AIMoveInfo))
            {
                Move((AIMoveInfo)(object)info);
                return;
            }
            if (typeof(T) == typeof(AIGazeInfo))
            {
                Gaze((AIGazeInfo)(object)info);
                return;
            }
        }

        /// <summary>
        /// 디폴트 행동강령 실행
        /// </summary>
        public void InitDefaultConduction()
        {
            defaultConduction.InitConduction();
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
    }
}
