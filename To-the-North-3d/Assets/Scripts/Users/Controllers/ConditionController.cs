using Assets.Scripts.Components.Hovers;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Users
{
    public class ConditionController : MonoBehaviour
    {
        private ConditionType targetType;
        private TextMeshProUGUI count;
        private Transform sleepTf;
        private ConditionControl conditionControl;
        private int tickAmount;
        private float tickLive = 0;

        private static readonly float tickTime = 2;

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
                ApplyEffects();
            }
        }

        public void InitCondition(ConditionType _targetType, Transform _sleepTf)
        {
            tickLive = 0;
            sleepTf = _sleepTf;
            targetType = _targetType;
            GetComponent<Image>().sprite = Resources.Load<Sprite>(GlobalComponent.Path.Image.Condition(targetType));
            try
            {
                tickAmount = ConditionConstraint.Tick.TickAmountForCondition[targetType];
            }
            catch (KeyNotFoundException)
            {
                // 해당 상태 이상 틱 효과 없음
                tickAmount = 0;
            }
            // 1. 틱 데미지가 있는 상태이상?
            if (ConditionConstraint.Tick.Damage.Contains(targetType))
            {
                conditionControl += InGameStatus.User.status.ApplyDamage;
            }

            // 2. 화면 진통이 있는 상태이상?
            if (ConditionConstraint.Tick.Vibrating.Contains(targetType))
            {
                conditionControl += ScreenVibrate;
            }

            // 3. 화면 붉어짐이 있는 상태 이상?
            if (ConditionConstraint.Tick.Blurred.Contains(targetType))
            {
                conditionControl += ScreenBlurred;
            }
        }

        /// <summary>
        /// 상태 이상에 따른 효과 적용 함수
        /// </summary>
        private void ApplyEffects()
        {
            tickLive += Time.deltaTime;
            if (tickLive >= tickTime)
            {
                tickLive = 0;
                try
                {
                    conditionControl.Invoke(tickAmount * InGameStatus.User.conditions[targetType]);
                }
                catch (System.NullReferenceException)
                {
                    // 해상 상태이상 틱 효과 없음
                }
            }
        }

        public void OnMouseEnter()
        {
            HoverConditionController.Instance.OnHoverEnter(targetType);
        }

        private void OnMouseExit()
        {
            HoverConditionController.Instance.OnHoverExit();
        }

        private void ScreenVibrate(float amount)
        {
            CommonGameManager.Instance.CameraHitController.OnHit(amount);
        }

        private void ScreenBlurred(float amount)
        {
            CommonGameManager.Instance.ScreenHitFilterController.OnHit(amount);
        }
    }
}
