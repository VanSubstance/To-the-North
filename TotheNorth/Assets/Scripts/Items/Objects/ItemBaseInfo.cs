using Assets.Scripts.Battles;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Info", menuName = "Data Objects/Item Info", order = int.MaxValue)]
public class ItemBaseInfo : ScriptableObject
{
    public ItemType itemType;
    public string imagePath;
    public string itemName;
    public string description;
    public int price, range;

    // 아래 코드는 예시용임!
    // 리팩토링이 반드시 필요함!!
    public ProjectileInfo projectileInfo;
    public float delayAmongFire;
}
