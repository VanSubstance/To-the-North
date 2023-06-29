using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Users;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Creatures
{
    public class NpcBaseController : AIBaseController
    {
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
        }

        public override void OnDied()
        {
        }
    }
}
