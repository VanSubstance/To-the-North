using Assets.Scripts.Commons;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Components.Popups
{
    internal class HoverItemInfoContainerController : MouseTrackController, IControllByKey
    {
        private bool isOccupied = false;
        private int prevH;
        private HoveringItemInfoChangeControl itemInfoControl;
        private IHoverItemInfo[] infoDisplayFunctions;
        private static HoverItemInfoContainerController _instance;
        public static HoverItemInfoContainerController Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(HoverItemInfoContainerController)) as HoverItemInfoContainerController;
                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            gameObject.SetActive(false);
            isOccupied = false;
            prevH = 0;
            int c = System.Enum.GetNames(typeof(DisplayIndex)).Length;
            infoDisplayFunctions = new IHoverItemInfo[c];
            for (int i = 0; i < c; i++)
            {
                infoDisplayFunctions[i] = transform.GetChild(i).GetComponent<IHoverItemInfo>();
            }
            itemInfoControl = null;
        }

        protected new void Update()
        {
            base.Update();
            if (transform.GetSiblingIndex() != transform.parent.childCount - 1)
            {
                transform.SetAsLastSibling();
            }
            ResizeByChildren();
        }

        private void ResizeByChildren()
        {
            float h = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (!transform.GetChild(i).gameObject.activeSelf) continue;
                h += transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 10;
            }
            if ((int)h != prevH)
            {
                float w = transform.GetComponent<RectTransform>().sizeDelta.x;
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h + 10);
            }
            prevH = (int)h;
        }

        private void InitItemInfo(ItemBaseInfo info)
        {
            // 델리게이트 초기화
            itemInfoControl += (info) => infoDisplayFunctions[(int)DisplayIndex.Base].OnItemInfoChanged(info); ;
            if (info is ItemEquipmentInfo)
            {
                itemInfoControl += (info) => infoDisplayFunctions[(int)DisplayIndex.Equipment].OnItemInfoChanged(info);
                if (info is ItemWeaponInfo)
                {
                    itemInfoControl += (info) => infoDisplayFunctions[(int)DisplayIndex.Damage].OnItemInfoChanged(info);
                    itemInfoControl += (info) => infoDisplayFunctions[(int)DisplayIndex.Weapon].OnItemInfoChanged(info);
                    if (((ItemWeaponInfo)info).bulletType != ItemBulletType.None)
                    {
                        itemInfoControl += (info) => infoDisplayFunctions[(int)DisplayIndex.WeaponRange].OnItemInfoChanged(info);
                    }
                }
                else if (info is ItemArmorInfo)
                {
                    itemInfoControl += (info) => infoDisplayFunctions[(int)DisplayIndex.Armor].OnItemInfoChanged(info);
                }
                else if (info is ItemMagazineInfo)
                {
                    itemInfoControl += (info) => infoDisplayFunctions[(int)DisplayIndex.Magazine].OnItemInfoChanged(info);
                }
                return;
            }
            if (info is ItemMaterialInfo)
            {
                itemInfoControl += (info) => infoDisplayFunctions[(int)DisplayIndex.Material].OnItemInfoChanged(info);
                return;
            }
            if (info is ItemConsumableInfo)
            {
                itemInfoControl += (info) => infoDisplayFunctions[(int)DisplayIndex.Consumable].OnItemInfoChanged(info);
                if (info is ItemFoodInfo)
                {
                    itemInfoControl += (info) => infoDisplayFunctions[(int)DisplayIndex.Food].OnItemInfoChanged(info);
                }
                else if (info is ItemBulletInfo)
                {
                    itemInfoControl += (info) => infoDisplayFunctions[(int)DisplayIndex.Bullet].OnItemInfoChanged(info);
                }
                return;
            }
        }

        /// <summary>
        /// 현재 호버링 중인 = 띄워줄 아이템 정보 할당 함수
        /// 만약 이미 특정 정보를 띄우고 있다 = 바로 종료
        /// </summary>
        /// <param name="_info">듸워줄 아이템 정보</param>
        public void OnHoverEnter(ItemBaseInfo _info)
        {
            if (isOccupied) return;
            isOccupied = true;
            base.TracksMouse();
            //Debug.Log("호버링 아이템 정보 할당!! " + _info);
            InitItemInfo(_info);
            itemInfoControl.Invoke(_info);
            string[] t = _info.GetType().ToString().Split(".");
            string comp = t[t.Length - 1];
            switch (comp)
            {
                case "ItemWeaponInfo":
                    break;
                case "ItemArmorInfo":
                    break;
                case "ItemFoodInfo":
                    break;
                case "ItemBulletInfo":
                    break;
                case "ItemMaterialInfo":
                    break;
            }
            gameObject.SetActive(true);
        }

        public void OnHoverExit()
        {
            gameObject.SetActive(false);
            if (itemInfoControl != null)
            {
                itemInfoControl.Invoke(null);
            }
            itemInfoControl = null;
            isOccupied = false;
        }

        public void ControllByKey(int purpose)
        {
            OnHoverExit();
        }

        private enum DisplayIndex
        {
            Base,
            Equipment,
            Armor,
            Damage,
            Weapon,
            WeaponRange,
            Magazine,
            Material,
            Consumable,
            Food,
            Bullet,
        }
    }
}
