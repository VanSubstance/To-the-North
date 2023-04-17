using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Popups
{
    public class HoverItemInfoArmorController : MonoBehaviour, IHoverItemInfo
    {
        [SerializeField]
        TextMeshProUGUI tPenatration, tImpact, tHeat;
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
            tPenatration.text = info.ClassPenetration.ToString();
            tImpact.text = info.ClassImpact.ToString();
            tHeat.text = info.ClassHeat.ToString();
            gameObject.SetActive(true);
        }
    }
}
