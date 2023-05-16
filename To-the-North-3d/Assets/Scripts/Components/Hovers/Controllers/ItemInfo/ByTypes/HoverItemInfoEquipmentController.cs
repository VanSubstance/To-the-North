using Assets.Scripts.Components.Progress;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoEquipmentController : MonoBehaviour, IHoverItemInfo
    {
        [SerializeField]
        private BarBaseController durabilityBar;
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
            durabilityBar.SetValue(1, info.Durability);
            gameObject.SetActive(true);
        }
    }
}
