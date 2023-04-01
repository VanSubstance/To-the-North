using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Items.Abstracts;
using UnityEngine;

namespace Assets.Scripts.Items.Controllers
{
    class ItemMaterialController : AItemBaseController<ItemMaterialInfo>
    {
        public override void ExecuteDoubleClick()
        {
            Debug.Log("재료 아이템 더블클릭!");
        }

        protected override bool CheckItemTag()
        {
            Debug.Log("재료 아이템 이동:: 칸에 놓을 수 있는지 체크!");
            return true;
        }

        protected override void InitExtraContent(ItemMaterialInfo content)
        {
            throw new System.NotImplementedException();
        }
    }
}
