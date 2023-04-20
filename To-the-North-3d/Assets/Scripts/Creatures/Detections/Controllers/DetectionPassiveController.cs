using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Events.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures.Detections
{
    internal class DetectionPassiveController : DetectionBaseController
    {
        public float range = 1f;
        /** 시야 시각화 */
        public override void DrawSightArea()
        {
            int stepCount = Mathf.RoundToInt(360 * meshResolution);
            float stepAngleSize = 360 / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - (360 / 2) + stepAngleSize * i;

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

        /// <summary>
        /// 시야 내에서 상호작용 거리 안에 들어온 이벤트들 깨우기
        /// </summary>
        public override Transform CheckSight()
        {
            if (aIBaseController)
            {
                // AI의 경우: 유저가 있는지만 체크
                // 유저가 있다 ? 유저 식별 시 행동 호출
                Collider2D userCol = Physics2D.OverlapCircle(transform.position, range, GlobalStatus.Constant.userMask);
                if (userCol != null)
                {
                    aIBaseController.OnDetectUser(userCol.transform);
                    return userCol.transform;
                }
                else
                {
                    aIBaseController.OnDetectUser(null);
                    return null;
                }
            }
            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            List<Collider2D> targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.eventMask).ToList();
            //targetsInViewRadius.AddRange(Physics2D.OverlapCircleAll(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.creatureMask));
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
            return null;
        }
        public override DetectionSightInfo SightCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(transform.position, dir,
                (isAI ? range : (int)InGameStatus.User.Detection.Instinct.range),
                GlobalStatus.Constant.blockingSightMask))
            {
                return new DetectionSightInfo(true, DistortPoint(globalAngle, hit.point), hit.distance, globalAngle);
            }
            else
            {
                return new DetectionSightInfo(false, transform.position + dir *
                    (isAI ? range : InGameStatus.User.Detection.Instinct.range),
                    (isAI ? range : InGameStatus.User.Detection.Instinct.range),
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
    }
}
