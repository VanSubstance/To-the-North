using Assets.Scripts.Commons;
using UnityEngine;

namespace Assets.Scripts.Components.Popups
{
    internal class HoverInfoController : MouseTrackController
    {
        private bool isOccupied = false;
        private int prevH;
        private static HoverInfoController _instance;
        public static HoverInfoController Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(HoverInfoController)) as HoverInfoController;

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
                h += transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 10;
            }
            if ((int)h != prevH)
            {
                float w = transform.GetComponent<RectTransform>().sizeDelta.x;
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h + 10);
            }
            prevH = (int)h;
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
            isOccupied = false;
        }
    }
}
