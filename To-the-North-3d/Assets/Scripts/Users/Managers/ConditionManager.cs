using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using UnityEngine;

namespace Assets.Scripts.Users
{
    internal class ConditionManager : MonoBehaviour
    {
        [SerializeField]
        private Transform controllerPrefab;
        private Transform sleepTf, visualizationTf;
        private Dictionary<ConditionType, ConditionController> controllers = new Dictionary<ConditionType, ConditionController>();

        // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
        private static ConditionManager _instance;
        // 인스턴스에 접근하기 위한 프로퍼티
        public static ConditionManager Instance
        {
            get
            {
                // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(ConditionManager)) as ConditionManager;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }
        private void Awake()
        {
            visualizationTf = transform.GetChild(0);
            sleepTf = transform.GetChild(1);
            foreach (ConditionType type in System.Enum.GetValues(typeof(ConditionType)))
            {
                if (type == ConditionType.None) continue;
                controllers[type] = Instantiate(controllerPrefab, sleepTf).GetComponent<ConditionController>();
                controllers[type].InitCondition(type, sleepTf);
            }

            foreach (ConditionType type in System.Enum.GetValues(typeof(ConditionType)))
            {
                if (type == ConditionType.None) continue;
                if (InGameStatus.User.conditions[type] > 0)
                {
                    AwakeCondition(type);
                }
            }
        }

        public void AwakeCondition(ConditionType targetType)
        {
            controllers[targetType].transform.SetParent(visualizationTf, false);
            controllers[targetType].gameObject.SetActive(true);
        }
    }
}
