using UnityEngine;
namespace Assets.Scripts.Commons
{
    public class UIManager : MonoBehaviour
    {
        public bool isInit = false;
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
    }
}
