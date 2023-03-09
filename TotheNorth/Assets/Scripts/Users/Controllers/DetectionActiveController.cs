using System;
using Assets.Scripts.Commons.Constants;
using UnityEngine;

namespace Assets.Scripts.Users.Controllers
{
    internal class DetectionActiveController : MonoBehaviour
    {
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
            transform.localRotation = Quaternion.Euler(0, 0, InGameStatus.User.Movement.curdegree);
            //Vector2 correctionCoor = new Vector2(
            //    (float)Math.Cos(Mathf.Deg2Rad * degree),
            //    (float)Math.Sin(Mathf.Deg2Rad * degree)
            //    ) * InGameStatus.User.Detection.Sight.range / 2f;
            //transform.localPosition = correctionCoor;
        }
    }
}
