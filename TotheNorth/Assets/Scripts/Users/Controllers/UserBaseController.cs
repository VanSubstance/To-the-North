using Assets.Scripts.Battles;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Users
{
    internal class UserBaseController : MonoBehaviour, ICreatureBattle
    {
        public void OnHit(PartType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, Vector3 hitDir)
        {
            switch (partType)
            {
                case PartType.Helmat:
                    break;
                case PartType.Mask:
                    break;
                case PartType.Head:
                    break;
                case PartType.Body:
                    break;
                case PartType.Leg:
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
