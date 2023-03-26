using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Users.Controllers;
using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    internal abstract class AIBaseController : MonoBehaviour
    {
        public float atkRange = 5f;

        public AIStatusType statusType = AIStatusType.Petrol;
        public Vector3? targetToMove, targetToGaze;
        public Vector3? targetPos;
        protected AISquadBaseController squadBase;
        private DetectionPassiveController passiveController;
        private DetectionSightController sightController;
        private bool isPause = false;
        private float timeStayForMove = 0, timeStayForGaze = 0;
        public bool isOrderMoveDone = true/*, isOrderGazeDone = true*/;

        private void Awake()
        {
            Transform temp = transform.Find("Detection Controller");
            passiveController = temp.Find("Passive").GetComponent<DetectionPassiveController>();
            sightController = temp.Find("Sight").GetComponent<DetectionSightController>();
            passiveController.SetAIBaseController(this);
            sightController.SetAIBaseController(this);
            sightController.range = atkRange;
        }

        private void Update()
        {
            if (!isPause)
            {
                ControllTracking();
                ControllMovement();
                ControllGaze();
                ControllIdle();
            }
        }

        private void ControllTracking()
        {
            if (targetPos != null)
            {
                if (targetToMove == null)
                {
                    SetTargetToTrack();
                }
            }
        }

        private void ControllMovement()
        {
            if (targetToMove != null)
            {
                MoveToTarget();
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }

        private void ControllGaze()
        {
            if (targetToGaze != null)
            {
                GazeTarget();
            }
        }

        /// <summary>
        /// 모든 행동이 끝나고 0.5초동안 아무런 명령을 받지 않았을 경우 자율 행동 제어
        /// </summary>
        private void ControllIdle()
        {

        }

        /// <summary>
        /// 타겟 방향으로 시야를 회전하는 함수 (전부가 아닌 조금씩)
        /// </summary>
        private void GazeTarget()
        {
            float targetDegree = CalculationFunctions.AngleFromDir((Vector3)targetToGaze - transform.position);
            targetDegree += 360 * 3;
            targetDegree -= sightController.curDegree;
            targetDegree %= 360;
            if (targetDegree > 1)
            {
                if (targetDegree < 180)
                {
                    sightController.AddRotationDegree(1);
                }
                else
                {
                    sightController.AddRotationDegree(-1);
                }
            }
            else
            {
                if (timeStayForGaze > 0)
                {
                    timeStayForGaze -= Time.deltaTime;
                }
                else
                {
                    targetToGaze = null;
                }
                return;
            }
        }

        /// <summary>
        /// 타겟 방향으로 이동하는 함수 (전부가 아닌 조금씩)
        /// </summary>
        private void MoveToTarget()
        {
            if (Vector2.Distance(transform.position, (Vector3)targetToMove) < 0.1f)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (isOrderMoveDone && timeStayForMove > 0)
                {
                    timeStayForMove -= Time.deltaTime;
                }
                else
                {
                    targetToMove = null;
                }
                return;
            }
            GetComponent<Rigidbody2D>().velocity = ((Vector3)targetToMove - transform.position).normalized * 2;
        }

        public bool isAllActDone()
        {
            return
                isOrderMoveDone &&
                targetToGaze == null &&
                timeStayForGaze <= 0 &&
                true;
        }

        public void PauseOrResumeAct(bool toPause)
        {
            isPause = toPause;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }

        public DetectionSightController GetDetectionSight()
        {
            return sightController;
        }

        public DetectionPassiveController GetDetectionPassive()
        {
            return passiveController;
        }

        /// <summary>
        /// 현재 목표 타겟 좌표 설정
        /// 이동 종료 후 대기 시간 설정
        /// </summary>
        /// <param name="target"></param>
        /// <param name="timeToStay"></param>
        public void SetTargetToTrack(Vector3? target, float timeToStay)
        {
            targetPos = target;
            timeStayForMove = timeToStay;
            isOrderMoveDone = false;
        }

        public void SetTargetToGaze(Vector3? target, float timeToStay, bool isRandom = false)
        {
            if (target == null)
            {
                targetToGaze = sightController.GetPositionOfLooking(Random.Range(-90, 90));
            }
            else
            {
                if (isRandom)
                {
                    targetToGaze = new Vector3(
                        ((Vector3)target).x + Random.Range(-2f, 2f),
                        ((Vector3)target).y + Random.Range(-2f, 2f),
                        0);
                }
                else
                {
                    targetToGaze = target;
                }
            }
            timeStayForGaze = timeToStay;
        }

        public void SetSquadBase(AISquadBaseController _squadBase)
        {
            squadBase = _squadBase;
        }

        /// <summary>
        /// 목표 타겟을 이용하여 실제로 이동할 좌표 계산
        /// </summary>
        private void SetTargetToTrack()
        {
            Vector3 _targetPos = (Vector3)targetPos;
            Vector3 originPos = transform.position;
            if (Vector3.Distance(originPos, _targetPos) < atkRange)
            {
                // 현재 타겟이 사거리 내에 있다
                //Debug.Log("사거리안에있음");
                if (!Physics2D.Raycast(_targetPos, (originPos - _targetPos), 0.1f, GlobalStatus.Constant.obstacleMask))
                {
                    // targetPos가 이동 가능한 위치에 있음
                    RaycastHit2D obsHit;
                    if (!(obsHit = Physics2D.Raycast(originPos, (_targetPos - originPos), atkRange, GlobalStatus.Constant.obstacleMask)))
                    {
                        // 조준 가능
                        targetPos = null;
                        isOrderMoveDone = true;
                        return;
                    }
                    else
                    {
                        // 조준 불가
                        targetToMove = FindPath(_targetPos, originPos, obsHit);
                    }
                }
                else
                {
                    // targetPos가 이동불가 위치에 있음
                    targetPos = null;
                    isOrderMoveDone = true;
                    return;
                }
            }
            else
            {
                // 현재 타겟이 사거리 밖이다
                targetToMove = FindPath(_targetPos, originPos);
            }
        }

        /// <summary>
        /// 목표 지점으로 가는 경로 찾기 함수
        /// </summary>
        /// <param name="targetPos">목표 지점</param>
        /// <param name="obsTf">기존에 식별된 장애물 Transform</param>
        /// <returns></returns>
        private Vector3 FindPath(Vector3 targetPos, Vector3 originPos, RaycastHit2D? prevHit = null)
        {
            RaycastHit2D obsHit;
            if (prevHit != null)
            {
                obsHit = (RaycastHit2D)prevHit;
            }
            else
            {
                if (!(obsHit = Physics2D.Raycast(originPos, (targetPos - originPos), Vector3.Distance(targetPos, originPos), GlobalStatus.Constant.obstacleMask)))
                {
                    // 장애물 없음
                    Vector3 dirVec = (targetPos - originPos).normalized;
                    return targetPos - (dirVec * (atkRange - 1));
                }
                else
                {
                }
            }
            // 장애물 있음
            return FindPathWithObstacle(targetPos, originPos, obsHit.transform);
        }

        private Vector3 FindPathWithObstacle(Vector3 targetPos, Vector3 originPos, Transform curObsTf)
        {
            Debug.DrawLine(targetPos, originPos, Color.yellow, 1000);
            float angK = CalculationFunctions.AngleFromDir(targetPos - originPos), angR = (angK + 180) % 360, disCompare = -1;
            float[] valByOrigin = GetAnglesAndDistanceMeetsObstacle(originPos, angK, curObsTf);
            float[] valByTarget = GetAnglesAndDistanceMeetsObstacle(targetPos, angR, curObsTf);
            if (valByOrigin[0] - angK + angR - valByTarget[2] < 180)
            {
                disCompare = valByOrigin[1] + valByTarget[3];
            }
            if (angK - angR + valByTarget[0] - valByOrigin[2] < 180)
            {
                if (disCompare != -1)
                {
                    if (disCompare < (valByOrigin[3] + valByTarget[1]))
                    {
                        return CalculationFunctions.DirFromAngle(valByOrigin[0]) * (valByOrigin[1] + 0.5f) + originPos;
                    }
                }
                return CalculationFunctions.DirFromAngle(valByOrigin[2]) * (valByOrigin[3] + 0.5f) + originPos;
            }
            else
            {
                // 타겟 위치 옮겨서 다시 계산
                targetPos += CalculationFunctions.DirFromAngle((valByTarget[0] + valByTarget[2]) / 2) * (valByTarget[1] + valByTarget[3]) / 2;
                return FindPathWithObstacle(targetPos, originPos, curObsTf);
            }
        }

        /// <summary>
        /// 주체 + K 도 기준 장애물과 만나지 않는 최초 위, 아래 각도와 거리 반환
        /// </summary>
        /// <param name="originPos">주체 위치</param>
        /// <param name="angK">시작 각도</param>
        /// <returns>[0]: Up 각도, [1]: Up 거리, [2]: Down 각도, [3]: Down 거리</returns>
        private float[] GetAnglesAndDistanceMeetsObstacle(Vector3 originPos, float angK, Transform curObsTf)
        {
            int unitDegree = 1;
            float[] res = new float[4];
            float disDump = 0f;
            RaycastHit2D hit;
            // Up
            for (int i = 1; i <= 180 / unitDegree; i++)
            {
                //Debug.DrawRay(originPos, CalculationFunctions.DirFromAngle(angK + (i * unitDegree)) * 100, Color.green, 1000);
                if (!(hit = Physics2D.Raycast(originPos, CalculationFunctions.DirFromAngle(angK + (i * unitDegree)), 100, GlobalStatus.Constant.obstacleMask)))
                {
                    //Debug.DrawRay(originPos, CalculationFunctions.DirFromAngle(angK + (i * unitDegree)) * 100, Color.red, 1000);
                    // 장애물 안걸리기 시작
                    res[0] = angK + (i * unitDegree) + 1;
                    res[1] = disDump;
                    break;
                }
                disDump = hit.distance;
                if (hit.transform.Equals(curObsTf))
                {
                    // 장애물에 걸리기는 했는데 중간 장애물이 아닌 경우
                }
            }
            // Down
            for (int i = 1; i <= 180 / unitDegree; i++)
            {
                //Debug.DrawRay(originPos, CalculationFunctions.DirFromAngle(angK - (i * unitDegree)) * 100, Color.magenta, 1000);
                if (!(hit = Physics2D.Raycast(originPos, CalculationFunctions.DirFromAngle(angK - (i * unitDegree)), 100, GlobalStatus.Constant.obstacleMask)))
                {
                    //Debug.DrawRay(originPos, CalculationFunctions.DirFromAngle(angK - (i * unitDegree)) * 100, Color.red, 1000);
                    res[2] = angK - (i * unitDegree) - 1;
                    res[3] = disDump;
                    break;
                }
                disDump = hit.distance;
                if (hit.transform.Equals(curObsTf))
                {
                    // 장애물에 걸리기는 했는데 중간 장애물이 아닌 경우
                }
            }
            return res;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("충돌!!");
            Debug.Log(GetComponent<Rigidbody2D>().velocity);
        }

        public abstract void OnDetectUser(Transform targetTf);
    }
}
