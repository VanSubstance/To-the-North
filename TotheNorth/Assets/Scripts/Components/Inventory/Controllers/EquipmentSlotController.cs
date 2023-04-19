using Assets.Scripts.Commons;
using Assets.Scripts.Items;
using Assets.Scripts.Users;
using UnityEngine;
using static GlobalComponent.Common;

public class EquipmentSlotController : InventorySlotController
{
    public ItemEquipmentInfo equipItem;

    public void EquipItem()
    {
        equipItem = itemTF.GetChild(2).GetComponent<ItemEquipmentController>().info;
        Debug.Log(equipItem);
        switch (equipItem)
        {
            case ItemArmorInfo armorInfo:
                // Perform actions specific to ItemArmorInfo
                DataManager.Instance.body = armorInfo;
                UserBaseController.Instance.ChangeEquipment(EquipBodyType.Body, armorInfo);
                break;
            case ItemWeaponInfo weaponInfo:
                // Perform actions specific to ItemWeaponInfo
                DataManager.Instance.right = weaponInfo;
                UserBaseController.Instance.ChangeEquipment(EquipBodyType.Right, weaponInfo);
                break;
            default:
                // Handle other cases
                break;
        }
    }
    public void UnEquipItem()
    {
        switch (equipItem)
        {
            case ItemArmorInfo armorInfo:
                // Perform actions specific to ItemArmorInfo
                DataManager.Instance.body = null;
                UserBaseController.Instance.ChangeEquipment(EquipBodyType.Body, null);
                break;
            case ItemWeaponInfo weaponInfo:
                // Perform actions specific to ItemWeaponInfo
                DataManager.Instance.right = null;
                UserBaseController.Instance.ChangeEquipment(EquipBodyType.Right, null);
                break;
            default:
                // Handle other cases
                break;
        }
        equipItem = null;
    }
}