using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoBulletController : MonoBehaviour, IHoverItemInfo
    {
        [SerializeField]
        private TextMeshProUGUI tBulletType, tAccelSpd;
        [SerializeField]
        private TextMeshProUGUI bulletType, accelSpd;
        private void Awake()
        {
            GlobalComponent.Common.Text.Item.Bullet.tBulletType = tBulletType;
            GlobalComponent.Common.Text.Item.Bullet.tAccelSpd = tAccelSpd;
        }

        public void OnItemInfoChanged(ItemBaseInfo _info)
        {
            if (_info == null)
            {
                gameObject.SetActive(false);
                return;
            }
            ItemBulletInfo info = (ItemBulletInfo)_info;
            bulletType.text = ConvertBulletTypeToString(info.bulletType);
            accelSpd.text = info.powerSpd.ToString();
            gameObject.SetActive(true);
        }

        public static string ConvertBulletTypeToString(ItemBulletType type)
        {
            switch (type)
            {
                case ItemBulletType.None:
                    return null;
                case ItemBulletType.Bullet_mm9:
                    return "9mm";
                case ItemBulletType.Arrow:
                    return "화살";
            }
            return null;
        }
    }
}
