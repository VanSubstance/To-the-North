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
                Debug.Log("응시중 ...");
                mover.TargetGaze = (Vector3)target;
                return;
            }
            // 타겟이 없음
            if (timeLeft <= 0)
            {
                timeLeft = CommonFunction.GetRandomFloat(2.5f, 4);
                if (CommonFunction.GetRandomFloat(0, 2) > 1)
                {
                    // 50% 확률로 이동
                    // 무작위로 좌표 부여
                    float temp = mover.Agent.stoppingDistance + 5f;
                    mover.TargetMove = new Vector3(
                        mover.transform.position.x + CommonFunction.GetRandomFloat(-temp, temp),
                        mover.transform.position.y,
                        mover.transform.position.z + CommonFunction.GetRandomFloat(-temp, temp)
                        );
                    mover.TargetGaze = mover.TargetMove - mover.transform.position;
                }
                else
                {
                    mover.TargetGaze = new Vector3(
                        CommonFunction.GetRandomFloat(-3f, 3f),
                        0,
                        CommonFunction.GetRandomFloat(-3f, 3f)
                        );
                }
                return;
            }
            timeLeft -= Time.deltaTime;
        }
    }
}
