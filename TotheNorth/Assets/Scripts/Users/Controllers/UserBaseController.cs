using Assets.Scripts.Battles;
using Assets.Scripts.Commons.Constants;
using UnityEngine;

namespace Assets.Scripts.Users
{
    internal class UserBaseController : MonoBehaviour, ICreatureBattle
    {
        public void OnHit(PartType partType, ProjectileInfo _info, Vector3 hitPos)
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
            InGameStatus.User.status.hpBar.LiveInfo = -10;
            if (InGameStatus.User.status.hpBar.LiveInfo <= 0)
            {
                //InGameStatus.User.isPause = true;
            }
            return;
        }
    }
}
