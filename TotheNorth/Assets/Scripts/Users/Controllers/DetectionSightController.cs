using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Commons.Constants;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Users.Controllers
{
    internal class DetectionSightController : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 360)]
        private int viewAngle;
        private int curRotationDegree = 0;

        public List<Transform> visibleTargets = new List<Transform>();

        private void Start()
        {
            StartCoroutine(CheckSightWithDelay(0.01f));
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
            transform.localRotation = Quaternion.Euler(0, 0, curRotationDegree = InGameStatus.User.Movement.curdegree);
        }

        private IEnumerator CheckSightWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                CheckSight();
            }
        }

        private void CheckSight()
        {
            visibleTargets.Clear();
            // viewRadius를 반지름으로 한 원 영역 내 targetMask 레이어인 콜라이더를 모두 가져옴
            Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, InGameStatus.User.Detection.Sight.range, 1 << 9);
            targetsInViewRadius.AddRange(Physics2D.OverlapCircleAll(transform.position, InGameStatus.User.Detection.Sight.range, 1 << 8));
            for (int i = 0; i < targetsInViewRadius.Length; i++)
            {
                Transform target = targetsInViewRadius[i].transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;

                // (플레이어와 forward와 target이 이루는 각 - 마우스 회전각)이 설정한 각도 내라면
                if (Math.Abs(Vector3.SignedAngle(transform.right, dirToTarget, Vector3.forward) - curRotationDegree) < viewAngle)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.transform.position);

                    // 타겟으로 가는 레이캐스트에 obstacleMask가 걸리지 않으면 visibleTargets에 Add
                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, 1 << 7))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }

        // y축 오일러 각을 3차원 방향 벡터로 변환한다.
        // 원본과 구현이 살짝 다름에 주의. 결과는 같다.
        public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleDegrees += transform.eulerAngles.y;
            }

            return new Vector3(Mathf.Cos((angleDegrees) * Mathf.Deg2Rad), Mathf.Sin((angleDegrees) * Mathf.Deg2Rad), 0);
        }

        public int GetViewAngle()
        {
            return viewAngle;
        }

        public int GetRotationDegree()
        {
            return curRotationDegree;
        }
    }
}
