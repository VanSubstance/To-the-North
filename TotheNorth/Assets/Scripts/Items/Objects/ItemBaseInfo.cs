using Assets.Scripts.Battles;
using UnityEngine;

public class ItemBaseInfo : ScriptableObject
{
    public ItemType itemType;
    public string imagePath;
    public string itemName;
    public string description;
    public int price;

    public ItemBaseInfo GetClone()
    {
        ItemBaseInfo res = CreateInstance<ItemBaseInfo>();
        res.itemType = itemType;
        res.imagePath = imagePath;
        res.itemName = itemName;
        res.description = description;
        res.price = price;
        return res;
    }
}
