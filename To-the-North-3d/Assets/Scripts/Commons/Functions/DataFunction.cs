using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

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
        StreamReader sr = new StreamReader($"Assets/Resources/Texts/{GlobalSetting.Langauge}/{filePath}", System.Text.Encoding.Default, true);
        Queue<string> res = new();
        string s;
        while ((s = sr.ReadLine()) != null)
        {
            res.Enqueue(s);
        }
        return res;
    }
}
