using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoMagazineController : MonoBehaviour, IHoverItemInfo
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
            
            ItemMagazineInfo info = (ItemMagazineInfo)_info;
            gameObject.SetActive(true);
        }
    }
}
