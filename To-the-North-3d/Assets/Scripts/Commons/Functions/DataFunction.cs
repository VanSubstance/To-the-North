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
}
