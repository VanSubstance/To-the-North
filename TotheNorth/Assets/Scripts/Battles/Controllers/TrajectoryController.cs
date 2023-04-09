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
        private float timerAfterStart;
        private void Awake()
        {
            line = GetComponent<LineRenderer>();
            positions = new List<Vector3>();
            isFinish = false;
            isPossessed = false;
            timerAfterStart = 0f;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (timerAfterStart < 0.2f)
            {
                timerAfterStart += Time.deltaTime;
            }
            else
            {
                if (positions.Count > 0)
                {
                    positions.RemoveAt(0);
                }
            }
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
                timerAfterStart = 0f;
                isPossessed = true;
                isFinish = false;
                gameObject.SetActive(true);
            }
            //if (positions.Count > 10)
            //{
            //    positions.RemoveAt(0);
            //}
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
            isPossessed = false;
            gameObject.SetActive(false);
        }
    }
}
