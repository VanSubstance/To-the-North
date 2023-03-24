using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Commons.Functions
{
    public static class CalculationFunctions
    {
        /// <summary>
        /// 0도 위치에서부터 angleDegree 기준 방향 벡터 (Vector3)
        /// </summary>
        /// <param name="angleDegrees"></param>
        /// <returns></returns>
        public static Vector3 DirFromAngle(float angleDegrees)
        {
            return new Vector3(Mathf.Cos((angleDegrees) * Mathf.Deg2Rad), Mathf.Sin((angleDegrees) * Mathf.Deg2Rad), 0);
        }
        /// <summary>
        /// 벡터의 방향으로부터 각도 추출 (Vector3)
        /// </summary>
        /// <param name="angleDegrees"></param>
        /// <returns></returns>
        public static float AngleFromDir(Vector2 dir)
        {
            if (dir.y == 0) return dir.x > 0 ? 0 : 180;
            return Quaternion.FromToRotation(Vector2.right, dir).eulerAngles.z;
        }

        /// <summary>
        /// 방향 벡터를 특정 각도만큼 회전한 방향벡터 반환
        /// </summary>
        /// <param name="dirVector">방향 벡터</param>
        /// <param name="degreeToRotate">회적할 각도</param>
        /// <returns></returns>
        public static Vector2 GetRotatedVector2(Vector2 dirVector, float degreeToRotate)
        {
            return DirFromAngle(AngleFromDir(dirVector) + degreeToRotate);
        }
    }
}
