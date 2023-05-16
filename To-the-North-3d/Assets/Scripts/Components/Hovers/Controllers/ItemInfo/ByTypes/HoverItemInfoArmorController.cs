using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoArmorController : MonoBehaviour, IHoverItemInfo
    {
        [SerializeField]
        private TextMeshProUGUI tPenatration, tImpact, tHeat;
        [SerializeField]
        private TextMeshProUGUI penatration, impact, heat;
        private void Awake()
        {
            GlobalComponent.Common.Text.Item.Armor.tPenatration = tPenatration;
            GlobalComponent.Common.Text.Item.Armor.tImpact = tImpact;
            GlobalComponent.Common.Text.Item.Armor.tHeat = tHeat;
        }

        public void OnItemInfoChanged(ItemBaseInfo _info)
        {
            if (_info == null)
            {
                gameObject.SetActive(false);
                return;
            }
            ItemArmorInfo info = (ItemArmorInfo)_info;
            penatration.text = info.ClassPenetration.ToString();
            impact.text = info.ClassImpact.ToString();
            heat.text = info.ClassHeat.ToString();
            gameObject.SetActive(true);
        }
    }
}
