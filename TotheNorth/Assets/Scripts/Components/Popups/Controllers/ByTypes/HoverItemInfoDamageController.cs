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
            if (_info is ItemWeaponInfo info)
            {
                pwPene.text = info.PowerPenetration.ToString();
                pwImp.text = info.PowerImpact.ToString();
                pwKnock.text = info.PowerKnockback.ToString();
                dmgPene.text = info.damagePenetration.ToString();
                dmgImp.text = info.damageImpact.ToString();
            }
            else if (_info is ItemBulletInfo bulletInfo)
            {
                pwPene.text = bulletInfo.powerPenetration.ToString();
                pwImp.text = bulletInfo.powerImpact.ToString();
                pwKnock.text = bulletInfo.powerKnockback.ToString();
                dmgPene.text = bulletInfo.damagePenetration.ToString();
                dmgImp.text = bulletInfo.damageImpact.ToString();
            }
            gameObject.SetActive(true);
        }
    }
}
