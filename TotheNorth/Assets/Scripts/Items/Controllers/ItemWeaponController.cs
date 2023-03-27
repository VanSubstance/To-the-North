using Assets.Scripts.Items.Abstracts;
using Assets.Scripts.Items.Objects;
using UnityEngine;

namespace Assets.Scripts.Items.Controllers
{
    internal class ItemWeaponController : AItemEquipmentBaseController<ItemWeaponInfo>
    {
        [SerializeField]
        private string weaponName;

        private ItemWeaponInfo weaponInfo;
        private void Awake()
        {
            weaponInfo = new ItemWeaponInfo(weaponName, 10);
        }

        public override ItemWeaponInfo GetItemEquipmentInfo()
        {
            Debug.Log("값 호출:: " + weaponInfo.ToString());
            return weaponInfo;
        }
    }
}
