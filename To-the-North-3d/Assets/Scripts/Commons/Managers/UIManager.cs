using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Commons
{
    public class UIManager : MonoBehaviour
    {
        public bool isInit = false;
        private List<KeyToggleManager> keyToggleManagers = new List<KeyToggleManager>();

        public bool IsAllClosed
        {
            get
            {
                return keyToggleManagers.Where((km) => km.IsOpen).ToArray().Length == 0;
            }
        }
        public bool IsClosedInForce = false;

        private static UIManager _instance;
        // 인스턴스에 접근하기 위한 프로퍼티
        public static UIManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(UIManager)) as UIManager;
                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        private Camera ConnectedCamera
        {
            get
            {
                return GetComponent<Canvas>().worldCamera;
            }
            set
            {
                GetComponent<Canvas>().worldCamera = value;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (ConnectedCamera == null)
            {
                ConnectedCamera = Camera.main;
            }
            TrackKeyManagers();
        }

        public void AddKeyToggleManager(KeyCode _keyToToggle, IControllByKey objectToControll)
        {
            KeyToggleManager newKM = gameObject.AddComponent<KeyToggleManager>();
            newKM.InitContent(_keyToToggle, objectToControll);
            keyToggleManagers.Add(newKM);
        }

        private void TrackKeyManagers()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!IsAllClosed)
                {
                    keyToggleManagers.ForEach((km) =>
                    {
                        km.CloseInForce();
                    });
                    IsClosedInForce = true;
                    return;
                }
            }
        }
    }
}
