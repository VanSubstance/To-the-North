using System;
using Assets.Scripts.Commons.Constants;
using UnityEngine;

namespace Assets.Scripts.Users.Controllers
{
    internal class DetectionActiveController : MonoBehaviour
    {
        [SerializeField]
        private int degree;
        private void Start()
        {
            //transform.Rotate(new Vector3(0, 0, degree));
        }
        private void Update()
        {
            CalcCenterCoor();
        }

        private void CalcCenterCoor()
        {
            transform.localRotation = Quaternion.Euler(0, 0, degree);
            Vector2 correctionCoor = new Vector2(
                (float)Math.Cos(Mathf.Deg2Rad * degree),
                (float)Math.Sin(Mathf.Deg2Rad * degree)
                ) * InGameStatus.User.Detection.Sight.range / 2f;
            transform.localPosition = correctionCoor;
        }

        public void TrySetDegreeInForce(int degree)
        {
            this.degree = degree;
        }

        public void TrySetDegreeInNormal(int degree)
        {
            if (InGameStatus.User.Detection.Sight.isControllInRealTime) return;
            TrySetDegreeInForce(degree);
        }
    }
}
