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

        /** 0도 위치에서부터 angleDegree 기준 방향 벡터 (Vector3) */
        public static Vector3 DirFromAngle(float angleDegrees)
        {
            return new Vector3(Mathf.Cos((angleDegrees) * Mathf.Deg2Rad), Mathf.Sin((angleDegrees) * Mathf.Deg2Rad), 0);
        }
    }
}
