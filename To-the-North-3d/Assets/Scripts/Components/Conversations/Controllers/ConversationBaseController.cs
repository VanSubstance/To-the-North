using Assets.Scripts.Components.Buttons.Controllers;
using Assets.Scripts.Components.Conversations.Managers;
using Assets.Scripts.Components.Conversations.Objects;
using Assets.Scripts.Components.Windows.Inventory;
using Assets.Scripts.Items;
using Assets.Scripts.Components.Infos;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Components.Windows;
using Assets.Scripts.Commons;
using System.Collections.Generic;

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
                                case "Have":
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
                            switch (cond.conditionType)
                            {
                                case "Have":
                                    // 퀘스트를 가지고 있어야 함
                                    // + 클리어 조건 전부 통과했어야 함
                                    try
                                    {
                                        if (!WindowQuestContainerController.Instance.questContentControllers[cond.code].IsClearable)
                                        {
                                            // 클리어 불가
                                            return false;
                                        }
                                    }
                                    catch (KeyNotFoundException)
                                    {
                                        return false;
                                    }
                                    break;
                                case "NotHave":
                                    // 퀘스트를 가지고 있지 않아야 함
                                    try
                                    {
                                        if (WindowQuestContainerController.Instance.questContentControllers[cond.code])
                                        {
                                            return false;
                                        }
                                    }
                                    catch (KeyNotFoundException)
                                    {
                                    }
                                    break;
                                case "NotDone":
                                    // 한 적이 없어야 함
                                    if (InGameStatus.Quest.Done.Contains(cond.code))
                                    {
                                        // 클리어 불가
                                        return false;
                                    }
                                    break;
                                case "Done":
                                    // 클리어 했어야 함
                                    if (!InGameStatus.Quest.Done.Contains(cond.code))
                                    {
                                        // 클리어 불가
                                        return false;
                                    }
                                    break;
                            }
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
                    foreach (ConvChoiceInfo.ChoiceCondition cond in temp.conditions)
                    {
                        switch (cond.contentType)
                        {
                            case ConvChoiceInfo.ChoiceCondition.ContentType.Item:
                                ItemBaseInfo ib;
                                switch (cond.conditionType)
                                {
                                    case "Get":
                                        // 아이템을 획득해야 함 <- 선택지 고르면 아이템 수령
                                        InGameStatus.Item.PushItemToInventory(Instantiate(ib = DataFunction.LoadItemInfoByCode(cond.code)));
                                        UIInfoTextContainerController.Instance.PrintText($"{GlobalText.System.ItemGet}: {ib.Title}");
                                        break;
                                    case "Pay":
                                        // 아이템을 제출해야 함 <- 선택지 고르면 아이템 소실
                                        ib = InGameStatus.Item.PullItemFromInventoryByCode(cond.code);
                                        UIInfoTextContainerController.Instance.PrintText($"{GlobalText.System.ItemGet}: {ib.Title}");
                                        break;
                                }
                                break;
                            case ConvChoiceInfo.ChoiceCondition.ContentType.Quest:
                                switch (cond.conditionType)
                                {
                                    case "Start":
                                        // 퀘스트를 시작한다는 개념 <- 선택지 고르면 퀘스트 수령
                                        UIInfoTextContainerController.Instance.PrintText($"{GlobalText.System.QuestGet}: {WindowQuestContainerController.Instance.ActivateQuest(cond.code)}");
                                        break;
                                    case "Clear":
                                        // 퀘스트를 완료했다는 개념 <- 선택지 고르면 퀘스트 사라짐
                                        UIInfoTextContainerController.Instance.PrintText($"{GlobalText.System.QuestClear}: {WindowQuestContainerController.Instance.ClearQuest(cond.code)}");
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
