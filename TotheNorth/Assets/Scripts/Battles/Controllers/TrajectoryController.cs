using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    public class TrajectoryController : MonoBehaviour
    {
        private LineRenderer line;
        private List<Vector3> positions;
        private float timerToRemove;
        private void Awake()
        {
            line = GetComponent<LineRenderer>();
            positions = new List<Vector3>();
            timerToRemove = 0f;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            DrawTail();
            timerToRemove += Time.deltaTime;
            if (timerToRemove > .2f)
            {
                OnDisable();
                gameObject.SetActive(false);
                return;
            }
            if (positions.Count > 10)
            {
                positions.RemoveAt(0);
            }
            //if (positions.Count > 0)
            //{
            //    // 가장 마지막 점 지우기
            //    positions.RemoveAt(0);
            //}
        }

        private void OnDisable()
        {
            positions.Clear();
            timerToRemove = 0f;
        }

        public void AddPoint(Vector3 pointToAdd)
        {
            if (!gameObject.activeSelf) gameObject.SetActive(true);
            positions.Add(pointToAdd);
            timerToRemove = 0f;
        }

        private void DrawTail()
        {
            line.positionCount = positions.Count;
            line.SetPositions(positions.ToArray());
        }
    }
}
