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

        public string basePath;

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
            curActiveChoiceCnt = info.choices.Count;
            for (int i = 0; i < curActiveChoiceCnt; i++)
            {
                ConvChoiceInfo temp = info.choices[i];
                choiceBtnList[i].SetText(
                    temp.text
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

        private void GoToConversation(string _next)
        {
            int t;
            if (int.TryParse(_next, out t))
            {
                // 해당 번호로 이동
                ClearConversation();
                InitConversation(curConvTrack[t]);
                return;
            }
            switch (_next)
            {
                case "Commerce":
                    // 상점으로 이동
                    StartConversation("Commerce");
                    break;
                case "Quit":
                    ConversationManager.FinishConversation();
                    return;
                default:
                    // 새로운 책으로 이동
                    StartConversation(_next);
                    break;
            }
        }

        public void SetBasePath(string _basePath)
        {
            basePath = _basePath;
        }

        public void StartConversation(string _path)
        {
            curConvTrack = DataFunction.LoadConversation($"{basePath}/{_path}");
            GoToConversation("0");
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
