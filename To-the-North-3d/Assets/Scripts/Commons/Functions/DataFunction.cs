using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Commons;
using Assets.Scripts.Components.Conversations.Objects;
using Assets.Scripts.Components.Hovers;
using Assets.Scripts.Items;
using Assets.Scripts.Users;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine;

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

    public static ItemBaseInfo LoadItemInfoByCode(string code)
    {
        string path = $"DataObjects/Items/";
        string[] tokens = code.Split("-");
        switch (tokens[0])
        {
            case "B":
                path += "Consumables/Bullets/";
                break;
            case "M":
                path += "Consumables/Medicines/";
                break;
            case "F":
                path += "Consumables/Foods/";
                break;
            case "Ba":
                path += "Equipments/Armors/Backpack/";
                break;
            case "Bo":
                path += "Equipments/Armors/Body/";
                break;
            case "H":
                path += "Equipments/Armors/Helmet/";
                break;
            case "Ma":
                path += "Equipments/Armors/Mask/";
                break;
            case "Mag":
                path += "Equipments/Magazines/";
                break;
            case "W":
                path += "Equipments/Weapons/";
                break;
            case "Mon":
                path += "Materials/";
                break;
        }
        path += code;
        ItemBaseInfo res = Resources.Load<ItemBaseInfo>(path);
        return res;
    }

    /// <summary>
    /// 아이템 객체 어레이 텍스트에서 추출 함수
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static ItemBaseInfo[] LoadItemList(string filePath)
    {
        List<ItemBaseInfo> res = new List<ItemBaseInfo>();
        Queue<string> codes = LoadTextFromFile(filePath);
        string code;
        while (codes.TryDequeue(out code))
        {
            res.Add(LoadItemInfoByCode(code));
        }
        return res.ToArray();
    }

    /// <summary>
    /// NPC에 해당하는 대화 데이터 불러오기 함수
    /// </summary>
    /// <param name="npcPath"></param>
    public static ConvInfo[] LoadConversation(string npcPath)
    {
        Queue<string> lines = LoadTextFromFile(npcPath);
        string line;
        string[] tokens;
        ConvInfo[] res = new ConvInfo[100];
        ConvInfo c;
        ConvChoiceInfo choice;
        ConvChoiceInfo.ChoiceCondition cond;

        while (lines.TryDequeue(out line))
        {
            // 숫자로 시작
            while (line.Equals(string.Empty))
            {
                // 빨리감기 -> 숫자 찾기
                lines.TryDequeue(out line);
            }
            // 신규 ConvInfo 생성
            c = new ConvInfo();
            // 페이지 번호
            res[int.Parse(line)] = c;
            // 설명 할당
            string d = string.Empty;
            while ((line = lines.Dequeue()).Length != 0)
            {
                d += $"{line}\n";
            }
            c.desc = d;

            // 선택지 찾기
            while (true)
            {
                while (!line.Equals("?"))
                {
                    lines.TryDequeue(out line);
                }
                // ? <- 선택지 시작
                choice = new ConvChoiceInfo();
                // 조건들 추가
                while (!(line = lines.Dequeue()).Equals("<"))
                {
                    tokens = line.Split(": ");
                    cond = new ConvChoiceInfo.ChoiceCondition();
                    cond.conditionType = System.Enum.Parse<ConvChoiceInfo.ChoiceCondition.ConditionType>(tokens[0]);
                    cond.contentType = System.Enum.Parse<ConvChoiceInfo.ChoiceCondition.ContentType>(tokens[1]);
                    cond.code = tokens[2];
                    if (cond.contentType.Equals(ConvChoiceInfo.ChoiceCondition.ContentType.Item)) cond.amount = int.Parse(tokens[3]);
                    choice.conditions.Add(cond);
                }
                // 텍스트 추가
                choice.text = lines.Dequeue();
                // 목표 지정
                choice.next = lines.Dequeue().Replace("> ", "");
                // 선택지 추가
                c.choices.Add(choice);
                // 선택지 끝인지 확인
                if ((line = lines.Dequeue()).Equals("!")) break;
            }
        }
        return res;
    }

    /// <summary>
    /// 언어 변환 함수
    /// </summary>
    public static void ApplyLanguage()
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
        GlobalText.Inventory.Commerce = curQ.Dequeue();

        // 시스템 관련
        curQ = LoadTextFromFile("System");
        GlobalText.System.ItemGet = curQ.Dequeue();
        GlobalText.System.ItemPay = curQ.Dequeue();
        GlobalText.System.QuestGet = curQ.Dequeue();
        GlobalText.System.QuestClear = curQ.Dequeue();

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

        // 아이템 관련
        curQ = LoadTextFromFile("Item");
        GlobalText.Item.Armor.tPenatration = curQ.Dequeue();
        GlobalText.Item.Armor.tImpact = curQ.Dequeue();
        GlobalText.Item.Armor.tHeat = curQ.Dequeue();

        GlobalText.Item.Bullet.tBulletType = curQ.Dequeue();
        GlobalText.Item.Bullet.tAccelSpd = curQ.Dequeue();

        GlobalText.Item.Damage.tPwPene = curQ.Dequeue();
        GlobalText.Item.Damage.tPwImp = curQ.Dequeue();
        GlobalText.Item.Damage.tPwKnock = curQ.Dequeue();
        GlobalText.Item.Damage.tDmgPene = curQ.Dequeue();
        GlobalText.Item.Damage.tDmgImp = curQ.Dequeue();

        GlobalText.Item.Weapon.tAtkSpd = curQ.Dequeue();
        GlobalText.Item.Weapon.tHandType = curQ.Dequeue();

        GlobalText.Item.WeaponRange.tReload = curQ.Dequeue();
        GlobalText.Item.WeaponRange.tRange = curQ.Dequeue();
        GlobalText.Item.WeaponRange.tProjSpd = curQ.Dequeue();
        GlobalText.Item.WeaponRange.tBulletType = curQ.Dequeue();


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
        GlobalComponent.Common.Text.Inventory.commerce.text = GlobalText.Inventory.Commerce;
        GlobalComponent.Common.Text.Inventory.equipment.text = GlobalText.Inventory.Equipment;
        GlobalComponent.Common.Text.Inventory.Equipment.helmet.text = GlobalText.Inventory.Helmet;
        GlobalComponent.Common.Text.Inventory.Equipment.mask.text = GlobalText.Inventory.Mask;
        GlobalComponent.Common.Text.Inventory.Equipment.body.text = GlobalText.Inventory.Body;
        GlobalComponent.Common.Text.Inventory.Equipment.backpack.text = GlobalText.Inventory.Backpack;
        GlobalComponent.Common.Text.Inventory.Equipment.weaponPrimary.text = GlobalText.Inventory.WeaponPri;
        GlobalComponent.Common.Text.Inventory.Equipment.weaponSecondary.text = GlobalText.Inventory.WeaponSec;

        // 아이템 정보
        GlobalComponent.Common.Text.Item.Armor.tPenatration.text = GlobalText.Item.Armor.tPenatration;
        GlobalComponent.Common.Text.Item.Armor.tImpact.text = GlobalText.Item.Armor.tImpact;
        GlobalComponent.Common.Text.Item.Armor.tHeat.text = GlobalText.Item.Armor.tHeat;

        GlobalComponent.Common.Text.Item.Bullet.tBulletType.text = GlobalText.Item.Bullet.tBulletType;
        GlobalComponent.Common.Text.Item.Bullet.tAccelSpd.text = GlobalText.Item.Bullet.tAccelSpd;

        GlobalComponent.Common.Text.Item.Damage.tPwPene.text = GlobalText.Item.Damage.tPwPene;
        GlobalComponent.Common.Text.Item.Damage.tPwImp.text = GlobalText.Item.Damage.tPwImp;
        GlobalComponent.Common.Text.Item.Damage.tPwKnock.text = GlobalText.Item.Damage.tPwKnock;
        GlobalComponent.Common.Text.Item.Damage.tDmgPene.text = GlobalText.Item.Damage.tDmgPene;
        GlobalComponent.Common.Text.Item.Damage.tDmgImp.text = GlobalText.Item.Damage.tDmgImp;

        GlobalComponent.Common.Text.Item.Weapon.tAtkSpd.text = GlobalText.Item.Weapon.tAtkSpd;
        GlobalComponent.Common.Text.Item.Weapon.tHandType.text = GlobalText.Item.Weapon.tHandType;

        GlobalComponent.Common.Text.Item.WeaponRange.tBulletType.text = GlobalText.Item.WeaponRange.tBulletType;
        GlobalComponent.Common.Text.Item.WeaponRange.tProjSpd.text = GlobalText.Item.WeaponRange.tProjSpd;
        GlobalComponent.Common.Text.Item.WeaponRange.tRange.text = GlobalText.Item.WeaponRange.tRange;
        GlobalComponent.Common.Text.Item.WeaponRange.tReload.text = GlobalText.Item.WeaponRange.tReload;
    }
}
