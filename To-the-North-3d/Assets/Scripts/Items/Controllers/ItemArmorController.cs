using System;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemArmorController : MonoBehaviour, IItemEquipable
    {
        [SerializeField]
        private ItemArmorInfo info;
        [SerializeField]
        private bool isVisibleWithoutInfo = true;
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
                    if (info.equipPartType.Equals(EquipBodyType.Body))
                    {
                        // 몸통일 경우 4개를 불러와야 함
                        // 0: 몸통, 1: 골반, 2: 앞다리, 3: 뒷다리
                        Sprite[] t = GlobalComponent.Asset.GetImageForBody(info);
                        sprite.sprite = t[0];
                        pelvis.sprite = t[1];
                        legF.sprite = t[2];
                        legB.sprite = t[3];

                    } else
                    {
                        sprite.sprite = GlobalComponent.Asset.GetImage(info);
                    }
                }
            }
        }
        [SerializeField]
        private SpriteRenderer sprite;
        private Sprite def;

        [SerializeField]
        private SpriteRenderer pelvis, legF, legB;

        private void Awake()
        {
            def = isVisibleWithoutInfo ? sprite.sprite : null;
            sprite.sprite = def;
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
