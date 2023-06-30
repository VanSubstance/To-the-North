using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Components.Conversations.Objects;
using Assets.Scripts.Components.Hovers;
using Assets.Scripts.Creatures;
using Assets.Scripts.Items;
using Assets.Scripts.Users;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    /// <summary>
    /// 사용할 오디오클립 전부 불러오기
    /// </summary>
    public static void LoadAudioClips()
    {
        string rootPath = $"Sounds/";
        AudioClip[] temp;
        /// 1. 이동 소리
        temp = Resources.LoadAll<AudioClip>($"{rootPath}Move/Human");
        GlobalDictionary.Sound.Move[CreatureType.Human].Run = temp[0];
        GlobalDictionary.Sound.Move[CreatureType.Human].Walk = temp[1];
        temp = Resources.LoadAll<AudioClip>($"{rootPath}Move/FourLeg");
        GlobalDictionary.Sound.Move[CreatureType.FourLeg].Run = temp[0];
        GlobalDictionary.Sound.Move[CreatureType.FourLeg].Walk = temp[1];
        temp = Resources.LoadAll<AudioClip>($"{rootPath}Move/Slime");
        GlobalDictionary.Sound.Move[CreatureType.Slime].Run = temp[0];
        GlobalDictionary.Sound.Move[CreatureType.Slime].Walk = temp[1];

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
                    cond.conditionType = tokens[0];
                    cond.contentType = System.Enum.Parse<ConvChoiceInfo.ChoiceCondition.ContentType>(tokens[1]);
                    cond.code = tokens[2];
                    switch (cond.contentType)
                    {
                        case ConvChoiceInfo.ChoiceCondition.ContentType.Item:
                            cond.amount = int.Parse(tokens[3]);
                            break;
                        case ConvChoiceInfo.ChoiceCondition.ContentType.Quest:
                            break;
                        case ConvChoiceInfo.ChoiceCondition.ContentType.Currency:
                            cond.code = null;
                            cond.amount = int.Parse(tokens[2]);
                            break;
                    }
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
        GlobalDictionary.Text.Common.ReturnToGame = curQ.Dequeue();
        GlobalDictionary.Text.Common.GoToOption = curQ.Dequeue();
        GlobalDictionary.Text.Common.SaveGame = curQ.Dequeue();
        GlobalDictionary.Text.Common.LoadGame = curQ.Dequeue();
        GlobalDictionary.Text.Common.StartGame = curQ.Dequeue();
        GlobalDictionary.Text.Common.GoToDesktop = curQ.Dequeue();
        GlobalDictionary.Text.Common.Loading = curQ.Dequeue();
        GlobalDictionary.Text.Common.Back = curQ.Dequeue();
        GlobalDictionary.Text.Common.Language = curQ.Dequeue();

        // 인벤토리 관련
        curQ = LoadTextFromFile("Inventory");
        GlobalDictionary.Text.Inventory.Inven = curQ.Dequeue();
        GlobalDictionary.Text.Inventory.Equipment = curQ.Dequeue();
        GlobalDictionary.Text.Inventory.Looting = curQ.Dequeue();
        GlobalDictionary.Text.Inventory.Helmet = curQ.Dequeue();
        GlobalDictionary.Text.Inventory.Mask = curQ.Dequeue();
        GlobalDictionary.Text.Inventory.Body = curQ.Dequeue();
        GlobalDictionary.Text.Inventory.Backpack = curQ.Dequeue();
        GlobalDictionary.Text.Inventory.WeaponPri = curQ.Dequeue();
        GlobalDictionary.Text.Inventory.WeaponSec = curQ.Dequeue();
        GlobalDictionary.Text.Inventory.Commerce = curQ.Dequeue();

        // 시스템 관련
        curQ = LoadTextFromFile("System");
        GlobalDictionary.Text.System.ItemGet = curQ.Dequeue();
        GlobalDictionary.Text.System.ItemPay = curQ.Dequeue();
        GlobalDictionary.Text.System.QuestGet = curQ.Dequeue();
        GlobalDictionary.Text.System.QuestClear = curQ.Dequeue();
        GlobalDictionary.Text.System.CurrencyGet = curQ.Dequeue();
        GlobalDictionary.Text.System.CurrencyPay = curQ.Dequeue();

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
            GlobalDictionary.Text.Conditions[curType] = newCondition;
            newCondition = new();
        }

        // 아이템 관련
        curQ = LoadTextFromFile("Item");
        GlobalDictionary.Text.Item.Armor.tPenatration = curQ.Dequeue();
        GlobalDictionary.Text.Item.Armor.tImpact = curQ.Dequeue();
        GlobalDictionary.Text.Item.Armor.tHeat = curQ.Dequeue();

        GlobalDictionary.Text.Item.Bullet.tBulletType = curQ.Dequeue();
        GlobalDictionary.Text.Item.Bullet.tAccelSpd = curQ.Dequeue();

        GlobalDictionary.Text.Item.Damage.tPwPene = curQ.Dequeue();
        GlobalDictionary.Text.Item.Damage.tPwImp = curQ.Dequeue();
        GlobalDictionary.Text.Item.Damage.tPwKnock = curQ.Dequeue();
        GlobalDictionary.Text.Item.Damage.tDmgPene = curQ.Dequeue();
        GlobalDictionary.Text.Item.Damage.tDmgImp = curQ.Dequeue();

        GlobalDictionary.Text.Item.Weapon.tAtkSpd = curQ.Dequeue();
        GlobalDictionary.Text.Item.Weapon.tHandType = curQ.Dequeue();

        GlobalDictionary.Text.Item.WeaponRange.tReload = curQ.Dequeue();
        GlobalDictionary.Text.Item.WeaponRange.tRange = curQ.Dequeue();
        GlobalDictionary.Text.Item.WeaponRange.tProjSpd = curQ.Dequeue();
        GlobalDictionary.Text.Item.WeaponRange.tBulletType = curQ.Dequeue();


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
        GlobalComponent.Common.Text.MainMenu.startGame.text = GlobalDictionary.Text.Common.StartGame;
        GlobalComponent.Common.Text.MainMenu.toDesktop.text = GlobalDictionary.Text.Common.GoToDesktop;
    }

    private static void ApplyLanguageLoading()
    {
        GlobalComponent.Common.Text.Loading.loading.text = GlobalDictionary.Text.Common.Loading;
    }

    private static void ApplyLanguageInGame()
    {
        // 옵션
        GlobalComponent.Common.Text.Option.backToGame.text = GlobalDictionary.Text.Common.ReturnToGame;
        GlobalComponent.Common.Text.Option.goToOption.text = GlobalDictionary.Text.Common.GoToOption;
        GlobalComponent.Common.Text.Option.saveGame.text = GlobalDictionary.Text.Common.SaveGame;
        GlobalComponent.Common.Text.Option.goToDesktop.text = GlobalDictionary.Text.Common.GoToDesktop;
        GlobalComponent.Common.Text.Option.back.text = GlobalDictionary.Text.Common.Back;
        GlobalComponent.Common.Text.Option.language.text = GlobalDictionary.Text.Common.Language;

        // 인벤토리
        GlobalComponent.Common.Text.Inventory.inventory.text = GlobalDictionary.Text.Inventory.Inven;
        GlobalComponent.Common.Text.Inventory.looting.text = GlobalDictionary.Text.Inventory.Looting;
        GlobalComponent.Common.Text.Inventory.commerce.text = GlobalDictionary.Text.Inventory.Commerce;
        GlobalComponent.Common.Text.Inventory.equipment.text = GlobalDictionary.Text.Inventory.Equipment;
        GlobalComponent.Common.Text.Inventory.Equipment.helmet.text = GlobalDictionary.Text.Inventory.Helmet;
        GlobalComponent.Common.Text.Inventory.Equipment.mask.text = GlobalDictionary.Text.Inventory.Mask;
        GlobalComponent.Common.Text.Inventory.Equipment.body.text = GlobalDictionary.Text.Inventory.Body;
        GlobalComponent.Common.Text.Inventory.Equipment.backpack.text = GlobalDictionary.Text.Inventory.Backpack;
        GlobalComponent.Common.Text.Inventory.Equipment.weaponPrimary.text = GlobalDictionary.Text.Inventory.WeaponPri;
        GlobalComponent.Common.Text.Inventory.Equipment.weaponSecondary.text = GlobalDictionary.Text.Inventory.WeaponSec;

        // 아이템 정보
        GlobalComponent.Common.Text.Item.Armor.tPenatration.text = GlobalDictionary.Text.Item.Armor.tPenatration;
        GlobalComponent.Common.Text.Item.Armor.tImpact.text = GlobalDictionary.Text.Item.Armor.tImpact;
        GlobalComponent.Common.Text.Item.Armor.tHeat.text = GlobalDictionary.Text.Item.Armor.tHeat;

        GlobalComponent.Common.Text.Item.Bullet.tBulletType.text = GlobalDictionary.Text.Item.Bullet.tBulletType;
        GlobalComponent.Common.Text.Item.Bullet.tAccelSpd.text = GlobalDictionary.Text.Item.Bullet.tAccelSpd;

        GlobalComponent.Common.Text.Item.Damage.tPwPene.text = GlobalDictionary.Text.Item.Damage.tPwPene;
        GlobalComponent.Common.Text.Item.Damage.tPwImp.text = GlobalDictionary.Text.Item.Damage.tPwImp;
        GlobalComponent.Common.Text.Item.Damage.tPwKnock.text = GlobalDictionary.Text.Item.Damage.tPwKnock;
        GlobalComponent.Common.Text.Item.Damage.tDmgPene.text = GlobalDictionary.Text.Item.Damage.tDmgPene;
        GlobalComponent.Common.Text.Item.Damage.tDmgImp.text = GlobalDictionary.Text.Item.Damage.tDmgImp;

        GlobalComponent.Common.Text.Item.Weapon.tAtkSpd.text = GlobalDictionary.Text.Item.Weapon.tAtkSpd;
        GlobalComponent.Common.Text.Item.Weapon.tHandType.text = GlobalDictionary.Text.Item.Weapon.tHandType;

        GlobalComponent.Common.Text.Item.WeaponRange.tBulletType.text = GlobalDictionary.Text.Item.WeaponRange.tBulletType;
        GlobalComponent.Common.Text.Item.WeaponRange.tProjSpd.text = GlobalDictionary.Text.Item.WeaponRange.tProjSpd;
        GlobalComponent.Common.Text.Item.WeaponRange.tRange.text = GlobalDictionary.Text.Item.WeaponRange.tRange;
        GlobalComponent.Common.Text.Item.WeaponRange.tReload.text = GlobalDictionary.Text.Item.WeaponRange.tReload;
    }
}
