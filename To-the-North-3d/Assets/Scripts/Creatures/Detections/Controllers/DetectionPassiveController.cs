using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Commons;
using Assets.Scripts.Events;
using UnityEngine;

namespace Assets.Scripts.Creatures.Detections
{
    internal class DetectionPassiveController : DetectionBaseController
    {
        public float range = 1f;
        /** 시야 시각화 */
        public override void DrawSightArea()
        {
            // 기본 시야
            int stepCount = Mathf.RoundToInt(360 * meshResolution);
            float stepAngleSize = 360 / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - (360 / 2) + stepAngleSize * i;

                DetectionSightInfo newViewCast = SightCast(angle, InGameStatus.User.Detection.Instinct.range, 0);
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
            meshDefault.Clear();
            meshDefault.vertices = vertices;
            meshDefault.triangles = triangles;
            meshDefault.RecalculateNormals();

            // 아래 시야
            if (HeightForLow == 0) return;

            viewPoints = new List<Vector3>();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - (360 / 2) + stepAngleSize * i;

                DetectionSightInfo newViewCast = SightCast(angle, InGameStatus.User.Detection.Instinct.range, -HeightForLow);
                viewPoints.Add(newViewCast.point);
            }

            vertexCount = viewPoints.Count + 1;
            vertices = new Vector3[vertexCount];
            triangles = new int[(vertexCount - 2) * 3];
            vertices[0] = Vector3.down * HeightForLow;

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
            meshLower.Clear();
            meshLower.vertices = vertices;
            meshLower.triangles = triangles;
            meshLower.RecalculateNormals();
        }

        /// <summary>
        /// 시야 내에서 상호작용 거리 안에 들어온 이벤트들 깨우기
        /// </summary>
        public override void CheckSight()
        {
            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            List<Collider> targetsInViewRadius = Physics.OverlapSphere(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.eventMask).ToList();
            targetsInViewRadius.AddRange(Physics.OverlapSphere(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.npcMask).ToList());
            //targetsInViewRadius.AddRange(Physics.OverlapCircleAll(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.creatureMask));
            for (int i = 0; i < targetsInViewRadius.Count; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                try
                {
                    target.GetComponent<IEventInteraction>().StartTrackingInteraction(transform);
                }
                catch (NullReferenceException)
                {

                }
            }
            targetsInViewRadius.Clear();
            targetsInViewRadius.AddRange(Physics.OverlapSphere(transform.position, InGameStatus.User.Detection.Instinct.range, GlobalStatus.Constant.creatureMask | GlobalStatus.Constant.eventMask | GlobalStatus.Constant.npcMask));
            if (targetsInViewRadius.Count > 0)
            {
                // 주변 반경 안에 크리쳐 식별
                targetsInViewRadius.ForEach((col) =>
                {
                    foreach (IInteractionWithSight isight in col.GetComponents<IInteractionWithSight>())
                    {
                        isight.DetectFull();
                    }
                });
            }
            return;
        }
    }
}
