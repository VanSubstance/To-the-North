using Assets.Scripts.Components.Buttons.Controllers;
using Assets.Scripts.Components.Conversations.Managers;
using Assets.Scripts.Components.Conversations.Objects;
using UnityEngine;

namespace Assets.Scripts.Components.Conversations.Controllers
{
    internal class ConversationBaseController : MonoBehaviour
    {
        [SerializeField]
        private Transform choicesTf, choicePrefab;
        [SerializeField]
        private TMPro.TextMeshProUGUI descUgui;
        [SerializeField]
        private int maxChoiceCnt = 5;

        private static ButtonTextController[] choiceBtnList;
        private ConvInfo[] curConvTrack;
        private int curActiveChoiceCnt = 0;

        private void Awake()
        {
            choiceBtnList = new ButtonTextController[maxChoiceCnt];
            for (int i = 0; i < maxChoiceCnt; i++)
            {
                Transform tem = Instantiate(choicePrefab, choicesTf);
                choiceBtnList[i] = tem.GetComponent<ButtonTextController>();
                choiceBtnList[i].SetActice(false);
            }
        }

        private void InitConversation(ConvInfo info)
        {
            descUgui.text = info.desc;
            if (info.choices == null)
            {
                choiceBtnList[0].SetText("[대화 종료]");
                choiceBtnList[0].SetButtonAction(() =>
                {
                    GoToConversation(-1);
                });
                choiceBtnList[0].SetActice(true);
                return;
            }
            curActiveChoiceCnt = info.choices.Length;
            for (int i = 0; i < curActiveChoiceCnt; i++)
            {
                ConvChoiceInfo temp = info.choices[i];
                choiceBtnList[i].SetText(
                    temp.text + (temp.next == -1 ? " [대화 종료] " : string.Empty)
                    );
                choiceBtnList[i].SetButtonAction(() =>
                {
                    GoToConversation(temp.next);
                });
                choiceBtnList[i].SetActice(true);
            }
        }

        private void ClearConversation()
        {
            descUgui.text = string.Empty;
            for (int i = 0; i < curActiveChoiceCnt; i++)
            {
                choiceBtnList[i].Clear();
                choiceBtnList[i].SetActice(false);
            }
        }

        private void GoToConversation(int idx)
        {
            if (idx < 0)
            {
                ConversationManager.FinishConversation();
                return;
            }
            ClearConversation();
            InitConversation(curConvTrack[idx]);
        }

        public void StartConversation(ConvInfo[] info)
        {
            curConvTrack = info;
            GoToConversation(0);
            gameObject.SetActive(true);
        }

        public void FinishConversation()
        {
            ClearConversation();
            curConvTrack = null;
            gameObject.SetActive(false);
        }
    }
}
