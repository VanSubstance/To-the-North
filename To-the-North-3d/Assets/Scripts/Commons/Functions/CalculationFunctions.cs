using UnityEngine;

namespace Assets.Scripts.Commons.Functions
{
    public static class CalculationFunctions
    {
        /// <summary>
        /// 0도 위치에서부터 angleDegree 기준 방향 벡터 (Vector3) (정규화 O)
        /// </summary>
        /// <param name="angleDegrees"></param>
        /// <returns></returns>
        public static Vector3 DirFromAngle(float angleDegrees)
        {
            return new Vector3(Mathf.Cos((angleDegrees) * Mathf.Deg2Rad), 0, Mathf.Sin((angleDegrees) * Mathf.Deg2Rad)).normalized;
        }
        /// <summary>
        /// 벡터의 방향으로부터 각도 추출 (Vector3)
        /// Vector3.right 기준 반시계 방향
        /// 0 ~ 360
        /// </summary>
        /// <param name="angleDegrees"></param>
        /// <returns></returns>
        public static float AngleFromDir(Vector2 dir)
        {
            //if (dir.y == 0) return dir.x > 0 ? 0 : 180;
            return Quaternion.FromToRotation(Vector3.right, dir).eulerAngles.z;
        }

        /// <summary>
        /// 방향 벡터를 특정 각도만큼 회전한 방향벡터 반환 (정규화 O)
        /// </summary>
        /// <param name="dirVector">방향 벡터</param>
        /// <param name="degreeToRotate">회적할 각도</param>
        /// <returns></returns>
        public static Vector2 GetRotatedVector2(Vector2 dirVector, float degreeToRotate)
        {
            return DirFromAngle(AngleFromDir(dirVector) + degreeToRotate);
        }

        /// <summary>
        /// 대상 위치가 이동 불가한 위치인지 = 장애물 내부에 존재하는지 판단 후, 이동 가능한 위치로 보정하여 반환하는 함수
        /// </summary>
        /// <param name="moverPos">움직일 대상의 현재 위치</param>
        /// <param name="originPos">테스트 위치</param>
        /// <returns></returns>
        public static Vector3 GetDetouredPositionIfInCollider(Vector3 moverPos, Vector3 originPos)
        {
            Collider[] cols = Physics.OverlapSphere(originPos, 0.1f, GlobalStatus.Constant.obstacleMask);
            if (cols.Length > 0 && cols[0] != null)
            {
                Vector3 t = cols[0].ClosestPointOnBounds(moverPos);
                return new Vector3(t.x, originPos.y, t.z);
            }
            return originPos;
        }
    }
}
