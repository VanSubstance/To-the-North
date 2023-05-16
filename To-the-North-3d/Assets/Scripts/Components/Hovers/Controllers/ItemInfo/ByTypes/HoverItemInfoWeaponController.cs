using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoWeaponController : MonoBehaviour, IHoverItemInfo
    {
        [SerializeField]
        private TextMeshProUGUI tAtkSpd, tHandType;
        [SerializeField]
        private TextMeshProUGUI atkSpd, handType;
        private void Awake()
        {
            GlobalComponent.Common.Text.Item.Weapon.tAtkSpd = tAtkSpd;
            GlobalComponent.Common.Text.Item.Weapon.tHandType = tHandType;
        }

        public void OnItemInfoChanged(ItemBaseInfo _info)
        {
            if (_info == null)
            {
                gameObject.SetActive(false);
                return;
            }
            ItemWeaponInfo info = (ItemWeaponInfo)_info;
            atkSpd.text = $"1회 / {info.delayAmongFire}초";
            handType.text = info.handType == EquipHandType.Multiple ? "양손" : "한손";
            gameObject.SetActive(true);
        }
    }
}
