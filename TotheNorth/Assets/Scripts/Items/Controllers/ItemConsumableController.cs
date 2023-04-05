using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Items.Abstracts;
using UnityEngine;

namespace Assets.Scripts.Items.Controllers
{
    class ItemConsumableController : AItemBaseController<ItemConsumableInfo>
    {
        public override void ExecuteDoubleClick()
        {
            Debug.Log("소모성 아이템 더블클릭!");
        }

        protected override bool CheckItemTag(string slotType)
        {
            if (slotType == "Inventory" || slotType == "QuickSlot" || slotType == "Rooting")
            {
                return true;
            }
            return false;
        }

        protected override void InitExtraContent(ItemConsumableInfo content)
        {
            throw new System.NotImplementedException();
        }
    }
}
