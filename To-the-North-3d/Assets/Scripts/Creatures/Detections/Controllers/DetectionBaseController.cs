using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures.Detections
{
    internal abstract class DetectionBaseController : MonoBehaviour
    {
        [SerializeField]
        private float heightForLow;

        protected float HeightForLow
        {
            get
            {
                return heightForLow;
            }
            set
            {
                heightForLow = value;
            }
        }
        public Mesh meshDefault, meshLower;
        public MeshFilter meshFilterDefault, meshFilterLower;

        public bool isAI = true;
        public float meshResolution;
        private void Awake()
        {
        }

        public void Start()
        {
            if (meshFilterDefault != null)
            {
                meshDefault = new Mesh();
                meshDefault.name = "Default Mesh";
                meshFilterDefault.mesh = meshDefault;
                meshFilterDefault.transform.GetComponent<MeshRenderer>().sortingLayerName = "Detection";
                meshFilterDefault.transform.GetComponent<MeshRenderer>().sortingOrder = 0;
                //meshFilterDefault.transform.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, .05f);
            }

            if (meshFilterLower != null)
            {

                meshLower = new Mesh();
                meshLower.name = "Lower Mesh";
                meshFilterLower.mesh = meshLower;
                meshFilterLower.transform.GetComponent<MeshRenderer>().sortingLayerName = "Detection";
                meshFilterDefault.transform.GetComponent<MeshRenderer>().sortingOrder = 0;
            }
        }

        public void LateUpdate()
        {
            DrawSightArea();
        }

        /** 0도 위치에서부터 angleDegree 기준 방향 벡터 (Vector3) */
        public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleDegrees += transform.eulerAngles.z;
            }
            return CalculationFunctions.DirFromAngle(angleDegrees);
        }

        /// <summary>
        /// 해당 각도의 방향으로 쏘았을 때, 도달한 최종점 정보 반환 함수
        /// </summary>
        /// <param name="globalAngle">목표 각도</param>
        /// <param name="heightFromOrigin">높이 수정값</param>
        /// <param name="_range">사거리</param>
        /// <returns></returns>
        public DetectionSightInfo SightCast(float globalAngle, float _range, float heightFromOrigin = 0f)
        {
            Vector3 dir = DirFromAngle(globalAngle, true), movedTrPos = new Vector3(transform.position.x, transform.position.y + heightFromOrigin, transform.position.z);
            if (Physics.Raycast(movedTrPos, dir, out RaycastHit hit,
                _range,
                GlobalStatus.Constant.obstacleMask))
            {
                return new DetectionSightInfo(true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new DetectionSightInfo(false, movedTrPos + dir * _range, _range, globalAngle);
            }
        }
        /** 시야 시각화 */
        public abstract void DrawSightArea();

        /** 시야 체크 */
        public abstract Transform CheckSight();
    }
}
