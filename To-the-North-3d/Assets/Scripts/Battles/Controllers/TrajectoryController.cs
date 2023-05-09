using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Functions;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    public class TrajectoryController : MonoBehaviour
    {
        private TrailRenderer trail;
        private bool isPossessed, isTerminated,
            isCurved, isClockwise;
        private Vector3 centerPos;
        public bool IsPossessed
        {
            get
            {
                return isPossessed;
            }
        }
        private float timerAfterStart;

        private float curDegree, endDegree, radius;
        private void Awake()
        {
            trail = GetComponent<TrailRenderer>();
            isTerminated = false;
            isPossessed = false;
            isCurved = false;
            timerAfterStart = 0f;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!GlobalStatus.Loading.System.CommonGameManager)
            {
                // 파괴
                StartCoroutine(CoroutineKill(0));
            }
            if (isTerminated) return;
            if (isCurved)
            {
                transform.position = centerPos + CalculationFunctions.DirFromAngle(isClockwise ? curDegree -= 2 : curDegree += 2).normalized * radius;
                if (
                    (isClockwise && curDegree < endDegree) ||
                    (!isClockwise && curDegree > endDegree))
                {
                    if (gameObject.activeSelf)
                    {
                        StartCoroutine(CoroutineKill(3));
                    }
                }
            }
            else
            {
                if (timerAfterStart < 0.2f)
                {
                    timerAfterStart += Time.deltaTime;
                }
                if (timerAfterStart > .2f)
                {
                    if (gameObject.activeSelf)
                    {
                        StartCoroutine(CoroutineKill(3));
                    }
                }
            }
        }

        /// <summary>
        /// 해당 위치로 궤적 이동
        /// </summary>
        /// <param name="posToMove"></param>
        public void MoveTo(Vector3 posToMove)
        {
            transform.position = posToMove;
            if (!gameObject.activeSelf)
            {
                isCurved = false;
                timerAfterStart = 0f;
                isPossessed = true;
                gameObject.SetActive(true);
                isTerminated = false;
            }
        }

        public void Finish()
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(CoroutineKill(3));
            }
        }

        private IEnumerator CoroutineKill(float t)
        {
            isTerminated = true;
            yield return new WaitForSeconds(t);
            trail.startWidth = .2f;
            trail.numCapVertices = 1;
            isPossessed = false;
            gameObject.SetActive(false);
        }

        public void PlayCurve(Vector3 _centerPos, float zeroDegree, float maxDergree, float _radius)
        {
            trail.startWidth = _radius / 2;
            trail.numCapVertices = 0;
            centerPos = _centerPos;
            if (zeroDegree > 90 || zeroDegree < -90)
            {
                isClockwise = false;
                curDegree = zeroDegree - maxDergree;
                endDegree = zeroDegree + maxDergree;
            }
            else
            {
                isClockwise = true;
                curDegree = zeroDegree + maxDergree;
                endDegree = zeroDegree - maxDergree;
            }
            radius = _radius;
            isCurved = true;
            isTerminated = false;
            transform.position = centerPos + CalculationFunctions.DirFromAngle(isClockwise ? curDegree-- : curDegree++).normalized * radius;
            gameObject.SetActive(true);
        }
    }
}
