using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoWeaponController : MonoBehaviour, IHoverItemInfo
    {
        [SerializeField]
        private TextMeshProUGUI tAtkSpd, tHandType;
        private void Awake()
        {
        }

        public void OnItemInfoChanged(ItemBaseInfo _info)
        {
            if (_info == null)
            {
                gameObject.SetActive(false);
                return;
            }
            ItemWeaponInfo info = (ItemWeaponInfo)_info;
            tAtkSpd.text = $"1회 / {info.delayAmongFire}초";
            tHandType.text = info.handType == EquipHandType.Multiple ? "양손" : "한손";
            gameObject.SetActive(true);
        }
    }
}
