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
        protected AAIBaseController aiBase;
        protected AIConductionType conductionType;

        protected void Awake()
        {
            aiBase = GetComponent<AAIBaseController>();
        }

        private void Update()
        {
            // 패트롤 상태인지
            if (aiBase.curConductionType == conductionType)
            {
                switch (aiBase.curStatus)
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
                }
            }
        }

        /// <summary>
        /// 패트롤 초기화
        /// </summary>
        public void InitConduction()
        {
            aiBase.curStatus = 1;
            aiBase.curConductionType = conductionType;
            OnInitConduction();
            aiBase.curStatus = 2;
        }
        public abstract void ActNext();

        /// <summary>
        /// 해당 행동강령이 초기화될 때 진행되어야 할 함수
        /// </summary>
        public abstract void OnInitConduction();
    }
}
