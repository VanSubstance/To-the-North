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
        switch (equipItem)
        {
            case ItemArmorInfo armorInfo:
                switch (equipType)
                {
                    case EquipBodyType.Helmat:
                        DataManager.Instance.helmat = armorInfo;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Helmat, armorInfo);
                        break;
                    case EquipBodyType.Head:
                        DataManager.Instance.helmat = armorInfo;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Head, armorInfo);
                        break;
                    case EquipBodyType.Mask:
                        DataManager.Instance.mask = armorInfo;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Mask, armorInfo);
                        break;
                    case EquipBodyType.Body:
                        DataManager.Instance.body = armorInfo;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Body, armorInfo);
                        break;
                    case EquipBodyType.Leg:
                        DataManager.Instance.leg = armorInfo;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Leg, armorInfo);
                        break;
                    case EquipBodyType.Back:
                        DataManager.Instance.back = armorInfo;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Back, armorInfo);
                        break;
                }
                break;
            case ItemWeaponInfo weaponInfo:
                switch (equipType)
                {
                    case EquipBodyType.Right:
                        DataManager.Instance.right = weaponInfo;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Right, weaponInfo);
                        break;
                    case EquipBodyType.Left:
                        DataManager.Instance.left = weaponInfo;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Left, weaponInfo);
                        break;
                }
                break;
            default:
                break;
        }
    }
    public void UnEquipItem()
    {
        switch (equipItem)
        {
            case ItemArmorInfo armorInfo:
                switch (equipType)
                {
                    case EquipBodyType.Helmat:
                        DataManager.Instance.helmat = null;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Helmat, null);
                        break;
                    case EquipBodyType.Head:
                        DataManager.Instance.helmat = null;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Head, null);
                        break;
                    case EquipBodyType.Mask:
                        DataManager.Instance.mask = null;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Mask, null);
                        break;
                    case EquipBodyType.Body:
                        DataManager.Instance.body = null;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Body, null);
                        break;
                    case EquipBodyType.Leg:
                        DataManager.Instance.leg = null;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Leg, null);
                        break;
                    case EquipBodyType.Back:
                        DataManager.Instance.back = null;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Back, null);
                        break;
                }
                break;
            case ItemWeaponInfo weaponInfo:
                switch (equipType)
                {
                    case EquipBodyType.Right:
                        DataManager.Instance.right = null;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Right, null);
                        break;
                    case EquipBodyType.Left:
                        DataManager.Instance.left = null;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Left, null);
                        break;
                }
                break;
            default:
                break;
        }
        equipItem = null;
    }
}