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
                switch (aiBase.GetCurStatus())
                {
                    case 0:
                        // 대기 상태
                        break;
                    case 1:
                        // 행동 강령 초기화중
                        break;
                    case 2:
                        // 행동강령 대기상태
                        // = 다음 행동 실행
                        ActNext();
                        break;
                    case 3:
                        // 단순 행동 실행중 (방해 X)
                        break;
                    case -1:
                        // 일시정지 명령 진입
                        // 직전 명령이 있다면 되감기
                        RewindPrevAct();
                        aiBase.SetCurStatus(-2);
                        break;
                    case -2:
                        // 완전 일시 정지 상태
                        break;
                }
            }
        }

        /// <summary>
        /// 패트롤 초기화
        /// </summary>
        public void InitConduction()
        {
            aiBase.SetCurStatus(1); ;
            aiBase.SetCurConductionType(conductionType);
            OnInitConduction();
            aiBase.SetCurStatus(2);
        }
        public abstract void ActNext();
        /// <summary>
        /// 직전 명령 되감기
        /// </summary>
        public abstract void RewindPrevAct();

        /// <summary>
        /// 해당 행동강령이 초기화될 때 진행되어야 할 함수
        /// </summary>
        public abstract void OnInitConduction();

        public AAIBaseController GetAIBaseController()
        {
            return aiBase;
        }
    }
}
