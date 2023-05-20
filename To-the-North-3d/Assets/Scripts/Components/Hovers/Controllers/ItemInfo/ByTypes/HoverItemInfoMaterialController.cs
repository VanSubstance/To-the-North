using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoMaterialController : MonoBehaviour, IHoverItemInfo
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
            ItemMaterialInfo info = (ItemMaterialInfo)_info;
            gameObject.SetActive(true);
        }
    }
}
