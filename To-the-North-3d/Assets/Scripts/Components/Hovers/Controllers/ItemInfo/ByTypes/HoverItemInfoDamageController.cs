using Assets.Scripts.Items;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverItemInfoDamageController : MonoBehaviour, IHoverItemInfo
    {
        [SerializeField]
        private TextMeshProUGUI tPwPene, tPwImp, tPwKnock, tDmgPene, tDmgImp;
        [SerializeField]
        private TextMeshProUGUI pwPene, pwImp, pwKnock, dmgPene, dmgImp;
        private void Awake()
        {
            GlobalComponent.Common.Text.Item.Damage.tPwPene = tPwPene;
            GlobalComponent.Common.Text.Item.Damage.tPwImp = tPwImp;
            GlobalComponent.Common.Text.Item.Damage.tPwKnock = tPwKnock;
            GlobalComponent.Common.Text.Item.Damage.tDmgPene = tDmgPene;
            GlobalComponent.Common.Text.Item.Damage.tDmgImp = tDmgImp;
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
