using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoConsumableController : MonoBehaviour, IHoverItemInfo
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
            ItemConsumableInfo info = (ItemConsumableInfo)_info;
            gameObject.SetActive(true);
        }
    }
}
