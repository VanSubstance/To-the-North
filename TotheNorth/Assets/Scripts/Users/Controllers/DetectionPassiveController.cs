using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Users.Objects;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Users.Controllers
{
    internal class DetectionPassiveController : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 10)]
        private float viewRadius = 1;
        public float meshResolution;
        Mesh viewMesh;
        public MeshFilter viewMeshFilter;

        private LayerMask obstacleMask = 1 << 7, eventMask = 1 << 9, creatureMask = 1 << 8;
        public List<Transform> visibleTargets = new List<Transform>();

        private void Start()
        {
            viewMesh = new Mesh();
            viewMesh.name = "View Mesh";
            viewMeshFilter.mesh = viewMesh;

            StartCoroutine(CheckSightWithDelay(0.01f));
        }

        private void LateUpdate()
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

        private void CheckSight()
        {
            visibleTargets.Clear();
            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, eventMask);
            targetsInViewRadius.AddRange(Physics2D.OverlapCircleAll(transform.position, viewRadius, creatureMask));
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                //if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                //{
                //    visibleTargets.Add(target);
                //}
                visibleTargets.Add(target);
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
        private DetectionSightInfo SightCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask))
            {
                //return new DetectionSightInfo(true, hit.point, hit.distance, globalAngle);
                return new DetectionSightInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
            }
            else
            {
                return new DetectionSightInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
            }
        }
        /** 시야 시각화 */
        private void DrawSightArea()
        {

            int stepCount = Mathf.RoundToInt(360 * meshResolution);
            float stepAngleSize = 360 / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.z - (360 / 2) + stepAngleSize * i;

                DetectionSightInfo newViewCast = SightCast(angle);
                viewPoints.Add(newViewCast.point);
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];
            vertices[0] = Vector3.zero;

            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }
            viewMesh.Clear();
            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }

        public float GetRadius()
        {
            return viewRadius;
        }
    }
}
