using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using UnityEngine;

namespace Assets.Scripts.Users.Controllers
{
    internal class UserMoveController : MonoBehaviour
    {
        private float mvSpd = 2f;
        [SerializeField]
        private DetectionActiveController sightDetectionController;
        // Start is called before the first frame update
        void Start()
        {
            SetMouseEvent();
        }

        // Update is called once per frame
        void Update()
        {
            TrackDirection();
        }

        private void TrackDirection()
        {
            if (Input.GetKey(KeyCode.A))
            {
                // 왼쪽
                transform.Translate(Vector3.left * mvSpd * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                // 아래쪽
                transform.Translate(Vector3.down * mvSpd * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                // 오른쪽
                transform.Translate(Vector3.right * mvSpd * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.W))
            {
                // 위쪽
                transform.Translate(Vector3.up * mvSpd * Time.deltaTime);
            }
        }

        private void SetMouseEvent()
        {
            GlobalStatus.Util.MouseEvent.actionSustain = (mousePos) =>
            {
                sightDetectionController.TrySetDegreeInForce(
                    (int)
                    Vector3.SignedAngle(Vector3.right, new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized, transform.forward));
            };
            GlobalStatus.Util.MouseEvent.Right.setActions(
                actionDrag: (tr, mousePos) =>
                {
                },
                actionDown: (tr, mousePos) =>
                {
                    InGameStatus.User.Detection.Sight.isControllInRealTime = true;
                },
                actionUp: (tr, mousePos) =>
                {
                    InGameStatus.User.Detection.Sight.isControllInRealTime = false;
                }
                );
        }
    }

}