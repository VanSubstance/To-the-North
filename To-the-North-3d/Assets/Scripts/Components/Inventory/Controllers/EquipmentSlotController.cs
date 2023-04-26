using Assets.Scripts.Commons;
using Assets.Scripts.Items;
using Assets.Scripts.Users;
using UnityEngine;

public class EquipmentSlotController : InventorySlotController
{
    private ItemEquipmentInfo equipItem;
    public ItemEquipmentInfo EquipItemInfo
    {
        set
        {
            if (value != null)
            {
                EquipItem(value);
                return;
            }
            if (equipItem != null)
            {
                UnEquipItem();
                return;
            }
        }
        get
        {
            return equipItem;
        }
    }
    public EquipBodyType equipType;
    public bool IsEquipped
    {
        get
        {
            return equipItem != null;
        }
    }

    public void EquipItem(ItemEquipmentInfo _equipInfo)
    {
        equipItem = _equipInfo;
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
                    case EquipBodyType.BackPack:
                        DataManager.Instance.back = armorInfo;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.BackPack, armorInfo);
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
                    case EquipBodyType.BackPack:
                        DataManager.Instance.back = null;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.BackPack, null);
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