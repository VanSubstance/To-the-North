using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Components.Popups
{
    public class HoverItemInfoWeaponController : MonoBehaviour, IHoverItemInfo
    {
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
            gameObject.SetActive(true);
        }
    }
}
