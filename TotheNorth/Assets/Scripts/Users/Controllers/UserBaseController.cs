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

        public void OnHit(EquipBodyType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, Vector3 hitDir)
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

            // 화면 피격 이벤트 처리
            CommonGameManager.Instance.OnHit(CalculationFunctions.AngleFromDir(hitDir), 8);

            // 계산 처리
            InGameStatus.User.status.hpBar.LiveInfo = -10;
            if (InGameStatus.User.status.hpBar.LiveInfo <= 0)
            {
                //InGameStatus.User.isPause = true;
            }
            return;
        }
    }
}
