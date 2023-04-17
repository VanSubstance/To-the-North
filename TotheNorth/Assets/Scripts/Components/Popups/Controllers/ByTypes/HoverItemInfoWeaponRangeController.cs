using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Popups
{
    public class HoverItemInfoWeaponRangeController : MonoBehaviour, IHoverItemInfo
    {
        [SerializeField]
        private TextMeshProUGUI tReload, tRange, tProjSpd, tBulletType;
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
            tReload.text = $"{info.timeReload}초";
            tRange.text = info.range.ToString();
            tProjSpd.text = info.spd.ToString();
            tBulletType.text = $"{HoverItemInfoBulletController.ConvertBulletTypeToString(info.bulletType)}";
            gameObject.SetActive(true);
        }
    }
}
