using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Events.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Creatures.Detections
{
    internal class DetectionAimController : DetectionBaseController
    {
        public float range = 3f, degree = 60f, curDegree = 0;
        private MeshRenderer sprite;
        private new void Start()
        {
            base.Start();
            sprite = viewMeshFilter.GetComponent<MeshRenderer>();
            sprite.renderingLayerMask = 3;
            if (!isAI)
                StartCoroutine(CheckCurRotation(Time.deltaTime));
        }

        private void TrackColor()
        {
            sprite.material.color = new Color(
                InGameStatus.User.Detection.Sight.DegreeError / 10,
                (10 - InGameStatus.User.Detection.Sight.DegreeError) / 10,
                0,
                0.1f
                );
        }

        private IEnumerator CheckCurRotation(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                SetRotationDegree();
                TrackColor();
            }
        }

        public void SetRotationDegree(float degree = 0)
        {
            if (isAI) curDegree = degree;
            transform.localRotation = Quaternion.Euler(0, 0, isAI ? curDegree : InGameStatus.User.Movement.curdegree);
        }

        public void AddRotationDegree(float degreeToAdd)
        {
            if (!isAI) return;
            curDegree += degreeToAdd;
            curDegree %= 360;
            transform.localRotation = Quaternion.Euler(0, 0, curDegree);
        }

        /** 해당 각도의 방향으로 쏘았을 때, 도달한 최종점 정보 반환 */
        public override DetectionSightInfo SightCast(float globalAngle, float heightDistort = 0f)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit,
                (isAI ? range : (int)InGameStatus.User.Detection.Sight.Range),
                GlobalStatus.Constant.obstacleMask))
            {
                return new DetectionSightInfo(true, DistortPoint(globalAngle, hit.point), hit.distance, globalAngle);
            }
            else
            {
                return new DetectionSightInfo(false, transform.position + dir *
                    (isAI ? range : InGameStatus.User.Detection.Sight.Range),
                    (isAI ? range : InGameStatus.User.Detection.Sight.Range),
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
            int stepCount = Mathf.RoundToInt((isAI ? degree : InGameStatus.User.Detection.Sight.DegreeError) * meshResolution);
            stepCount = stepCount > 0 ? stepCount : 1;
            float stepAngleSize = (isAI ? degree : InGameStatus.User.Detection.Sight.DegreeError) / stepCount;
            stepAngleSize = float.IsInfinity(stepAngleSize) ? 0.5f : stepAngleSize;
            List<Vector3> viewPoints = new List<Vector3>();

            if (InGameStatus.User.Detection.Sight.DegreeError < 0.5f)
            {
                float angle = transform.eulerAngles.z - ((isAI ? degree : InGameStatus.User.Detection.Sight.DegreeError) / 2);
                DetectionSightInfo newViewCast = SightCast(angle + 0.5f);
                viewPoints.Add(newViewCast.point);
                newViewCast = SightCast(angle - 0.5f);
                viewPoints.Add(newViewCast.point);
            }
            else
            {
                for (int i = 0; i <= stepCount; i++)
                {
                    float angle = transform.eulerAngles.z - ((isAI ? degree : InGameStatus.User.Detection.Sight.DegreeError) / 2) + stepAngleSize * i;
                    DetectionSightInfo newViewCast = SightCast(angle);
                    viewPoints.Add(newViewCast.point);
                }
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
                Collider[] userCol = Physics.OverlapSphere(transform.position, range, GlobalStatus.Constant.userMask);
                if (userCol != null)
                {
                    Transform userTf = userCol[0].transform;
                    Vector3 dirToTarget = (userTf.position - transform.position).normalized;
                    if (Math.Abs(Vector3.SignedAngle(transform.right, dirToTarget, Vector3.forward)) * 2 < degree)
                    {
                        float dstToTarget = Vector3.Distance(transform.position, userTf.position);
                        // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                        if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, GlobalStatus.Constant.obstacleMask))
                        {
                            aIBaseController.OnDetectUser(userTf);
                            return userTf;
                        }
                        else
                        {
                            aIBaseController.OnDetectUser(null);
                            return null;
                        }
                    }
                }
                return null;
            }
            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            List<Collider> targetsInViewRadius = new List<Collider>();
            targetsInViewRadius.AddRange(Physics.OverlapSphere(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.eventMask));
            //targetsInViewRadius.AddRange(Physics.OverlapCircleAll(transform.position, InGameStatus.User.Detection.Sight., GlobalStatus.Constant.creatureMask));
            for (int i = 0; i < targetsInViewRadius.Count; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                // (플레이어와 forward와 target이 이루는 각 - 마우스 회전각)이 설정한 각도 내라면
                if (Math.Abs(Vector3.SignedAngle(transform.right, dirToTarget, Vector3.forward)) * 2 < InGameStatus.User.Detection.Sight.Degree)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                    // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, GlobalStatus.Constant.obstacleMask))
                    {
                        try
                        {
                            target.GetComponent<IEventInteraction>().StartTrackingInteraction(transform);
                        }
                        catch (NullReferenceException)
                        {
                        }
                    }
                }
            }
            return null;
        }
    }
}
