using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Components.Popups
{
    public class HoverItemInfoArmorController : MonoBehaviour, IHoverItemInfo
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
            ItemArmorInfo info = (ItemArmorInfo)_info;
            gameObject.SetActive(true);
        }
    }
}
