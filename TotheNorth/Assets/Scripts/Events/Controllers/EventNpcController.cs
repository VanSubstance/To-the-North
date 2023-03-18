using System.Collections;
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
            Debug.Log("Npc 상호작용:: 이벤트 처리");
            baseController.PauseOrResumeAct(true);
            StartCoroutine(timer(3f));
        }

        public IEnumerator timer(float t)
        {
            yield return new WaitForSeconds(t);
            Debug.Log("재개");
            baseController.PauseOrResumeAct(false);
        }
    }
}
