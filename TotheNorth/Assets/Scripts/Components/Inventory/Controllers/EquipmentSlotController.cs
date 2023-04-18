using Assets.Scripts.Commons;
using Assets.Scripts.Items;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotController : InventorySlotController
{
    public ItemEquipmentInfo equipItem;

    public void EquipItem()
    {
        equipItem = itemTF.GetChild(2).GetComponent<ItemEquipmentController>().info;
        switch (equipItem)
        {
            case ItemArmorInfo armorInfo:
                // Perform actions specific to ItemArmorInfo
                DataManager.Instance.body = armorInfo;
                break;
            case ItemWeaponInfo weaponInfo:
                // Perform actions specific to ItemWeaponInfo
                DataManager.Instance.right = weaponInfo;
                break;
            default:
                // Handle other cases
                break;
        }
        DataManager.Instance.updateEquipment();
    }
    public void UnEquipItem()
    {
        switch (equipItem)
        {
            case ItemArmorInfo armorInfo:
                // Perform actions specific to ItemArmorInfo
                DataManager.Instance.body = null;
                break;
            case ItemWeaponInfo weaponInfo:
                // Perform actions specific to ItemWeaponInfo
                DataManager.Instance.right = null;
                break;
            default:
                // Handle other cases
                break;
        }
        equipItem = null;
        DataManager.Instance.updateEquipment();
    }
}