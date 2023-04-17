using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Components.Popups
{
    public class HoverItemInfoEquipmentController : MonoBehaviour, IHoverItemInfo
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
            ItemEquipmentInfo info = (ItemEquipmentInfo)_info;
            gameObject.SetActive(true);
        }
    }
}
