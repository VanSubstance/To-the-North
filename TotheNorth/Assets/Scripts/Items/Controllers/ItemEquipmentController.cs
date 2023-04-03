using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Items.Abstracts;
using UnityEngine;

namespace Assets.Scripts.Items.Controllers
{
    class ItemEquipmentController : AItemBaseController<ItemEquipmentInfo>
    {
        public override void ExecuteDoubleClick()
        {
            Debug.Log("장비 아이템 더블클릭!");
        }

        protected override bool CheckItemTag()
        {
            Debug.Log("장비 아이템 이동:: 칸에 놓을 수 있는지 체크!");
            return true;
        }

        protected override void InitExtraContent(ItemEquipmentInfo content)
        {
            throw new System.NotImplementedException();
        }
    }
}