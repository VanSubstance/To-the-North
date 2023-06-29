using UnityEngine;

namespace Assets.Scripts.Creatures.Detections
{
    public class DetectionController
    {
        /// <summary>
        /// 조건 시야 내 목표 타겟 좌표 반환 (절대 좌표)
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="distance"></param>
        /// <param name="targetLayer"></param>
        /// <param name="blockLayer"></param>
        /// <returns></returns>
        public static bool TryGetTarget(Vector3 origin, float distance, LayerMask targetLayer, LayerMask blockLayer, out Transform result)
        {
            Collider[] res;
            if ((res = Physics.OverlapSphere(origin, distance, targetLayer)).Length > 0)
            {
                // 1차 식별 = 비거리 안에 유저가 존재
                if (!Physics.Raycast(origin, res[0].transform.position, out RaycastHit hit, (origin - res[0].transform.position).magnitude, blockLayer))
                {
                    // 2차 식별 = 사이에 장애물 없음
                    result = res[0].transform;
                    return true;
                }
            }
            result = null;
            return false;
        }
    }
}
