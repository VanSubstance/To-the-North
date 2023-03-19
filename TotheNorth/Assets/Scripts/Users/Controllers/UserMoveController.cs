using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Commons.Constants;
using UnityEngine;

namespace Assets.Scripts.Users.Controllers
{
    internal class UserMoveController : MonoBehaviour
    {
        void Start()
        {
            SetMouseEvent();
        }
        private void Update()
        {
            if (!InGameStatus.User.isPause)
            {
                TrackDirection();
                TrackMovementType();
                TrackSightZoom(0.01f);
            }
        }

        private void TrackDirection()
        {
            if (Input.GetKey(KeyCode.A))
            {
                // 왼쪽
                transform.Translate(Vector3.left * GetMovementSpd() * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                // 아래쪽
                transform.Translate(Vector3.down * GetMovementSpd() * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                // 오른쪽
                transform.Translate(Vector3.right * GetMovementSpd() * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.W))
            {
                // 위쪽
                transform.Translate(Vector3.up * GetMovementSpd() * Time.deltaTime);
            }
        }

        private float GetMovementSpd()
        {
            if (InGameStatus.User.Detection.Sight.isControllInRealTime)
            {
                // 시선 집중 중일때는 무조건 잠복 속도로
                return InGameStatus.User.Movement.spdWalk * InGameStatus.User.Movement.weightCrouch;
            }
            switch (InGameStatus.User.Movement.curMovement)
            {
                case Objects.MovementType.WALK:
                    return InGameStatus.User.Movement.spdWalk;
                case Objects.MovementType.RUN:
                    return InGameStatus.User.Movement.spdWalk * InGameStatus.User.Movement.weightRun;
                case Objects.MovementType.CROUCH:
                    return InGameStatus.User.Movement.spdWalk * InGameStatus.User.Movement.weightCrouch;
            }
            return InGameStatus.User.Movement.spdWalk;
        }

        private void TrackMovementType()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                InGameStatus.User.Movement.curMovement = Objects.MovementType.RUN;
                return;
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                InGameStatus.User.Movement.curMovement = Objects.MovementType.CROUCH;
                return;
            }
            InGameStatus.User.Movement.curMovement = Objects.MovementType.WALK;
        }

        private void TrackSightZoom(float timePass)
        {
            if (InGameStatus.User.Detection.Sight.isControllInRealTime)
            {
                // 시야 늘어나기
                if (InGameStatus.User.Detection.Sight.range < InGameStatus.User.Detection.Sight.rangeMax)
                {
                    InGameStatus.User.Detection.Sight.range += timePass;
                    return;
                }
            }
            else
            {
                // 시야 줄어들기: 시야 늘어나기의 2배속
                if (InGameStatus.User.Detection.Sight.range > InGameStatus.User.Detection.Sight.rangeMin)
                {
                    InGameStatus.User.Detection.Sight.range -= timePass * 2;
                    return;
                }
            }
        }

        private void SetMouseEvent()
        {
            GlobalStatus.Util.MouseEvent.actionSustain = (mousePos) =>
            {
                if (!InGameStatus.User.isPause)
                {
                    InGameStatus.User.Movement.curdegree = (int)
                    Vector3.SignedAngle(Vector3.right, new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized, transform.forward);
                }
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