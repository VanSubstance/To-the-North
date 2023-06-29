using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures.AI.Status
{
    public class IdleStatus : IAIStatus
    {
        private float timeLeft = 0;
        public void UpdateAction(AIBaseController mover, Vector3? target)
        {
            if (target != null)
            {
                // 타겟이 있음 = 이동 중지 후 응시
                timeLeft = CommonFunction.GetRandomFloat(2.5f, 4);
                mover.Agent.isStopped = true;
                mover.Agent.stoppingDistance = mover.WeaponRange / 2;
                mover.SightDirection = ((Vector3)target).x - mover.transform.position.x > 0 ? 0 : 180;
                return;
            }
            if (!mover.Agent.isStopped)
            {
                return;
            }
            // 타겟이 없음
            if (timeLeft <= 0)
            {
                mover.Agent.isStopped = false;
                timeLeft = CommonFunction.GetRandomFloat(2.5f, 4);
                // 무작위로 좌표 부여
                float temp = mover.Agent.stoppingDistance + 5f;
                mover.TargetMove = new Vector3(
                    mover.transform.position.x + CommonFunction.GetRandomFloat(-temp, temp),
                    mover.transform.position.y,
                    mover.transform.position.z + CommonFunction.GetRandomFloat(-temp, temp)
                    );
                return;
            }
            timeLeft -= Time.deltaTime;
        }
    }
}
