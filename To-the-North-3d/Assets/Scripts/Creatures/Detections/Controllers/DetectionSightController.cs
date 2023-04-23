using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Creatures.Detections
{
    internal class DetectionSightController : DetectionBaseController
    {
        public float range = 3f, degree = 60f, curDegree = 0;
        private bool isForce = false;

        public bool isVisualization = true;

        private Vector3 target;
        /// <summary>
        /// 상대 좌표 기준
        /// 타겟 설정 함수
        /// </summary>
        public Vector3 Target
        {
            set
            {
                target = value;
            }
        }

        public void SetTrackByDegree(float targetDegree)
        {
            isForce = true;
            curDegree = targetDegree;
            transform.localRotation = Quaternion.Euler(0, targetDegree, 0);
        }

        public bool IsGazeDone
        {
            get
            {
                float d = CalculationFunctions.AngleFromDir(new Vector2(target.x, target.z));
                return Mathf.Abs(curDegree - d) < 1f;
            }
        }

        private void Update()
        {
            ControlGaze();
            ControlGaze();
        }

        /// <summary>
        /// 타겟 방향으로 고개 돌리기
        /// 타겟에 도착했다 = 암것도 안함
        /// </summary>
        private void ControlGaze()
        {
            if (isForce) return;
            float d = CalculationFunctions.AngleFromDir(new Vector2(target.x, target.z));
            if (Mathf.Abs(curDegree - d) < 1f) return;
            if ((d - curDegree + 360) % 360 > 180)
            {
                // 시계
                AddRotationDegree(-1f);
            }
            else
            {
                // 반시계
                AddRotationDegree(1f);
            }
        }

        public void AddRotationDegree(float degreeToAdd)
        {
            curDegree += 360 + degreeToAdd;
            curDegree %= 360;
            transform.localRotation = Quaternion.Euler(0, curDegree, 0);
        }

        /** 시야 시각화 */
        public override void DrawSightArea()
        {
            List<Vector3> viewPoints;
            int stepCount = 0;
            float stepAngleSize = 0;

            int vertexCount;
            Vector3[] vertices;
            int[] triangles;

            if (meshFilterDefault != null)
            {
                meshDefault.Clear();
                meshLower.Clear();
                if (!isVisualization)
                {
                    return;
                }
                stepCount = Mathf.RoundToInt((isAI ? degree : InGameStatus.User.Detection.Sight.Degree) * meshResolution);
                stepAngleSize = (isAI ? degree : InGameStatus.User.Detection.Sight.Degree) / stepCount;
                viewPoints = new List<Vector3>();

                for (int i = 0; i <= stepCount; i++)
                {
                    float angle = transform.eulerAngles.y - ((isAI ? degree : InGameStatus.User.Detection.Sight.Degree) / 2) + stepAngleSize * i;

                    DetectionSightInfo newViewCast = SightCast(angle, isAI ? range : InGameStatus.User.Detection.Sight.Range, 4f);
                    viewPoints.Add(newViewCast.point);
                }

                vertexCount = viewPoints.Count + 1;
                vertices = new Vector3[vertexCount];
                triangles = new int[(vertexCount - 2) * 3];
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
                meshDefault.vertices = vertices;
                meshDefault.triangles = triangles;
                meshDefault.RecalculateNormals();
            }

            // 아래 시야
            if (HeightForLow == 0) return;

            if (meshFilterLower != null)
            {
                viewPoints = new List<Vector3>();

                for (int i = 0; i <= stepCount; i++)
                {
                    float angle = transform.eulerAngles.y - ((isAI ? degree : InGameStatus.User.Detection.Sight.Degree) / 2) + stepAngleSize * i;

                    DetectionSightInfo newViewCast = SightCast(angle, isAI ? range : InGameStatus.User.Detection.Sight.Range, -HeightForLow);
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
                meshLower.vertices = vertices;
                meshLower.triangles = triangles;
                meshLower.RecalculateNormals();
            }

            // 아래 시야 중 반칸 장애물 넘어서가 있는 경우

        }

        /// <summary>
        /// 시야 내에서 상호작용 거리 안에 들어온 이벤트 또는 타겟 식별
        /// </summary>
        public override Transform CheckSight()
        {
            if (aIBaseController)
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
                        float d = Math.Abs(CalculationFunctions.AngleFromDir(new Vector2(dirToTarget.x, dirToTarget.z)) - curDegree);
                        if (d < degree / 2 || (360 - d) < degree / 2)
                        {
                            // 시야 각도 안
                            float dstToTarget = Vector3.Distance(transform.position, userTf.position);
                            // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                            // RayCast를 두번 해서 둘중 하나라도 통과하면 OK
                            // 1. 현재 y에서
                            if (Physics.Raycast(transform.position, new Vector3(dirToTarget.x, 0, dirToTarget.z), out RaycastHit hitInfo, dstToTarget, GlobalStatus.Constant.obstacleMask | GlobalStatus.Constant.hitMask))
                            {
                                if (hitInfo.transform.CompareTag("User"))
                                {
                                    aIBaseController.OnDetectUser(userTf);
                                    return userTf;
                                }
                            }
                            // 2. y - HeightForLow에서
                            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - HeightForLow, transform.position.z), new Vector3(dirToTarget.x, 0, dirToTarget.z), out RaycastHit hitInfo2, dstToTarget, GlobalStatus.Constant.obstacleMask | GlobalStatus.Constant.hitMask))
                            {
                                if (hitInfo2.transform.CompareTag("User"))
                                {
                                    aIBaseController.OnDetectUser(userTf);
                                    return userTf;
                                }
                            }
                            return null;
                        }
                        return null;
                    }
                }
                return null;
            }
            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            List<Collider> targetsInViewRadius = new List<Collider>();
            targetsInViewRadius.AddRange(Physics.OverlapSphere(transform.position, InGameStatus.User.Detection.distanceInteraction, GlobalStatus.Constant.eventMask));
            for (int i = 0; i < targetsInViewRadius.Count; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                float d = Math.Abs(CalculationFunctions.AngleFromDir(new Vector2(dirToTarget.x, dirToTarget.z)) - curDegree);
                if (d < InGameStatus.User.Detection.Sight.Degree / 2 || (360 - d) < InGameStatus.User.Detection.Sight.Degree / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                    // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                    if (Physics.Raycast(transform.position, new Vector3(dirToTarget.x, 0, dirToTarget.z), dstToTarget, GlobalStatus.Constant.obstacleMask))
                    {
                        // 해당 이벤트 반짝반짝 on
                    }
                    else
                    {
                        // 해당 이벤트 반짝반짝 off
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
            targetsInViewRadius.Clear();
            targetsInViewRadius.AddRange(Physics.OverlapSphere(transform.position, InGameStatus.User.Detection.Sight.Range, GlobalStatus.Constant.creatureMask | GlobalStatus.Constant.eventMask));
            if (targetsInViewRadius.Count > 0)
            {
                foreach (Collider col in targetsInViewRadius)
                {
                    Vector3 dirToTarget = (col.transform.position - transform.position).normalized;
                    float d = Math.Abs(CalculationFunctions.AngleFromDir(new Vector2(dirToTarget.x, dirToTarget.z)) - curDegree);
                    if (d < InGameStatus.User.Detection.Sight.Degree / 2 || (360 - d) < InGameStatus.User.Detection.Sight.Degree / 2)
                    {
                        float dstToTarget = Vector3.Distance(transform.position, col.transform.transform.position);

                        // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리면 안보이는거지만 대략은 보여야 함
                        if (Physics.Raycast(transform.position, new Vector3(dirToTarget.x, 0, dirToTarget.z), dstToTarget, GlobalStatus.Constant.obstacleMask))
                        {
                            // 해당 몬스터 반짝반짝 on
                        }
                        else
                        {
                            // 해당 몬스터 반짝반짝 off
                        }
                    }
                }
            }
            return null;
        }
    }
}
