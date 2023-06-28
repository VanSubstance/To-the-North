using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Users;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Creatures
{
    internal class MonsterBaseController : AIBaseController
    {
        [SerializeField]
        private Transform lootingPrefab;
        public override void DetectFull()
        {
            if (BushHidden && !BushHidden.Equals(UserBaseController.Instance.BushHidden))
            {
                ChangeVisualOpacity(.2f);
                return;
            }
            ChangeVisualOpacity(1);
        }

        public override void DetectHalf()
        {
            if (BushHidden && !BushHidden.Equals(UserBaseController.Instance.BushHidden))
            {
                ChangeVisualOpacity(.2f);
                return;
            }
            ChangeVisualOpacity(.2f);
        }

        public override void DetectNone()
        {
            ChangeVisualOpacity(0);
        }

        public override void DetectSound(Vector3 _pos)
        {
            DetectPosition(_pos);
        }

        public override void OnDetectPosition(Vector3 targetPos)
        {
            DetectPosition(targetPos);
        }

        private void DetectPosition(Vector3 _pos)
        {
            foreach (AbsAIStatusController statusCtrl in statusCtrls)
            {
                statusCtrl.DetectPosition(_pos);
            }
        }

        public override void OnDetectUser(Transform userTf)
        {
            if (IsRunaway)
            {
                statusType = Interfaces.AIStatusType.Runaway;
            }
            else if (Info.IsActiveBehaviour)
            {
                statusType = Interfaces.AIStatusType.Combat;
            }
            else
            {

            }

            foreach (AbsAIStatusController statusCtrl in statusCtrls)
            {
                statusCtrl.DetectUser(userTf);
            }
        }

        public override void OnDied()
        {
            if (lootingPrefab != null)
            {
                lootingPrefab = Instantiate(lootingPrefab, GlobalComponent.Common.Parent.Event);
                lootingPrefab.position = transform.position;
            }
            Destroy(gameObject);
        }
    }
}
