using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Events.Interfaces;
using Assets.Scripts.Users.Objects;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Users.Controllers
{
    internal class DetectionSightController : DetectionBaseController
    {
        public float range = 3f, degree = 60f;
        private new void Start()
        {
            base.Start();
            StartCoroutine(CheckCurRotation(0.01f));
        }

        private IEnumerator CheckCurRotation(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                SetRotationDegree();
            }
        }

        private void SetRotationDegree()
        {
            transform.localRotation = Quaternion.Euler(0, 0, isAI ? 0 : InGameStatus.User.Movement.curdegree);
        }

        /** 해당 각도의 방향으로 쏘았을 때, 도달한 최종점 정보 반환 */
        public override DetectionSightInfo SightCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(transform.position, dir,
                (isAI ? range : (int)InGameStatus.User.Detection.Sight.range),
                GlobalStatus.Constant.obstacleMask))
            {
                return new DetectionSightInfo(true, DistortPoint(globalAngle, hit.point), hit.distance, globalAngle);
            }
            else
            {
                return new DetectionSightInfo(false, transform.position + dir *
                    (isAI ? range : InGameStatus.User.Detection.Sight.range),
                    (isAI ? range : InGameStatus.User.Detection.Sight.range),
                    globalAngle);
            }
        }

        /** 현재 각도에 따른 충돌 포인트 왜곡 */
        private Vector2 DistortPoint(float globalAngle, Vector2 pointOrigin, float distortRate = 0.1f)
        {
            globalAngle = globalAngle % 360;
            if (globalAngle < 90)
            {
                pointOrigin.x += distortRate;
                pointOrigin.y += distortRate;
                return pointOrigin;
            }
            if (globalAngle < 180)
            {
                pointOrigin.x -= distortRate;
                pointOrigin.y += distortRate;
                return pointOrigin;
            }
            if (globalAngle < 270)
            {
                pointOrigin.x -= distortRate;
                pointOrigin.y -= distortRate;
                return pointOrigin;
            }
            pointOrigin.x += distortRate;
            pointOrigin.y -= distortRate;
            return pointOrigin;
        }

        /** 시야 시각화 */
        public override void DrawSightArea()
        {
            int stepCount = Mathf.RoundToInt((isAI ? degree : InGameStatus.User.Detection.Sight.degree) * meshResolution);
            float stepAngleSize = (isAI ? degree : InGameStatus.User.Detection.Sight.degree) / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.z - ((isAI ? degree : InGameStatus.User.Detection.Sight.degree) / 2) + stepAngleSize * i;

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
            if (isAI) return;
            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.eventMask);
            //targetsInViewRadius.AddRange(Physics2D.OverlapCircleAll(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.creatureMask));
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                // (플레이어와 forward와 target이 이루는 각 - 마우스 회전각)이 설정한 각도 내라면
                if (Math.Abs(Vector3.SignedAngle(transform.right, dirToTarget, Vector3.forward)) * 2 < InGameStatus.User.Detection.Sight.degree)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                    // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                    if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, GlobalStatus.Constant.obstacleMask))
                    {
                        target.GetComponent<IEventInteraction>().StartTrackingInteraction(transform);
                    }
                }
            }
        }
    }
}
