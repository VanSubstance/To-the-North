using System;
using System.Collections.Generic;
using System.Linq;
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

                DetectionSightInfo newViewCast = SightCast(angle, isAI ? range : InGameStatus.User.Detection.Instinct.range, 0);
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

                DetectionSightInfo newViewCast = SightCast(angle, isAI ? range : InGameStatus.User.Detection.Instinct.range, -HeightForLow);
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
        public override Transform CheckSight()
        {
            if (isAI)
            {
                // AI의 경우: 유저가 있는지만 체크
                // 유저가 있다 ? 유저 식별 시 행동 호출
                Collider[] hitCols = Physics.OverlapSphere(transform.position, range, GlobalStatus.Constant.hitMask);
                foreach (Collider hitCol in hitCols)
                {
                    if (hitCol.CompareTag("User"))
                    {
                        // 유저 식별
                        Transform userTf = hitCol.transform;
                        Vector3 dirToTarget = (userTf.position - transform.position).normalized;
                        // 시야 각도 안
                        float dstToTarget = Vector3.Distance(transform.position, userTf.position);
                        // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                        // RayCast를 두번 해서 둘중 하나라도 통과하면 OK
                        // 1. 현재 y에서
                        if (Physics.Raycast(transform.position, new Vector3(dirToTarget.x, 0, dirToTarget.z), out RaycastHit hitInfo, dstToTarget, GlobalStatus.Constant.obstacleMask | GlobalStatus.Constant.hitMask))
                        {
                            if (hitInfo.transform.CompareTag("User"))
                            {
                                return userTf;
                            }
                        }
                        // 2. y - HeightForLow에서
                        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - HeightForLow, transform.position.z), new Vector3(dirToTarget.x, 0, dirToTarget.z), out RaycastHit hitInfo2, dstToTarget, GlobalStatus.Constant.obstacleMask | GlobalStatus.Constant.hitMask))
                        {
                            if (hitInfo2.transform.CompareTag("User"))
                            {
                                return userTf;
                            }
                        }
                    }
                }
                return null;
            }
            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            List<Collider> targetsInViewRadius = Physics.OverlapSphere(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.eventMask).ToList();
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
            targetsInViewRadius.AddRange(Physics.OverlapSphere(transform.position, InGameStatus.User.Detection.Instinct.range, GlobalStatus.Constant.creatureMask));
            if (targetsInViewRadius.Count > 0)
            {
                // 주변 반경 안에 크리쳐 식별
                IInteractionWithSight iSight;
                foreach (Collider col in targetsInViewRadius)
                {
                    iSight = col.GetComponent<IInteractionWithSight>();
                    iSight.DetectFull();
                }
            }
            return null;
        }
    }
}
