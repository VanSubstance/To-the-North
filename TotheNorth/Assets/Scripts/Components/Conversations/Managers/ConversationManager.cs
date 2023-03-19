using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Conversations.Managers
{
    internal class ConversationManager : MonoBehaviour
    {
        [SerializeField]
        private Transform conversationPrefab;
        private static Transform convTf;
        public static bool isInConversation = false;

        private void Update()
        {
            if (!GlobalStatus.Loading.System.ConversationManager)
            {
                try
                {
                    convTf = Instantiate(conversationPrefab, GameObject.Find("UI").transform);
                    convTf.localPosition = Vector3.zero;
                    convTf.localScale = Vector3.one;
                    convTf.gameObject.SetActive(false);
                    GlobalStatus.Loading.System.ConversationManager = true;
                }
                catch (NullReferenceException)
                {
                    // UI 오브젝트 생성 전
                }
            }
        }

        public static void StartConversation()
        {
            isInConversation = true;
            convTf.gameObject.SetActive(true);
        }

        public static void FinishConversation()
        {
            isInConversation = false;
            convTf.gameObject.SetActive(false);
        }

        public static bool IsInConversation()
        {
            return isInConversation;
        }
    }
}
