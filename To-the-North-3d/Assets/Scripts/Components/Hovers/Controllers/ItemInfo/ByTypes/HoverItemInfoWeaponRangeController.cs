using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoWeaponRangeController : MonoBehaviour, IHoverItemInfo
    {
        [SerializeField]
        private TextMeshProUGUI tReload, tRange, tProjSpd, tBulletType;
        [SerializeField]
        private TextMeshProUGUI reload, range, projSpd, bulletType;
        private void Awake()
        {
            GlobalComponent.Common.Text.Item.WeaponRange.tReload = tReload;
            GlobalComponent.Common.Text.Item.WeaponRange.tRange = tRange;
            GlobalComponent.Common.Text.Item.WeaponRange.tProjSpd = tProjSpd;
            GlobalComponent.Common.Text.Item.WeaponRange.tBulletType = tBulletType;
        }

        public void OnItemInfoChanged(ItemBaseInfo _info)
        {
            if (_info == null)
            {
                gameObject.SetActive(false);
                return;
            }
            ItemWeaponInfo info = (ItemWeaponInfo)_info;
            reload.text = $"{info.timeReload}초";
            range.text = info.range.ToString();
            projSpd.text = info.spd.ToString();
            bulletType.text = $"{HoverItemInfoBulletController.ConvertBulletTypeToString(info.bulletType)}";
            gameObject.SetActive(true);
        }
    }
}
