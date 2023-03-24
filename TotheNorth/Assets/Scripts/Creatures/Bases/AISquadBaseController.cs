using System.Collections.Generic;
using System.Collections;
using Assets.Scripts.Creatures.Interfaces;
using Assets.Scripts.Creatures.Controllers.Creatures;
using Assets.Scripts.Commons.Functions;
using UnityEngine;

namespace Assets.Scripts.Creatures.Bases
{
    class AISquadBaseController : MonoBehaviour
    {
        [SerializeField]
        private MonsterBaseController[] units;
        private List<MonsterBaseController> unitsTank, unitsBruiser, unitRanger;
        [SerializeField]
        private float timeWhenDetect = 5f;

        public AIStatusType statusType = AIStatusType.Petrol;

        private bool isDetected = false;
        private float timer = 0f;

        private Vector3? targetPos;

        private readonly Vector2[,] formationCorrectionMatrix = new Vector2[3, 5] {
            {
                new Vector2(2.5f, 0),
                new Vector2(2f, 2.5f),
                new Vector2(2f, -2.5f),
                new Vector2(1f, -4f),
                new Vector2(1f, 4f),
            },
            {
                new Vector2(0f, -1.5f),
                new Vector2(0f, 1.5f),
                new Vector2(0f, 3.5f),
                new Vector2(0f, -3.5f),
                new Vector2(1f, 0),
            },
            {
                new Vector2(-2f, 0),
                new Vector2(-2f, 2f),
                new Vector2(-2f, -2f),
                new Vector2(-2f, 4f),
                new Vector2(-2f, -4f),
            },
        };

        private void Awake()
        {
            unitsTank = new List<MonsterBaseController>();
            unitsBruiser = new List<MonsterBaseController>();
            unitRanger = new List<MonsterBaseController>();
            foreach (MonsterBaseController unit in units)
            {
                switch (unit.monsterType)
                {
                    case 0:
                        unitsTank.Add(unit);
                        break;
                    case 1:
                        unitsBruiser.Add(unit);
                        break;
                    case 2:
                        unitRanger.Add(unit);
                        break;
                }
            }
        }

        /// <summary>
        /// 특정 몬스터의 종류와 인덴스 번호에 따른 위치 보정 벡터 반환
        /// </summary>
        /// <param name="monsterType">임시 변수:: 0 = 근접, 1 = 브루저, 2 = 원거리</param>
        /// <param name="idx">인덱스 번호</param>
        /// <returns></returns>
        private Vector2 GetCorrectionVectorDependingOnPoint(int monsterType, int idx)
        {
            return CalculationFunctions.GetRotatedVector2(
                formationCorrectionMatrix[monsterType, idx],
                CalculationFunctions.AngleFromDir((Vector3)targetPos - transform.position)
                );
        }

        /// <summary>
        /// 부대를 해당 위치로 이동
        /// </summary>
        /// <param name="_targetPos"></param>
        public void MoveToTarget(Vector3 _targetPos)
        {
            targetPos = _targetPos;
            transform.position = _targetPos;
            for (int i = 0; i < unitsTank.Count; i++)
            {
                unitsTank[i].SetTargetToMove(targetPos + GetCorrectionVectorDependingOnPoint(0, i), 0);
            }
            for (int i = 0; i < unitsBruiser.Count; i++)
            {
                unitsBruiser[i].SetTargetToMove(targetPos + GetCorrectionVectorDependingOnPoint(1, i), 0);
            }
            for (int i = 0; i < unitRanger.Count; i++)
            {
                unitRanger[i].SetTargetToMove(targetPos + GetCorrectionVectorDependingOnPoint(2, i), 0);
            }
        }

        /// <summary>
        /// 부대원들이 주변을 둘러보게 만든다
        /// </summary>
        /// <param name="isForce">아직 행동하고 있는 몬스터도 강제로 둘러보게 만들 것인가?</param>
        public void SetTargetToGaze(Vector3? _targetDir, float timeStay, bool isForce = false)
        {
            for (int i = 0; i < units.Length; i++)
            {
                if (units[i].isAllActDone() || isForce)
                {
                    // 실행
                    units[i].SetTargetToGaze(_targetDir, timeStay, true);
                }
            }
        }

        public bool isAllActDone()
        {
            for (int i = 0; i < units.Length; i++)
            {
                if (!units[i].isAllActDone()) return false;
            }
            return true;
        }

        public void DetectEnemy(bool _isDetected)
        {
            if (!isDetected)
            {
                if (_isDetected)
                {
                    // 최초 식별
                    StartCoroutine(CoroutineTimer());
                    // 부대원들 Combat 상태로 전환
                    foreach (AIBaseController unit in units)
                    {
                    }
                }
                else
                {
                    // 식별 없음
                }
            }
            else
            {
                // 이미 식별이 된 상태
                if (_isDetected)
                {
                    // 추가 식별
                    timer = timeWhenDetect;
                }
                else
                {
                    // 추가 식별 없음
                }
            }
        }

        private IEnumerator CoroutineTimer()
        {
            isDetected = true;
            while (timer > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }
            // 부대원들 Combat 상태 종료
            // = 디폴트 상태 = Petrol 상태로 전환
            foreach (AIBaseController unit in units)
            {
            }
            isDetected = false;
        }
    }
}
