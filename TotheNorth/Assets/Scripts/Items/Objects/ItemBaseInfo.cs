using System;
using UnityEngine;

[Serializable]
public class ItemBaseInfo : ScriptableObject
{
    [HideInInspector]
    public ItemType itemType;
    public Vector2 size;
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
        res.size = size;
        return res;
    }
}
