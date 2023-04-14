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
}
