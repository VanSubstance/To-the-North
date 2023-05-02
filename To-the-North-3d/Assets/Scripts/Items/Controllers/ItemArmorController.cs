using System;
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
                if (info == null)
                {
                    sprite.sprite = def;
                }
                else
                {
                    sprite.sprite = GlobalComponent.Asset.GetImage(info);
                }
            }
        }
        [SerializeField]
        private SpriteRenderer sprite;
        private Sprite def;

        private void Awake()
        {
            def = sprite.sprite;
            if (info)
            {
                Info = Instantiate(info);
            }
        }

        public void ChangeEquipment(ItemEquipmentInfo _info)
        {
            if (_info == null)
            {
                Info = null;
            }
            else
            {
                try
                {
                    Info = (ItemArmorInfo)_info;
                }
                catch (InvalidCastException)
                {
                    // 장비 정보가 오염됨
                }
            }
        }

        public bool IsEmpty() => Info == null;
    }
}
