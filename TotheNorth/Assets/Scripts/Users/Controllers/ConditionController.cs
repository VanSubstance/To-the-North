using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts.Commons.Constants;

namespace Assets.Scripts.Users
{
    public class ConditionController : MonoBehaviour
    {
        private ConditionType targetType;
        private TextMeshProUGUI count;
        private Transform sleepTf;
        private void Awake()
        {
            count = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (InGameStatus.User.conditions[targetType] == 0)
            {
                gameObject.SetActive(false);
                transform.SetParent(sleepTf, false);
            }
            if (targetType != ConditionType.None)
            {
                count.text = InGameStatus.User.conditions[targetType].ToString();
            }
        }

        public void InitCondition(ConditionType _targetType, Transform _sleepTf)
        {
            sleepTf = _sleepTf;
            targetType = _targetType;
            GetComponent<Image>().sprite = Resources.Load<Sprite>(GlobalComponent.Path.Image.Condition(targetType));
        }
    }
}
