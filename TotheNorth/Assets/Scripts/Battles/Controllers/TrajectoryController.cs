using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    public class TrajectoryController : MonoBehaviour
    {
        private LineRenderer line;
        private List<Vector3> positions;
        private bool isFinish, isPossessed;
        public bool IsPossessed
        {
            get
            {
                return isPossessed;
            }
        }
        private void Awake()
        {
            line = GetComponent<LineRenderer>();
            positions = new List<Vector3>();
            isFinish = false;
            isPossessed = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            DrawTail();
            if (!isFinish) return;
            if (positions.Count > 0)
            {
                positions.RemoveAt(0);
                return;
            }
            if (positions.Count == 0)
            {
                StartCoroutine(CoroutineKill());
                return;
            }
        }

        public void AddPoint(Vector3 pointToAdd)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
                isFinish = false;
            }
            if (positions.Count > 10)
            {
                positions.RemoveAt(0);
            }
            positions.Add(pointToAdd);
        }

        public void Finish()
        {
            isFinish = true;
        }

        private void DrawTail()
        {
            line.positionCount = positions.Count;
            line.SetPositions(positions.ToArray());
        }

        private IEnumerator CoroutineKill()
        {
            yield return new WaitForSeconds(3f);
            positions.Clear();
            isPossessed = true;
            gameObject.SetActive(false);
        }
    }
}
