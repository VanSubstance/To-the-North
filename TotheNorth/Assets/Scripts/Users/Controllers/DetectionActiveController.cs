using System;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Assets.Scripts.Users.Controllers
{
    internal class DetectionActiveController : MonoBehaviour
    {
        [SerializeField]
        private Vector2 originalCoor;
        [SerializeField]
        private int degree;
        [SerializeField]
        private int radiusOfSight = 1;
        private void Start()
        {
            transform.Rotate(new Vector3(0, 0, degree));
        }
        private void Update()
        {
            CalcCenterCoor();
        }

        private void CalcCenterCoor()
        {
            Vector2 correctionCoor = new Vector2(
                (float)Math.Cos(Mathf.Deg2Rad * degree),
                (float)Math.Sin(Mathf.Deg2Rad * degree)
                ) * radiusOfSight / 2f;
            transform.localPosition = originalCoor + correctionCoor;
        }
    }
}
