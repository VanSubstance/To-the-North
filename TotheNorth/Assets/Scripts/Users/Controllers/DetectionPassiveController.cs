using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Events.Interfaces;
using Assets.Scripts.Users.Objects;
using UnityEngine;

namespace Assets.Scripts.Users.Controllers
{
    internal class DetectionPassiveController : DetectionBaseController
    {
        /** 시야 시각화 */
        public override void DrawSightArea()
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
            viewMeshForVisualization.Clear();
            viewMeshForVisualization.vertices = vertices;
            viewMeshForVisualization.triangles = triangles;
            viewMeshForVisualization.RecalculateNormals();
        }

        /// <summary>
        /// 시야 내에서 상호작용 거리 안에 들어온 이벤트들 깨우기
        /// </summary>
        public override void CheckSight()
        {
            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.eventMask);
            //targetsInViewRadius.AddRange(Physics2D.OverlapCircleAll(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.creatureMask));
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                target.GetComponent<IEventInteraction>().StartTrackingInteraction(transform);
            }
        }

        public override DetectionSightInfo SightCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            return new DetectionSightInfo(false, transform.position + dir * InGameStatus.User.Detection.Instinct.range, InGameStatus.User.Detection.Instinct.range, globalAngle);
        }
    }
}
