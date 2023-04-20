using UnityEngine;

public static class GlobalSetting
{
    public static float accelSpeed = 2f;
    public const float unitSize = 120f;
    public const float gridUnitSize = 2f / 3f * unitSize;
    public static readonly Vector2 inventorySize = new Vector2(10, 12);
    public static int InventorySlotCount
    {
        get => (int)inventorySize.x * (int)inventorySize.y;
    }
}