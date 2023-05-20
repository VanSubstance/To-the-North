using Assets.Scripts.Commons;
using Assets.Scripts.Items;
using Assets.Scripts.Users;

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
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Helmat, armorInfo);
                        break;
                    case EquipBodyType.Head:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Head, armorInfo);
                        break;
                    case EquipBodyType.Mask:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Mask, armorInfo);
                        break;
                    case EquipBodyType.Body:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Body, armorInfo);
                        break;
                    case EquipBodyType.Leg:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Leg, armorInfo);
                        break;
                    case EquipBodyType.BackPack:
                        InGameStatus.Weight.MaxWeightC = equipItem.weightExtension;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.BackPack, armorInfo);
                        break;
                }
                break;
            case ItemWeaponInfo weaponInfo:
                switch (equipType)
                {
                    case EquipBodyType.Primary:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Primary, weaponInfo);
                        break;
                    case EquipBodyType.Secondary:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Secondary, weaponInfo);
                        break;
                }
                break;
            default:
                break;
        }
        InGameStatus.Weight.WeightC = equipItem.weight;
    }
    public void UnEquipItem()
    {
        switch (equipItem)
        {
            case ItemArmorInfo armorInfo:
                switch (equipType)
                {
                    case EquipBodyType.Helmat:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Helmat, null);
                        break;
                    case EquipBodyType.Head:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Head, null);
                        break;
                    case EquipBodyType.Mask:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Mask, null);
                        break;
                    case EquipBodyType.Body:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Body, null);
                        break;
                    case EquipBodyType.Leg:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Leg, null);
                        break;
                    case EquipBodyType.BackPack:
                        InGameStatus.Weight.MaxWeightC = -equipItem.weightExtension;
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.BackPack, null);
                        break;
                }
                break;
            case ItemWeaponInfo weaponInfo:
                switch (equipType)
                {
                    case EquipBodyType.Primary:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Primary, null);
                        break;
                    case EquipBodyType.Secondary:
                        UserBaseController.Instance.ChangeEquipment(EquipBodyType.Secondary, null);
                        break;
                }
                break;
            default:
                break;
        }
        InGameStatus.Weight.WeightC = -equipItem.weight;
        equipItem = null;
    }
}