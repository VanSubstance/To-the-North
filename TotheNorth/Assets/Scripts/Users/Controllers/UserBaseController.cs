using System.Collections.Generic;
using Assets.Scripts.Battles;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Users
{
    public class UserBaseController : MonoBehaviour, ICreatureBattle
    {
        public Vector3 position
        {
            get => transform.position;
        }
        public float x
        {
            get => transform.position.x;
        }
        public float y
        {
            get => transform.position.y;
        }

        private Dictionary<EquipBodyType, IItemEquipable> equipableBodies = new Dictionary<EquipBodyType, IItemEquipable>();

        private float tickHealthCondition;

        private void Awake()
        {
            Transform temp = transform.Find("Hits");
            equipableBodies[EquipBodyType.Helmat] = temp.GetChild(0).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Mask] = temp.GetChild(1).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Head] = temp.GetChild(2).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Body] = temp.GetChild(3).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Leg] = temp.GetChild(4).GetChild(0).GetComponent<ItemArmorController>();
            temp = transform.Find("Hands");
            equipableBodies[EquipBodyType.Right] = temp.GetChild(0).GetChild(0).GetComponent<ItemWeaponController>();
            equipableBodies[EquipBodyType.Left] = temp.GetChild(1).GetChild(0).GetComponent<ItemWeaponController>();
            tickHealthCondition = 0;
        }

        private void Update()
        {
            tickHealthCondition += Time.deltaTime;
            if (tickHealthCondition > 1)
            {
                tickHealthCondition = 0;
                TickHealthCondition();
            }
        }

        private void TickHealthCondition()
        {
            InGameStatus.User.status.hungerBar.LiveInfo = -.2f;
            InGameStatus.User.status.thirstBar.LiveInfo = -.2f;
            // 온도의 경우, 주변 환경에 따라서 오르거나 내리거나 유지되어야 할 듯
            InGameStatus.User.status.temperatureBar.LiveInfo = +.5f;
        }

        /// <summary>
        /// 장비 바꿔끼기 함수
        /// </summary>
        /// <param name="targetType">목표 부위</param>
        /// <param name="itemInfo">장착할 아이템 정보</param>
        public void ChangeEquipment(EquipBodyType targetType, ItemEquipmentInfo itemInfo)
        {
            equipableBodies[targetType].ChangeEquipment(itemInfo);
        }

        public void OnHit(EquipBodyType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, int[] damage, Vector3 hitDir)
        {
            switch (partType)
            {
                case EquipBodyType.Helmat:
                    break;
                case EquipBodyType.Mask:
                    break;
                case EquipBodyType.Head:
                    break;
                case EquipBodyType.Body:
                    break;
                case EquipBodyType.Leg:
                    break;
            }
            // 상태 이상 부여 심사

            // 테스트 효과 활성화
            OccurCondition(ConditionType.Test);

            if (damage[1] > 0)
            {
                // 관통당함
                // 심각한 출혈 심사
                if (Random.Range(0f, 1f) <= Mathf.Pow(damage[1] / 100f, 1.5f))
                {
                    // 심각한 출혈 발생
                    OccurCondition(ConditionType.Bleeding_Heavy);
                }
            }

            if (damage[2] > 0)
            {
                // 충격 데미지
                // 얉은 출혈 심사
                if (Random.Range(0f, 1f) <= damage[2] / 100f)
                {
                    // 얕은 출혈 발생
                    OccurCondition(ConditionType.Bleeding_Light);
                }
                // 골절 심사
                if (Random.Range(0f, 1f) <= Mathf.Pow(damage[1] / 100f, 1.7f))
                {
                    // 골절 발생
                    OccurCondition(ConditionType.Fracture);
                }
            }

            // 화면 피격 이벤트 처리
            CommonGameManager.Instance.OnHit(CalculationFunctions.AngleFromDir(hitDir), damage);
            return;
        }

        /// <summary>
        /// 상태 이상 발생 함수
        /// </summary>
        /// <param name="targetCondition">발생할 상태 이상</param>
        private void OccurCondition(ConditionType targetCondition)
        {
            InGameStatus.User.conditions[targetCondition]++;
            ConditionManager.Instance.AwakeCondition(targetCondition);
        }
    }
}
