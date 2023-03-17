using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Users.Objects;
using UnityEngine;

namespace Assets.Scripts.Users.Controllers
{
    internal abstract class DetectionBaseController : MonoBehaviour
    {
        public bool isAI = true;
        public float meshResolution;
        public Mesh viewMesh, viewMeshForVisualization;
        public MeshFilter viewMeshFilter, visualizationFilter;

        public void Start()
        {
            viewMesh = new Mesh();
            viewMesh.name = "View Mesh";
            viewMeshFilter.mesh = viewMesh;
            viewMeshForVisualization = new Mesh();
            viewMeshForVisualization.name = "View Mesh";
            visualizationFilter.mesh = viewMeshForVisualization;

            StartCoroutine(CheckSightWithDelay(0.01f));
        }

        public void LateUpdate()
        {
            DrawSightArea();
        }

        private IEnumerator CheckSightWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                CheckSight();
            }
        }

        /** 0도 위치에서부터 angleDegree 기준 방향 벡터 (Vector3) */
        public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleDegrees += transform.eulerAngles.y;
            }
            return CalculationFunctions.DirFromAngle(angleDegrees);
        }

        /** 해당 각도의 방향으로 쏘았을 때, 도달한 최종점 정보 반환 */
        public abstract DetectionSightInfo SightCast(float globalAngle);
        /** 시야 시각화 */
        public abstract void DrawSightArea();

        /** 시야 체크 */
        public abstract void CheckSight();
    }
}
