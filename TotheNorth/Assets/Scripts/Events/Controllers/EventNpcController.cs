using System.Collections;
using Assets.Scripts.Components.Conversations.Managers;
using Assets.Scripts.Creatures.Abstracts;
using Assets.Scripts.Events.Abstracts;
using UnityEngine;

namespace Assets.Scripts.Events.Controllers
{
    internal class EventNpcController : AEventBaseController
    {
        private AAIBaseController baseController;
        private void Awake()
        {
            baseController = GetComponent<AAIBaseController>();
        }

        /// <summary>
        /// NPC 상호작용 함수
        /// </summary>
        public override void OnInteraction()
        {
            baseController.PauseOrResumeAct(true);
            StartCoroutine(StartConversation());
        }

        public IEnumerator StartConversation()
        {
            ConversationManager.StartConversation();
            while (ConversationManager.IsInConversation())
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
            baseController.PauseOrResumeAct(false);
        }
    }
}
