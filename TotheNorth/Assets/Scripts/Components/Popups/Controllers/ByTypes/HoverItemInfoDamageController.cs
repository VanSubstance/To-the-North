using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Popups
{
    public class HoverItemInfoDamageController : MonoBehaviour, IHoverItemInfo
    {
        [SerializeField]
        private TextMeshProUGUI pwPene, pwImp, pwKnock, dmgPene, dmgImp;
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
            pwPene.text = info.PowerPenetration.ToString();
            pwImp.text = info.PowerImpact.ToString();
            pwKnock.text = info.PowerKnockback.ToString();
            dmgPene.text = info.damagePenetration.ToString();
            dmgImp.text = info.damageImpact.ToString();
            gameObject.SetActive(true);
        }
    }
}
