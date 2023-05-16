using Assets.Scripts.Commons;
using Assets.Scripts.Users;
using UnityEngine;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverConditionController : MouseTrackController, IControllByKey
    {
        [SerializeField]
        private ConditionInfo[] conditionInfos;

        private bool isOccupied = false;

        private static HoverConditionController _instance;
        public static HoverConditionController Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(HoverConditionController)) as HoverConditionController;
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
            GetComponent<Canvas>().sortingLayerName = "UI Covering Map";
            gameObject.SetActive(false);
            isOccupied = false;
        }

        protected new void Update()
        {
            base.Update();
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
        public void OnHoverEnter(ConditionType type)
        {
            if (isOccupied) return;
            isOccupied = true;
            base.TracksMouse();
            //Debug.Log("호버링 아이템 정보 할당!! " + _info);
            switch (type)
            {
                case ConditionType.None:
                    break;
                case ConditionType.Bleeding_Light:
                    break;
                case ConditionType.Bleeding_Heavy:
                    break;
                case ConditionType.Fracture:
                    break;
                case ConditionType.Pain:
                    break;
                case ConditionType.Dizziness:
                    break;
                case ConditionType.Infection:
                    break;
                case ConditionType.Hunger:
                    break;
                case ConditionType.Thirst:
                    break;
                case ConditionType.Exhaust:
                    break;
                case ConditionType.Hot:
                    break;
                case ConditionType.Cold:
                    break;
            }
            gameObject.SetActive(true);
        }

        public void OnHoverExit()
        {
            gameObject.SetActive(false);
            isOccupied = false;
        }

        public void ControllByKey(int purpose)
        {
            OnHoverExit();
        }

        public void OnOpen()
        {
        }

        public void OnClose()
        {
        }
    }
}
