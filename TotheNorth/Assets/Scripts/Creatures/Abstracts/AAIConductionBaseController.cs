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
    /// <summary>
    /// 행동강령용 추상 컨트롤러
    /// </summary>
    internal abstract class AAIConductionBaseController : MonoBehaviour, IAIConduction
    {
        [SerializeField]
        public AAIBaseController aiBase;
        protected AIConductionType conductionType;

        protected void Awake()
        {
            if (aiBase == null)
                aiBase = GetComponent<AAIBaseController>();
        }

        private void Update()
        {
            // 패트롤 상태인지
            if (aiBase.GetCurConductionType() == conductionType)
            {
                if (aiBase.IsExecutable())
                {
                    ActNext();
                    return;
                }
                if (aiBase.GetPausePhase() == -1)
                {
                    SaveBumpForPause();
                    aiBase.SetPausePhase(-2);
                    return;
                }
            }
        }

        /// <summary>
        /// 패트롤 초기화
        /// </summary>
        public void InitConduction()
        {
            OnInitConduction();
        }
        public abstract void ActNext();
        /// <summary>
        /// 직전 명령 되감기
        /// </summary>
        public abstract void SaveBumpForPause();

        /// <summary>
        /// 해당 행동강령이 초기화될 때 진행되어야 할 함수
        /// </summary>
        public abstract void OnInitConduction();

        public AAIBaseController GetAIBaseController()
        {
            return aiBase;
        }

        public AIConductionType GetConductionType()
        {
            return conductionType;
        }
    }
}
