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
        }

        private SpriteRenderer sprite;
        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>();
        }

        public void ChangeEquipment(ItemEquipmentInfo _info)
        {
            try
            {
                info = (ItemArmorInfo)_info;
                sprite.sprite = Resources.Load<Sprite>(GlobalComponent.Path.GetImagePath(info));
            }
            catch (InvalidCastException)
            {
                // 장비 정보가 오염됨
            }
        }

        public bool IsEmpty() => info == null;
    }
}
