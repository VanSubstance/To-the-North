using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    public class TrajectoryController : MonoBehaviour
    {
        private bool isPossessed, isTerminated;
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
            isTerminated = false;
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
            if (timerAfterStart > .2f && !isTerminated)
            {
                StartCoroutine(CoroutineKill());
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
                timerAfterStart = 0f;
                isPossessed = true;
                gameObject.SetActive(true);
            }
        }

        public void Finish()
        {
            StartCoroutine(CoroutineKill());
        }

        private IEnumerator CoroutineKill()
        {
            isTerminated = true;
            yield return new WaitForSeconds(3f);
            isPossessed = false;
            gameObject.SetActive(false);
        }
    }
}
