using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures.AI.Status
{
    public class CombatStatus : IAIStatus
    {
        private float timeLeft = -1, cntSearch = 3;
        public void UpdateAction(AIBaseController mover, Vector3? target)
        {
            mover.Agent.isStopped = false;
            if (target != null)
            {
                // 타겟 있음 = 라이브 상태
                timeLeft = -1;
                cntSearch = 3;
                mover.Agent.stoppingDistance = mover.WeaponRange / 2;
                if (mover.Info.IsRunAway)
                {
                    // 도주
                    mover.TargetMove = (mover.transform.position - (Vector3)target) + mover.transform.position;
                    return;
                }
                mover.TargetMove = ((Vector3)target);
                if (!mover.CheckAim())
                {
                }
            }
            else
            {
                if (!mover.Agent.isStopped)
                {
                    return;
                }
                if (cntSearch <= 0)
                {
                    // 종료 = AI 대기 상태로 변경
                    mover.AiStatus = new IdleStatus();
                    return;
                }
                if (timeLeft <= 0)
                {
                    cntSearch--;
                    timeLeft = CommonFunction.GetRandomFloat(2.5f, 4);
                    // 무작위로 좌표 부여
                    float temp = mover.WeaponRange + 5f;
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
}
