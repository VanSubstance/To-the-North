using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoFoodController : MonoBehaviour, IHoverItemInfo
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
            ItemFoodInfo info = (ItemFoodInfo)_info;
            gameObject.SetActive(true);
        }
    }
}
