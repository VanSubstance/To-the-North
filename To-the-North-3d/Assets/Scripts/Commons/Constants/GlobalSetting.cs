using UnityEngine;

public static class GlobalSetting
{
    public static float accelSpeed = 2f;
    public readonly static float UnitSize = 120;
    private static string language = "Kor";
    public static string Language
    {
        set
        {
            language = value;
            if (CommonGameManager.Instance)
            {
                CommonGameManager.Instance.ApplyLanguage();
                return;
            }
        }
        get
        {
            return language;
        }
    }
}