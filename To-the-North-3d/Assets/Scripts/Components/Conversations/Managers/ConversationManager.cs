using System;
using Assets.Scripts.Components.Conversations.Controllers;
using Assets.Scripts.Components.Conversations.Objects;
using UnityEngine;

namespace Assets.Scripts.Components.Conversations.Managers
{
    internal class ConversationManager : MonoBehaviour
    {
        [SerializeField]
        private Transform conversationPrefab;
        private static ConversationBaseController baseController;
        public static bool isInConversation = false;

        private void Update()
        {
            if (!GlobalStatus.Loading.System.ConversationManager)
            {
                try
                {
                    Transform tem = Instantiate(conversationPrefab, GameObject.Find("UI").transform);
                    tem.localPosition = Vector3.zero;
                    tem.localScale = Vector3.one;
                    tem.gameObject.SetActive(false);
                    baseController = tem.GetComponent<ConversationBaseController>();
                    GlobalStatus.Loading.System.ConversationManager = true;
                }
                catch (NullReferenceException)
                {
                    // UI 오브젝트 생성 전
                }
            }
        }

        public static void StartConversation(ConvInfo[] info)
        {
            isInConversation = true;
            InGameStatus.User.isPause = true;
            baseController.StartConversation(info);
        }

        public static void FinishConversation()
        {
            isInConversation = false;
            InGameStatus.User.isPause = false;
            baseController.FinishConversation();
        }
    }
}
