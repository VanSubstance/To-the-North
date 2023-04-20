using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemArmorController : MonoBehaviour, IItemEquipable
    {
        [SerializeField]
        private ItemArmorInfo info;
        public ItemArmorInfo Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
                sprite.sprite = GlobalComponent.Asset.GetImage(info);
            }
        }
        [SerializeField]
        private SpriteRenderer sprite;

        public void ChangeEquipment(ItemEquipmentInfo _info)
        {
            if (_info == null)
            {
                Info = null;
                sprite.sprite = null;
                Debug.Log("장비 뺌 or null 들어옴");
            }
            else
            {
                try
                {
                    Info = (ItemArmorInfo)_info;
                    Debug.Log("장비 착용함");
                }
                catch (InvalidCastException)
                {
                    // 장비 정보가 오염됨
                    Debug.Log("장비 정보가 오염 됨");
                }
            }
        }

        public bool IsEmpty() => Info == null;
    }
}
