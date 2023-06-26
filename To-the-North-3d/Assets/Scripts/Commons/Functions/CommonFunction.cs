using System;


public class CommonFunction
{
    /// <summary>
    /// Enum 속성들 Array로 추출
    /// </summary>
    /// <param name="enumType"></param>
    /// <returns></returns>
    public static Array ListOfEnumValue(Type enumType)
    {
        return Enum.GetValues(enumType);
    }
}
