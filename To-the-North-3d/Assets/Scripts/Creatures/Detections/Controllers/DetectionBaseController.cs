using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Users.Objects;
using UnityEngine;

namespace Assets.Scripts.Creatures.Detections
{
    internal abstract class DetectionBaseController : MonoBehaviour
    {
        protected AIBaseController aIBaseController;
        public bool isAI = true;
        public float meshResolution;
        public Mesh viewMesh;
        public MeshFilter viewMeshFilter;
        private void Awake()
        {
        }

        public void Start()
        {
            viewMesh = new Mesh();
            viewMesh.name = "View Mesh";
            viewMeshFilter.mesh = viewMesh;
            viewMeshFilter.GetComponent<Renderer>().sortingLayerName = "Detection";
            //viewMeshForVisualization = new Mesh();
            //viewMeshForVisualization.name = "View Mesh";
            //visualizationFilter.mesh = viewMeshForVisualization;
        }

        public void LateUpdate()
        {
            DrawSightArea();
        }

        public void SetAIBaseController(AIBaseController aIBaseController)
        {
            this.aIBaseController = aIBaseController;
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

        /** 해당 각도의 방향으로 쏘았을 때, 도달한 최종점 정보 반환 */
        public abstract DetectionSightInfo SightCast(float globalAngle, float heightDistort = 0f);
        /** 시야 시각화 */
        public abstract void DrawSightArea();

        /** 시야 체크 */
        public abstract Transform CheckSight();
    }
}
