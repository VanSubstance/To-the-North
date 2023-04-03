using System;
using System.Collections;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Conductions;
using Assets.Scripts.Creatures.Controllers;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Users.Controllers;
using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    internal abstract class AIBaseController : MonoBehaviour
    {
        [SerializeField]
        private CreatureInfo info;

        public AIStatusType statusType = AIStatusType.Petrol;
        public Vector3? targetToMove, targetToGaze, targetPos, vectorToMove;
        private Vector3 vectorToDistort = Vector3.zero;
        protected AISquadBaseController squadBase;
        private DetectionPassiveController passiveController;
        private DetectionSightController sightController;
        private bool isPause = false, isForce = false;
        private bool? isUpward = null;
        private float timeStayForMove = 0, timeStayForGaze = 0;
        public bool isOrderMoveDone = true, isCollided = false;

        private readonly float forcingDis = 2f;

        private Transform hpBarTf;

        /// <summary>
        /// 현재 목표 타겟 좌표 설정
        /// 이동 종료 후 대기 시간 설정
        /// </summary>
        /// <param name="target"></param>
        /// <param name="timeToStay"></param>
        public void SetTargetToTrack(Vector3? target, float timeToStay, bool _isForce)
        {
            targetPos = CalculationFunctions.GetDetouredPositionIfInCollider(transform.position, (Vector3)target);
            timeStayForMove = timeToStay;
            isOrderMoveDone = false;
            isForce = _isForce;
            isUpward = null;
        }

        private void Awake()
        {
            if (info == null) Destroy(gameObject);
            info = CreatureInfo.GetClone(info);
            Transform temp = transform.Find("Detection Controller");
            passiveController = temp.Find("Passive").GetComponent<DetectionPassiveController>();
            sightController = temp.Find("Sight").GetComponent<DetectionSightController>();
            passiveController.SetAIBaseController(this);
            sightController.SetAIBaseController(this);
            sightController.range = info.atkRange;
        }

        private void Update()
        {
            if (!isPause)
            {
                ControllTracking();
                ControllMovement();
                ControllGaze();
                ControllIdle();
                ControllHpBar();
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
            if (targetPos != null && targetToMove != null)
            {
                // 현재 이동 목표에 도달했는지
                if (Vector2.Distance(transform.position, (Vector3)targetToMove) < 0.25f)
                {
                    vectorToMove = null;
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
                // 도달 목표에 도달했는지
                if (Vector2.Distance(transform.position, (Vector3)targetPos) < (isForce ? forcingDis : info.atkRange))
                {
                    vectorToMove = null;
                    if (isOrderMoveDone && timeStayForMove > 0)
                    {
                        timeStayForMove -= Time.deltaTime;
                    }
                    else
                    {
                        targetToMove = null;
                    }
                }
                // 이동 방향 계산
                MoveToTarget();
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            if (vectorToMove != null)
            {
                if (vectorToDistort.magnitude < 0.125f)
                {
                    vectorToDistort = Vector3.zero;
                }
                else
                {
                    vectorToDistort *= 7 / 8f;
                }
                transform.Translate(
                    (((Vector3)vectorToMove).normalized + vectorToDistort)
                    * info.moveSpd * Time.deltaTime);
            }
        }

        private void ControllGaze()
        {
            if (targetToGaze != null)
            {
                GazeTarget();
            }
        }

        private float timerIdle = -1f;
        /// <summary>
        /// 모든 행동이 끝나고 2초마다 아무런 명령을 받지 않았을 경우 자율 행동 제어
        /// </summary>
        private void ControllIdle()
        {
            if (targetPos == null)
            {
                // 타겟 소실
                timerIdle -= Time.deltaTime;
                if (timerIdle < 0)
                {
                    // 주변 랜덤 응시
                    SetTargetToGaze(null, 0, true);
                    timerIdle = 2f;
                    return;
                }
            }
        }

        private void ControllHpBar()
        {
            hpBarTf.position = transform.position + (Vector3.up * 2);
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
        /// 타겟 방향 벡터 계산 함수
        /// </summary>
        private void MoveToTarget()
        {
            try
            {
                vectorToMove = ((Vector3)targetToMove - transform.position).normalized;
            }
            catch (InvalidOperationException)
            {
                // 조기 도착
            }
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
            vectorToMove = null;
        }

        public DetectionSightController GetDetectionSight()
        {
            return sightController;
        }

        public DetectionPassiveController GetDetectionPassive()
        {
            return passiveController;
        }

        public void SetTargetToGaze(Vector3? target, float timeToStay, bool isRandom = false)
        {
            if (target == null)
            {
                targetToGaze = sightController.GetPositionOfLooking(UnityEngine.Random.Range(-90, 90));
            }
            else
            {
                if (isRandom)
                {
                    targetToGaze = new Vector3(
                        ((Vector3)target).x + UnityEngine.Random.Range(-2f, 2f),
                        ((Vector3)target).y + UnityEngine.Random.Range(-2f, 2f),
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
            Destroy(GetComponent<AIPetrolController>());
            Destroy(GetComponent<AICombatController>());
        }

        /// <summary>
        /// 목표 타겟을 이용하여 실제로 이동할 좌표 계산
        /// </summary>
        private void SetTargetToTrack()
        {
            if (targetPos == null) return;
            Vector3 _targetPos = (Vector3)targetPos;
            Vector3 originPos = transform.position;
            if (Vector3.Distance(originPos, _targetPos) < (isForce ? forcingDis : info.atkRange))
            {
                // 현재 타겟이 사거리 내에 있다
                //Debug.Log("사거리안에있음");
                if (isForce)
                {
                    // 타겟에 도착한 것으로 본다
                    targetPos = null;
                    isOrderMoveDone = true;
                    return;
                }
                // targetPos가 이동 가능한 위치에 있음
                RaycastHit2D obsHit;
                if (!(obsHit = Physics2D.Raycast(originPos, (_targetPos - originPos), info.atkRange, GlobalStatus.Constant.compositeObstacleMask)))
                {
                    // 조준 가능
                    targetPos = null;
                    isOrderMoveDone = true;
                    return;
                }
                else
                {
                    // 조준 불가
                    targetToGaze = targetToMove = FindPath(_targetPos, originPos, obsHit);
                }
            }
            else
            {
                // 현재 타겟이 사거리 밖에 있다
                targetToGaze = targetToMove = FindPath(_targetPos, originPos);
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
                if (!(obsHit = Physics2D.Raycast(originPos, (targetPos - originPos), Vector3.Distance(targetPos, originPos), GlobalStatus.Constant.compositeObstacleMask)))
                {
                    // 장애물 없음
                    Vector3 dirVec = (targetPos - originPos).normalized;
                    return targetPos - (dirVec * (isForce ? 0 : info.moveDis));
                }
            }
            // 장애물 있음
            return FindPathWithObstacle(targetPos, originPos, obsHit.transform);
        }

        /// <summary>
        /// 장애물을 우회하는 경로를 찾는 함수
        /// </summary>
        /// <param name="targetPos"></param>
        /// <param name="originPos"></param>
        /// <param name="curObsTf"></param>
        /// <returns></returns>
        private Vector3 FindPathWithObstacle(Vector3 targetPos, Vector3 originPos, Transform curObsTf)
        {
            bool isTargetUnreachable = Physics2D.Raycast(targetPos, (originPos - targetPos).normalized, 0.1f, GlobalStatus.Constant.compositeObstacleMask);
            float angK = CalculationFunctions.AngleFromDir(targetPos - originPos), angR = (angK + 180) % 360, disCompare = -1;
            float[] valByOrigin = GetAnglesAndDistanceMeetsObstacle(originPos, angK, curObsTf);
            if (isTargetUnreachable)
            {
                // 타겟이 접근 불가 상태이다
                if (valByOrigin[1] < valByOrigin[3])
                {
                    // 위가 더 짧다 = 위로 우회
                    if (isUpward == null)
                    {
                        isUpward = true;
                    }
                    if ((bool)isUpward)
                    {
                        return CalculationFunctions.DirFromAngle(valByOrigin[0]) * Math.Min((valByOrigin[1] + 0.5f), info.moveDis) + originPos;
                    }
                    else
                    {
                        return CalculationFunctions.DirFromAngle(valByOrigin[2]) * Math.Min((valByOrigin[3] + 0.5f), info.moveDis) + originPos;
                    }
                }
                // 아래가 더 짧다 = 아래로 우회
                if (isUpward == null)
                {
                    isUpward = false;
                }
                if (!(bool)isUpward)
                {
                    return CalculationFunctions.DirFromAngle(valByOrigin[2]) * Math.Min((valByOrigin[3] + 0.5f), info.moveDis) + originPos;
                }
                else
                {
                    return CalculationFunctions.DirFromAngle(valByOrigin[0]) * Math.Min((valByOrigin[1] + 0.5f), info.moveDis) + originPos;
                }
            }
            float[] valByTarget = GetAnglesAndDistanceMeetsObstacle(targetPos, angR, curObsTf);
            if (valByOrigin[0] - angK + angR - valByTarget[2] < 180)
            {
                disCompare = valByOrigin[1] + valByTarget[3];
            }
            if (angK - angR + valByTarget[0] - valByOrigin[2] < 180)
            {
                if (disCompare != -1)
                {
                    // 둘 다 가능
                    if (disCompare < (valByOrigin[3] + valByTarget[1]))
                    {
                        if (isUpward == null)
                        {
                            isUpward = true;
                        }
                        if ((bool)isUpward)
                        {
                            return CalculationFunctions.DirFromAngle(valByOrigin[0]) * Math.Min((valByOrigin[1] + 0.5f), info.moveDis) + originPos;
                        }
                        else
                        {
                            return CalculationFunctions.DirFromAngle(valByOrigin[2]) * Math.Min((valByOrigin[3] + 0.5f), info.moveDis) + originPos;
                        }
                    }
                }
                // 아래만 가능 or 아래가 위보다 짧음
                if (isUpward == null)
                {
                    isUpward = false;
                }
                if (!(bool)isUpward)
                {
                    return CalculationFunctions.DirFromAngle(valByOrigin[2]) * Math.Min((valByOrigin[3] + 0.5f), info.moveDis) + originPos;
                }
                else
                {
                    return CalculationFunctions.DirFromAngle(valByOrigin[0]) * Math.Min((valByOrigin[1] + 0.5f), info.moveDis) + originPos;
                }
            }
            if (disCompare != -1)
            {
                if (isUpward == null)
                {
                    isUpward = true;
                }
                if ((bool)isUpward)
                {
                    // 위만 가능
                    return CalculationFunctions.DirFromAngle(valByOrigin[0]) * Math.Min((valByOrigin[1] + 0.5f), info.moveDis) + originPos;
                }
                else
                {
                    return CalculationFunctions.DirFromAngle(valByOrigin[2]) * Math.Min((valByOrigin[3] + 0.5f), info.moveDis) + originPos;
                }
            }
            // 타겟 위치 옮겨서 다시 계산
            //Debug.Log("각이 안나옴:: 추가 계산");
            targetPos -= CalculationFunctions.DirFromAngle((valByTarget[0] + valByTarget[2]) / 2) * (valByTarget[1] + valByTarget[3]) / 2;
            Debug.DrawLine(originPos, targetPos, Color.green, 1);
            return FindPathWithObstacle(targetPos, originPos, curObsTf);
        }

        /// <summary>
        /// 주체 + K 도 기준 장애물과 만나지 않는 최초 위, 아래 각도와 거리 반환
        /// </summary>
        /// <param name="originPos">주체 위치</param>
        /// <param name="angK">시작 각도</param>
        /// <returns>[0]: Up 각도, [1]: Up 거리, [2]: Down 각도, [3]: Down 거리</returns>
        private float[] GetAnglesAndDistanceMeetsObstacle(Vector3 originPos, float angK, Transform curObsTf)
        {
            int unitDegree = 3;
            float[] res = new float[4];
            float disDump = 0f;
            RaycastHit2D hit;
            // Up
            for (int i = 1; i <= 180 / unitDegree; i++)
            {
                //Debug.DrawRay(originPos, CalculationFunctions.DirFromAngle(angK + (i * unitDegree)) * 100, Color.green, 0.3f);
                if (!(hit = Physics2D.Raycast(originPos, CalculationFunctions.DirFromAngle(angK + (i * unitDegree)), 100, GlobalStatus.Constant.compositeObstacleMask)))
                {
                    //Debug.DrawRay(originPos, CalculationFunctions.DirFromAngle(angK + (i * unitDegree)) * 100, Color.blue, 0.3f);
                    // 장애물 안걸리기 시작
                    res[0] = angK + (i * unitDegree) + unitDegree;
                    res[1] = disDump;
                    break;
                }
                if (!hit.transform.Equals(curObsTf))
                {
                    // 장애물에 걸리기는 했는데 중간 장애물이 아닌 경우
                    //Debug.Log("걔가 아닌데?");
                    //Debug.DrawRay(originPos, CalculationFunctions.DirFromAngle(angK + (i * unitDegree)) * 100, Color.blue, 0.3f);
                    res[0] = angK + (i * unitDegree) + unitDegree;
                    res[1] = disDump;
                    break;
                }
                disDump = hit.distance;
            }
            // Down
            for (int i = 1; i <= 180 / unitDegree; i++)
            {
                //Debug.DrawRay(originPos, CalculationFunctions.DirFromAngle(angK - (i * unitDegree)) * 100, Color.magenta, 0.3f);
                if (!(hit = Physics2D.Raycast(originPos, CalculationFunctions.DirFromAngle(angK - (i * unitDegree)), 100, GlobalStatus.Constant.compositeObstacleMask)))
                {
                    //Debug.DrawRay(originPos, CalculationFunctions.DirFromAngle(angK - (i * unitDegree)) * 100, Color.red, 0.3f);
                    res[2] = angK - (i * unitDegree) - unitDegree;
                    res[3] = disDump;
                    break;
                }
                if (!hit.transform.Equals(curObsTf))
                {
                    // 장애물에 걸리기는 했는데 중간 장애물이 아닌 경우
                    //Debug.Log("걔가 아닌데?");
                    //Debug.DrawRay(originPos, CalculationFunctions.DirFromAngle(angK - (i * unitDegree)) * 100, Color.red, 0.3f);
                    res[2] = angK - (i * unitDegree) - unitDegree;
                    res[3] = disDump;
                    break;
                }
                disDump = hit.distance;
            }
            return res;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (vectorToMove != null && targetPos != null)
            {
                StartCoroutine(forcingReTrackAfterSec(1 / 4f));
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (vectorToMove != null && targetPos != null)
            {
                isCollided = true;
                vectorToDistort += CalculationFunctions.DirFromAngle(CalculationFunctions.AngleFromDir(collision.contacts[0].normal) + UnityEngine.Random.Range(-30, 30)) * 2;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            isCollided = false;
        }

        private IEnumerator forcingReTrackAfterSec(float t)
        {
            yield return new WaitForSeconds(t);
            while (isCollided)
            {
                SetTargetToTrack();
                yield return new WaitForSeconds(1f);
            }
        }

        public void SetHpBar(Transform _hpBarTf)
        {
            hpBarTf = _hpBarTf;
        }

        public abstract void OnDetectUser(Transform targetTf);
    }
}

