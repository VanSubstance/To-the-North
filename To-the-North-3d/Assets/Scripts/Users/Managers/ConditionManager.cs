using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Users
{
    internal class ConditionManager : MonoBehaviour
    {
        [SerializeField]
        private Transform controllerPrefab;
        private Transform sleepTf, visualizationTf;
        private Dictionary<ConditionType, ConditionController> controllers = new Dictionary<ConditionType, ConditionController>();
        private Dictionary<ConditionType, float?> timers = new Dictionary<ConditionType, float?>();

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
            Init();
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

            StartCoroutine(CoroutineTimerCondition());
        }

        public void AwakeCondition(ConditionType targetType)
        {
            controllers[targetType].transform.SetParent(visualizationTf, false);
            controllers[targetType].gameObject.SetActive(true);
            timers[targetType] = 0;
        }

        public void AsleepCondition(ConditionType targetType)
        {
            controllers[targetType].transform.SetParent(sleepTf, false);
            controllers[targetType].gameObject.SetActive(false);
            timers[targetType] = null;
        }

        private void Init()
        {
            if (InGameStatus.User.conditions != null) return;
            InGameStatus.User.conditions = new();
            foreach (ConditionType type in System.Enum.GetValues(typeof(ConditionType)))
            {
                if (type == ConditionType.None) continue;
                InGameStatus.User.conditions[type] = 0;
            }
        }

        /// <summary>
        /// 상태이상들 타이머
        /// </summary>
        /// <returns></returns>
        private IEnumerator CoroutineTimerCondition()
        {
            while (true)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                foreach (ConditionType type in System.Enum.GetValues(typeof(ConditionType)))
                {
                    if (type == ConditionType.None) continue;
                    try
                    {
                        if (timers[type] == null) continue;
                        timers[type] += Time.deltaTime;
                    } catch (KeyNotFoundException)
                    {
                        timers[type] = 0;
                    }
                }

                // 지속 시간 관련 발생해야 하는 상태 이상 관측

                // 감염
                if (
                    (timers[ConditionType.Bleeding_Light] != null && timers[ConditionType.Bleeding_Light] > 30) ||
                    (timers[ConditionType.Bleeding_Heavy] != null && timers[ConditionType.Bleeding_Heavy] > 15)
                    )
                {
                    // 감염 기본 조건 달성
                    if (timers[ConditionType.Infection] == null ||
                        timers[ConditionType.Infection] > 20)
                    {
                        // 감염 발생 후 20초 경과 or 최초 감염 발생
                        UserBaseController.Instance.OccurCondition(ConditionType.Infection);
                        timers[ConditionType.Infection] = 0;
                    }
                }
            }
        }
    }
}
