using Assets.Scripts.Commons.Constants;
using UnityEngine;

namespace Assets.Scripts.Users
{
    internal class UserMoveController : MonoBehaviour
    {
        private float secForRecoverStamina = 0;
        private Rigidbody2D rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody2D>();
        }
        private void Update()
        {
            if (!InGameStatus.User.isPause)
            {
                TrackDirection();
                TrackMovementType();
                TrackStamina();
            }
        }

        private void TrackDirection()
        {
            float spdW = GetMovementSpd();
            bool isMoving = false;
            Vector3 vecHor = Vector3.zero, vecVer = Vector3.zero, vecToMove = Vector3.zero;
            if (Input.GetKey(KeyCode.A))
            {
                isMoving = true;
                // 왼쪽
                if (InGameStatus.User.Movement.curMovement == Objects.MovementType.RUN)
                {
                    secForRecoverStamina = 0;
                    InGameStatus.User.status.staminaBar.AddCurrent(-Time.deltaTime * 20);
                }
                vecToMove += Vector3.left * spdW;
                vecHor += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                isMoving = true;
                // 아래쪽
                if (InGameStatus.User.Movement.curMovement == Objects.MovementType.RUN)
                {
                    secForRecoverStamina = 0;
                    InGameStatus.User.status.staminaBar.AddCurrent(-Time.deltaTime * 20);
                }
                vecToMove += Vector3.down * spdW;
                vecVer += Vector3.down;
            }
            if (Input.GetKey(KeyCode.D))
            {
                isMoving = true;
                // 오른쪽
                if (InGameStatus.User.Movement.curMovement == Objects.MovementType.RUN)
                {
                    secForRecoverStamina = 0;
                    InGameStatus.User.status.staminaBar.AddCurrent(-Time.deltaTime * 20);
                }
                vecToMove += Vector3.right * spdW;
                vecVer += Vector3.right;
            }
            if (Input.GetKey(KeyCode.W))
            {
                isMoving = true;
                // 위쪽
                if (InGameStatus.User.Movement.curMovement == Objects.MovementType.RUN)
                {
                    secForRecoverStamina = 0;
                    InGameStatus.User.status.staminaBar.AddCurrent(-Time.deltaTime * 20);
                }
                vecToMove += Vector3.up * spdW;
                vecVer += Vector3.up;
            }
            if (isMoving)
            {
                rigid.velocity = vecToMove;
            }
            else
            {
                rigid.velocity = Vector2.zero;
            }
            CameraTrackControlller.headHorPos = vecHor * spdW;
            CameraTrackControlller.headVerPos = vecVer * spdW;
        }

        private float GetMovementSpd()
        {
            if (InGameStatus.User.Detection.Sight.isControllInRealTime)
            {
                // 시선 집중 중일때는 무조건 잠복 속도로
                return InGameStatus.User.Movement.weightCrouch;
            }
            switch (InGameStatus.User.Movement.curMovement)
            {
                case Objects.MovementType.WALK:
                    return InGameStatus.User.Movement.spdWalk;
                case Objects.MovementType.RUN:
                    if (InGameStatus.User.status.staminaBar.GetCurrent() > 0)
                    {
                        return InGameStatus.User.Movement.weightRun;
                    }
                    else
                    {
                        return InGameStatus.User.Movement.spdWalk;
                    }
                case Objects.MovementType.CROUCH:
                    return InGameStatus.User.Movement.weightCrouch;
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

        private void TrackStamina()
        {
            if (secForRecoverStamina > 2)
            {
                // 마지막으로 뛴 순간으로부터 2초 후
                // = 스테미나 회복 시작
                InGameStatus.User.status.staminaBar.AddCurrent(Time.deltaTime * 10);
                return;
            }
            secForRecoverStamina += Time.deltaTime;
        }
    }

}