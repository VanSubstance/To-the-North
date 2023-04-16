using UnityEngine;
using static GlobalComponent.Common;

namespace Assets.Scripts.Components.Popups
{
    internal class HoverInfoController : MonoBehaviour
    {
        private bool isOccupied = false;
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
        }

        private void Update()
        {
            if (transform.GetSiblingIndex() != transform.parent.childCount - 1)
            {
                transform.SetAsLastSibling();
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
            Debug.Log("호버링 아이템 정보 할당!! " + _info);
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
            isOccupied = true;
            gameObject.SetActive(true);
        }

        public void OnHoverExit()
        {
            gameObject.SetActive(false);
            isOccupied = false;
        }
    }
}
