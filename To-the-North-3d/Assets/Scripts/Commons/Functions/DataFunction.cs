using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using Assets.Scripts.Components.Hovers;
using Assets.Scripts.Users;

public static class DataFunction
{
    public static T LoadObjectFromJson<T>(string jsonPath)
    {
        try
        {
            string jsonData = File.ReadAllText(jsonPath + ".json");
            return JsonConvert.DeserializeObject<T>(jsonData);
        }
        catch (FileNotFoundException)
        {
            // 파일 없음
        }
        return (T)(object)null;
    }
    public static void SaveObjectToJson<T>(string jsonPath, T objectToSave)
    {
        File.WriteAllText(
            jsonPath + ".json",
            JsonConvert.SerializeObject(objectToSave, Formatting.Indented)
            );
    }

    public static Queue<string> LoadTextFromFile(string filePath)
    {
        StreamReader sr = new StreamReader($"Assets/Resources/Texts/{GlobalSetting.Language}/{filePath}.txt", System.Text.Encoding.Default, true);
        Queue<string> res = new();
        string s;
        while ((s = sr.ReadLine()) != null)
        {
            res.Enqueue(s);
        }
        return res;
    }

    public static void ApplyLanguage()
    {
        CommonGameManager.Instance.FadeScreen(true, actionAfter: () =>
        {
            CommonGameManager.Instance.FadeScreen(false, actionBefore: () =>
            {
                // 텍스트 불러오기
                // 옵션 관련
                Queue<string> curQ = LoadTextFromFile("Option");
                GlobalText.Common.ReturnToGame = curQ.Dequeue();
                GlobalText.Common.GoToOption = curQ.Dequeue();
                GlobalText.Common.SaveGame = curQ.Dequeue();
                GlobalText.Common.LoadGame = curQ.Dequeue();
                GlobalText.Common.StartGame = curQ.Dequeue();
                GlobalText.Common.GoToDesktop = curQ.Dequeue();
                GlobalText.Common.Loading = curQ.Dequeue();
                GlobalText.Common.Back = curQ.Dequeue();
                GlobalText.Common.Language = curQ.Dequeue();

                // 인벤토리 관련
                curQ = LoadTextFromFile("Inventory");
                GlobalText.Inventory.Inven = curQ.Dequeue();
                GlobalText.Inventory.Equipment = curQ.Dequeue();
                GlobalText.Inventory.Looting = curQ.Dequeue();
                GlobalText.Inventory.Helmet = curQ.Dequeue();
                GlobalText.Inventory.Mask = curQ.Dequeue();
                GlobalText.Inventory.Body = curQ.Dequeue();
                GlobalText.Inventory.Backpack = curQ.Dequeue();
                GlobalText.Inventory.WeaponPri = curQ.Dequeue();
                GlobalText.Inventory.WeaponSec = curQ.Dequeue();

                // 상태이상 관련
                curQ = LoadTextFromFile("Condition");
                ConditionInfo newCondition = new();
                ConditionType curType;
                string s;
                while (curQ.TryDequeue(out s))
                {
                    // 타입: 1줄
                    curType = System.Enum.Parse<ConditionType>(s);
                    // 제목: 1줄
                    newCondition.title = curQ.Dequeue();
                    newCondition.description = string.Empty;
                    // 효과: n줄
                    while (curQ.TryDequeue(out s))
                    {
                        if (s == string.Empty) break;
                        newCondition.description += $"{s}\n";
                    }
                    GlobalText.Conditions[curType] = newCondition;
                    newCondition = new();
                }

                switch (SceneManager.GetActiveScene().name)
                {
                    case "MainMenu":
                        ApplyLanguageMainMenu();
                        break;
                    case "Loading":
                        ApplyLanguageLoading();
                        break;
                    default:
                        ApplyLanguageInGame();
                        break;
                }
            });
        });
    }

    private static void ApplyLanguageMainMenu()
    {
        GlobalComponent.Common.Text.MainMenu.startGame.text = GlobalText.Common.StartGame;
        GlobalComponent.Common.Text.MainMenu.toDesktop.text = GlobalText.Common.GoToDesktop;
    }

    private static void ApplyLanguageLoading()
    {
        GlobalComponent.Common.Text.Loading.loading.text = GlobalText.Common.Loading;
    }

    private static void ApplyLanguageInGame()
    {
        // 옵션
        GlobalComponent.Common.Text.Option.backToGame.text = GlobalText.Common.ReturnToGame;
        GlobalComponent.Common.Text.Option.goToOption.text = GlobalText.Common.GoToOption;
        GlobalComponent.Common.Text.Option.saveGame.text = GlobalText.Common.SaveGame;
        GlobalComponent.Common.Text.Option.goToDesktop.text = GlobalText.Common.GoToDesktop;
        GlobalComponent.Common.Text.Option.back.text = GlobalText.Common.Back;
        GlobalComponent.Common.Text.Option.language.text = GlobalText.Common.Language;

        // 인벤토리
        GlobalComponent.Common.Text.Inventory.inventory.text = GlobalText.Inventory.Inven;
        GlobalComponent.Common.Text.Inventory.looting.text = GlobalText.Inventory.Looting;
        GlobalComponent.Common.Text.Inventory.equipment.text = GlobalText.Inventory.Equipment;
        GlobalComponent.Common.Text.Inventory.Equipment.helmet.text = GlobalText.Inventory.Helmet;
        GlobalComponent.Common.Text.Inventory.Equipment.mask.text = GlobalText.Inventory.Mask;
        GlobalComponent.Common.Text.Inventory.Equipment.body.text = GlobalText.Inventory.Body;
        GlobalComponent.Common.Text.Inventory.Equipment.backpack.text = GlobalText.Inventory.Backpack;
        GlobalComponent.Common.Text.Inventory.Equipment.weaponPrimary.text = GlobalText.Inventory.WeaponPri;
        GlobalComponent.Common.Text.Inventory.Equipment.weaponSecondary.text = GlobalText.Inventory.WeaponSec;
    }
}
