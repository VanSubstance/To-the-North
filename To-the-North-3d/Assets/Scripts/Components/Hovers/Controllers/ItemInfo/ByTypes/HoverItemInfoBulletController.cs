using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoBulletController : MonoBehaviour, IHoverItemInfo
    {
        [SerializeField]
        private TextMeshProUGUI tBulletType, tAccelSpd;
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
            ItemBulletInfo info = (ItemBulletInfo)_info;
            tBulletType.text = ConvertBulletTypeToString(info.bulletType);
            tAccelSpd.text = info.powerSpd.ToString();
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
