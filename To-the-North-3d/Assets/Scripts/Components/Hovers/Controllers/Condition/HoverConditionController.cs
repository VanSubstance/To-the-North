using Assets.Scripts.Commons;
using Assets.Scripts.Users;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Components.Hovers
{
    public class HoverConditionController : MouseTrackController, IControllByKey
    {
        [SerializeField]
        private TextMeshProUGUI title, desc;

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
            ConditionInfo cur = ConditionInfo.infos[type];
            title.text = cur.title;
            desc.text = cur.description;
            gameObject.SetActive(true);
        }

        public void OnHoverExit()
        {
            gameObject.SetActive(false);
            title.text = string.Empty;
            desc.text = string.Empty;
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
