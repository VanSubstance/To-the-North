using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Components.Conversations.Managers;
using Assets.Scripts.Components.Conversations.Objects;
using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Events.Abstracts;
using UnityEngine;

namespace Assets.Scripts.Events.Controllers
{
    internal class EventNpcController : AEventBaseController
    {
        private AIBaseController baseController;
        private List<ConvInfo> convInfos;
        private void Awake()
        {
            baseController = GetComponent<AIBaseController>();
            convInfos = new List<ConvInfo>() {
                    new ConvInfo(
                        "무슨일인가 애송이?",
                        new ConvChoiceInfo[] {
                            new ConvChoiceInfo("그냥 말 걸어봤어. 다리를 걸어버릴수는 없잖아?", 2),
                            new ConvChoiceInfo("무슨 일이 있어야만 말을 거나? 무슨 일 만들어줘?", 3),
                            new ConvChoiceInfo("그냥 말 걸어봤습니다.", 1),
                        }
                    ),
                    new ConvInfo(
                        "허허, 그렇군."
                    ),
                    new ConvInfo(
                        "허허, 싸가지가 바가지군 그래.\n미쳤으면 조용히 갈길 가게나."
                    ),
                    new ConvInfo(
                        "허허, 미1친새낀가? 그냥 안부를 물었을 뿐이지 않는가?\n 왜이리 무섭게 말하는가?,,",
                        new ConvChoiceInfo[] {
                            new ConvChoiceInfo("무시하고 갈길 간다.", -1),
                            new ConvChoiceInfo("아니 시1발련아 애송이라고 선시비 연건 넌데? 말 돌리지 말고, 무슨 일 만들어주냐니까?", 2)
                        }
                    ),
            };
        }

        /// <summary>
        /// NPC 상호작용 함수
        /// </summary>
        public override void OnInteraction()
        {
            baseController.IsPause = true;
            StartCoroutine(StartConversation());
        }

        private IEnumerator StartConversation()
        {
            ConversationManager.StartConversation(convInfos.ToArray());
            while (ConversationManager.isInConversation)
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
            baseController.IsPause = false;
        }
    }
}
