using System.Collections;
using Assets.Scripts.Components.Conversations.Managers;
using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Events.Abstracts;
using UnityEngine;

namespace Assets.Scripts.Events.Controllers
{
    internal class EventNpcController : AbsEventBaseController
    {
        private AIBaseController baseCtrl;

        private string BasePath
        {
            get
            {
                return baseCtrl.Info.NpcPath;
            }
        }
        private new void Awake()
        {
            base.Awake();
            baseCtrl = GetComponent<AIBaseController>();
        }

        /// <summary>
        /// NPC 상호작용 함수
        /// </summary>
        public override void OnInteraction()
        {
            StartCoroutine(StartConversation());
        }

        private IEnumerator StartConversation()
        {
            ConversationManager.SetBasePath(BasePath);
            ConversationManager.StartConversation($"Common");
            while (ConversationManager.isInConversation)
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
            baseCtrl.IsPause = false;
        }
    }
}
