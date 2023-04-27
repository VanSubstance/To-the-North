using Assets.Scripts.Components.Windows.Inventory;
using UnityEngine;

namespace Assets.Scripts.Components.Infos
{
    public class UIQuickController : MonoBehaviour
    {
        private bool isCtrlPressed;
        private QuickSlotController[] quicks;
        public QuickSlotController[] Quicks
        {
            get
            {
                return quicks;
            }
        }
        private void Awake()
        {
            isCtrlPressed = false;
            quicks = new QuickSlotController[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                quicks[i] = transform.GetChild(i).GetComponent<QuickSlotController>();
            }
        }

        private static UIQuickController _instance;
        public static UIQuickController Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(UIQuickController)) as UIQuickController;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl)) return;
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // 1번 핫키 사용
                quicks[0].Use();
                return;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // 1번 핫키 사용
                quicks[1].Use();
                return;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // 1번 핫키 사용
                quicks[2].Use();
                return;
            }
        }
    }
}
