using System.Collections;
using System.Collections.Generic;
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
            sprite = meshFilterDefault.GetComponent<MeshRenderer>();
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
            transform.localRotation = Quaternion.Euler(0, isAI ? curDegree : InGameStatus.User.Movement.curdegree, 0);
        }

        public void AddRotationDegree(float degreeToAdd)
        {
            if (!isAI) return;
            curDegree += degreeToAdd;
            curDegree %= 360;
            transform.localRotation = Quaternion.Euler(0, 0, curDegree);
        }

        /** 시야 시각화 */
        public override void DrawSightArea()
        {
            int stepCount = Mathf.RoundToInt((isAI ? degree : InGameStatus.User.Detection.Sight.DegreeError) * meshResolution);
            stepCount = stepCount > 0 ? stepCount : 1;
            float stepAngleSize = (isAI ? degree : InGameStatus.User.Detection.Sight.DegreeError) / stepCount;
            stepAngleSize = float.IsInfinity(stepAngleSize) ? 0.5f : stepAngleSize;
            List<Vector3> viewPoints = new List<Vector3>();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - ((isAI ? degree : InGameStatus.User.Detection.Sight.DegreeError) / 2) + stepAngleSize * i;

                DetectionSightInfo newViewCast = SightCast(angle, isAI ? range : InGameStatus.User.Detection.Sight.Range);
                viewPoints.Add(newViewCast.point);
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
            meshDefault.Clear();
            meshDefault.vertices = vertices;
            meshDefault.triangles = triangles;
            meshDefault.RecalculateNormals();
        }

        /// <summary>
        /// 시야 내에서 상호작용 거리 안에 들어온 이벤트들 깨우기
        /// </summary>
        public override Transform CheckSight()
        {
            return null;
        }
    }
}
