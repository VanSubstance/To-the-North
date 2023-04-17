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
            string bt = string.Empty;
            switch (info.bulletType)
            {
                case ItemBulletType.None:
                    break;
                case ItemBulletType.Bullet_mm9:
                    bt += "9mm";
                    break;
                case ItemBulletType.Arrow:
                    bt += "화살";
                    break;
            }
            tBulletType.text = $"{bt}";
            gameObject.SetActive(true);
        }
    }
}
