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
        private Transform UITf, conversationPrefab;
        private static Transform convTf;
        public static bool isInConversation = false;

        private void Awake()
        {
            convTf = Instantiate(conversationPrefab, UITf);
            convTf.localPosition = Vector3.zero;
            convTf.localScale = Vector3.one;
            convTf.gameObject.SetActive(false);
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
