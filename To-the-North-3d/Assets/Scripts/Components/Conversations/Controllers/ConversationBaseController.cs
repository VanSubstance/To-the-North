using Assets.Scripts.Components.Buttons.Controllers;
using Assets.Scripts.Components.Conversations.Managers;
using Assets.Scripts.Components.Conversations.Objects;
using Assets.Scripts.Components.Windows.Inventory;
using Assets.Scripts.Items;
using UnityEngine;
using System.Linq;

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
            ConvChoiceInfo[] possibleChoices = info.choices.Where((_choice) =>
            {
                foreach (ConvChoiceInfo.ChoiceCondition cond in _choice.conditions)
                {
                    switch (cond.contentType)
                    {
                        case ConvChoiceInfo.ChoiceCondition.ContentType.Item:
                            switch (cond.conditionType)
                            {
                                case ConvChoiceInfo.ChoiceCondition.ConditionType.Have:
                                    // 아이템을 가지고 있어야 함
                                    if (!InGameStatus.Item.LookForItemByCode(cond.code))
                                    {
                                        // 아이템이 없음
                                        return false;
                                    }
                                    break;
                            }
                            break;
                        case ConvChoiceInfo.ChoiceCondition.ContentType.Quest:
                            break;
                    }
                }
                return true;
            }).ToArray();
            curActiveChoiceCnt = possibleChoices.Length;
            for (int i = 0; i < curActiveChoiceCnt; i++)
            {
                ConvChoiceInfo temp = possibleChoices[i];
                choiceBtnList[i].SetText(
                    temp.text
                    );
                choiceBtnList[i].SetButtonAction(() =>
                {
                    foreach(ConvChoiceInfo.ChoiceCondition cond in temp.conditions)
                    {
                        switch (cond.contentType)
                        {
                            case ConvChoiceInfo.ChoiceCondition.ContentType.Item:
                                switch (cond.conditionType)
                                {
                                    case ConvChoiceInfo.ChoiceCondition.ConditionType.Get:
                                        // 아이템을 획득해야 함 <- 선택지 고르면 아이템 수령
                                        InGameStatus.Item.PushItemToInventory(Instantiate(DataFunction.LoadItemInfoByCode(cond.code)));
                                        break;
                                    case ConvChoiceInfo.ChoiceCondition.ConditionType.Pay:
                                        // 아이템을 제출해야 함 <- 선택지 고르면 아이템 소실
                                        InGameStatus.Item.PullItemFromInventoryByCode(cond.code);
                                        break;
                                }
                                break;
                            case ConvChoiceInfo.ChoiceCondition.ContentType.Quest:
                                switch (cond.conditionType)
                                {
                                    case ConvChoiceInfo.ChoiceCondition.ConditionType.Get:
                                        // 퀘스트를 시작한다는 개념 <- 선택지 고르면 퀘스트 수령
                                        Debug.Log($"Quest Start ! {cond.code}");
                                        break;
                                    case ConvChoiceInfo.ChoiceCondition.ConditionType.Pay:
                                        // 퀘스트를 완료했다는 개념 <- 선택지 고르면 퀘스트 사라짐
                                        Debug.Log($"Quest Clear ! {cond.code}");
                                        break;
                                }
                                break;
                        }
                    }
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
                    OpenCommerce();
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

        public void OpenCommerce()
        {
            foreach (ItemBaseInfo _info in DataFunction.LoadItemList($"{basePath}/Commerce"))
            {
                WindowInventoryController.Instance.GenerateItemObjectWithAuto(ContentType.Commerce, Instantiate(_info));
            }
            WindowInventoryController.Instance.OpenCommerce();
        }

        public void FinishConversation()
        {
            ClearConversation();
            curConvTrack = null;
            gameObject.SetActive(false);
        }
    }
}
