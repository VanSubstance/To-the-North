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
        private List<AIBaseController> unitsTank, unitsBruiser, unitRanger;

        public AIStatusType statusType = AIStatusType.Petrol;
        private List<AIBaseController> units;
        private bool isDetected = false;
        private Vector3? targetPos;
        public Vector3? detectPos;

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
            units = new List<AIBaseController>();
            units.AddRange(unitsTank);
            units.AddRange(unitsBruiser);
            units.AddRange(unitRanger);
            foreach (AIBaseController unit in units)
            {
                unit.SetSquadBase(this);
            }
        }

        /// <summary>
        /// 특정 몬스터의 종류와 인덴스 번호에 따른 위치 보정 벡터 반환
        /// </summary>
        /// <param name="monsterType">임시 변수:: 0 = 근접, 1 = 브루저, 2 = 원거리</param>
        /// <param name="idx">인덱스 번호</param>
        /// <returns></returns>
        private Vector3 GetCorrectionVectorDependingOnPoint(int monsterType, int idx)
        {
            return CalculationFunctions.GetRotatedVector2(
                formationCorrectionMatrix[monsterType, idx],
                CalculationFunctions.AngleFromDir((Vector3)targetPos - transform.position)
                ) * 3;
        }

        /// <summary>
        /// 부대를 해당 위치로 이동
        /// </summary>
        /// <param name="_targetPos"></param>
        public void MoveToTarget(Vector3 _targetPos)
        {
            targetPos = _targetPos;
            for (int i = 0; i < unitsTank.Count; i++)
            {
                unitsTank[i].SetTargetToTrack(_targetPos + GetCorrectionVectorDependingOnPoint(0, i), 0);
                unitsTank[i].SetTargetToGaze(targetPos + GetCorrectionVectorDependingOnPoint(0, i), 0);
            }
            for (int i = 0; i < unitsBruiser.Count; i++)
            {
                unitsBruiser[i].SetTargetToTrack(_targetPos + GetCorrectionVectorDependingOnPoint(1, i), 0);
                unitsBruiser[i].SetTargetToGaze(targetPos + GetCorrectionVectorDependingOnPoint(1, i), 0);
            }
            for (int i = 0; i < unitRanger.Count; i++)
            {
                unitRanger[i].SetTargetToTrack(_targetPos + GetCorrectionVectorDependingOnPoint(2, i), 0);
                unitRanger[i].SetTargetToGaze(targetPos + GetCorrectionVectorDependingOnPoint(1, i), 0);
            }
            transform.position = _targetPos;
        }

        /// <summary>
        /// 부대원들이 주변을 둘러보게 만든다
        /// </summary>
        /// <param name="isForce">아직 행동하고 있는 몬스터도 강제로 둘러보게 만들 것인가?</param>
        public void SetTargetToGaze(Vector3? _targetDir, float timeStay, bool isForce = false)
        {
            for (int i = 0; i < units.Count; i++)
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
            for (int i = 0; i < units.Count; i++)
            {
                if (!units[i].isAllActDone()) return false;
            }
            return true;
        }

        /// <summary>
        /// 타겟이 식별된 위치를 인자로 받는 함수
        /// </summary>
        /// <param name="_detectedPos">타겟이 식별된 위치</param>
        public void DetectEnemy(Vector3 _detectedPos)
        {
            statusType = AIStatusType.Combat;
            isDetected = true;
            detectPos = _detectedPos;
        }

        public void SetAllUnitsStatus(AIStatusType _type)
        {
            foreach (AIBaseController unit in units)
            {
                unit.statusType = _type;
            }
        }

        public List<AIBaseController> GetUnitsTank()
        {

            return unitsTank;
        }

        public List<AIBaseController> GetUnitsBruiser()
        {

            return unitsBruiser;
        }

        public List<AIBaseController> GetUnitsRanger()
        {

            return unitRanger;
        }

        /// <summary>
        /// 현재 적을 식별한 상태인지 불러오는 함수
        /// 단, 불러온 뒤 식별한 상태를 거짓으로 바꾼다
        /// </summary>
        /// <returns></returns>
        public bool GetIsDetected()
        {
            bool res = isDetected;
            isDetected = false;
            return res;
        }
    }
}
