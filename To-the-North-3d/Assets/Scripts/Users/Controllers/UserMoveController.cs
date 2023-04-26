using Assets.Scripts.Creatures;
using UnityEngine;

namespace Assets.Scripts.Users
{
    internal class UserMoveController : AbsCreatureActionController
    {
        private float secForRecoverStamina = 0;
        private Rigidbody rigid;
        private KeyCode keyCodeLast;

        private Vector3 dodgeDir = Vector3.zero;
        private float timeLastKeyTrack, timeDodgeTrack, spdDodge = 10;
        private bool isDodging;
        private float WforStamina
        {
            get
            {
                return 1 * (InGameStatus.User.IsConditionExist(ConditionConstraint.PerformanceLack.SpeedUseStamina) ? 1.5f : 1);
            }
        }

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            keyCodeLast = KeyCode.None;
            timeDodgeTrack = 0;
            timeLastKeyTrack = 0;
            isDodging = false;
            isCrouching = false;
        }
        private void Update()
        {
            if (!InGameStatus.User.isPause)
            {
                TrackStamina();
            }
        }

        private void LateUpdate()
        {
            if (!InGameStatus.User.isPause)
            {
                if (isDodging)
                {
                    TrackDodge();
                }
                else
                {
                    TrackLastKey();
                    TrackDirection();
                    TrackMovementType();
                }
            }
        }

        private void TrackLastKey()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                keyCodeLast = KeyCode.A;
                timeLastKeyTrack = 0;
                return;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                keyCodeLast = KeyCode.S;
                timeLastKeyTrack = 0;
                return;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                keyCodeLast = KeyCode.D;
                timeLastKeyTrack = 0;
                return;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                keyCodeLast = KeyCode.W;
                timeLastKeyTrack = 0;
                return;
            }
            if (timeLastKeyTrack > 0.1f)
            {
                keyCodeLast = KeyCode.None;
                return;
            }
            timeLastKeyTrack += Time.deltaTime;
        }

        private void TrackDodge()
        {
            if (timeDodgeTrack <= 0.7f)
            {
                if (timeDodgeTrack <= 0.2f)
                {
                    InGameStatus.User.status.staminaBar.LiveInfo = (-Time.deltaTime * 100 * WforStamina);
                }
                timeDodgeTrack += Time.deltaTime;
                rigid.velocity = dodgeDir * spdDodge * (1 + Mathf.Log(1 - timeDodgeTrack));
                return;
            }
            rigid.velocity = Vector2.zero;
            keyCodeLast = KeyCode.None;
            dodgeDir = Vector3.zero;
            isDodging = false;
        }

        private void TrackDirection()
        {
            float spdW = GetMovementSpd();
            bool isMoving = false;
            Vector3 vecHor = Vector3.zero, vecVer = Vector3.zero, vecToMove = Vector3.zero;
            if (Input.GetKey(KeyCode.A))
            {
                if (keyCodeLast == KeyCode.A)
                {
                    // 구르기
                    Dodge(Vector3.left);
                    return;
                }
                isMoving = true;
                // 왼쪽
                if (InGameStatus.User.Movement.curMovement == Objects.MovementType.RUN)
                {
                    secForRecoverStamina = 0;
                    InGameStatus.User.status.staminaBar.LiveInfo = (-Time.deltaTime * 20 * WforStamina);
                }
                vecToMove += Vector3.left * spdW;
                vecHor += Vector3.left;
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (keyCodeLast == KeyCode.S)
                {
                    // 구르기
                    Dodge(Vector3.back);
                    return;
                }
                isMoving = true;
                // 아래쪽
                if (InGameStatus.User.Movement.curMovement == Objects.MovementType.RUN)
                {
                    secForRecoverStamina = 0;
                    InGameStatus.User.status.staminaBar.LiveInfo = (-Time.deltaTime * 20 * WforStamina);
                }
                vecToMove += Vector3.back * spdW;
                vecVer += Vector3.back;
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (keyCodeLast == KeyCode.D)
                {
                    // 구르기
                    Dodge(Vector3.right);
                    return;
                }
                isMoving = true;
                // 오른쪽
                if (InGameStatus.User.Movement.curMovement == Objects.MovementType.RUN)
                {
                    secForRecoverStamina = 0;
                    InGameStatus.User.status.staminaBar.LiveInfo = (-Time.deltaTime * 20 * WforStamina);
                }
                vecToMove += Vector3.right * spdW;
                vecVer += Vector3.right;
            }
            if (Input.GetKey(KeyCode.W))
            {
                if (keyCodeLast == KeyCode.W)
                {
                    // 구르기
                    Dodge(Vector3.forward);
                    return;
                }
                isMoving = true;
                // 위쪽
                if (InGameStatus.User.Movement.curMovement == Objects.MovementType.RUN)
                {
                    secForRecoverStamina = 0;
                    InGameStatus.User.status.staminaBar.LiveInfo = (-Time.deltaTime * 20 * WforStamina);
                }
                vecToMove += Vector3.forward * spdW;
                vecVer += Vector3.forward;
            }
            if (isMoving)
            {
                float w = 1;
                if (InGameStatus.User.IsConditionExist(ConditionConstraint.PerformanceLack.SpeedMove))
                {
                    w /= 2;
                }
                rigid.velocity = vecToMove * w;
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
                    if (InGameStatus.User.status.staminaBar.LiveInfo > 0)
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
                if (InGameStatus.User.IsConditionExist(ConditionConstraint.UtilBlock.Run))
                {
                    // 상태 이상으로 인해 달릴 수 없음
                    return;
                }
                InGameStatus.User.Movement.curMovement = Objects.MovementType.RUN;
                return;
            }
            if (Input.GetKey(KeyCode.LeftControl))
            {
                InGameStatus.User.Movement.curMovement = Objects.MovementType.CROUCH;
                Crouch();
                return;
            }
            InGameStatus.User.Movement.curMovement = Objects.MovementType.WALK;
            Stand();
        }

        private void TrackStamina()
        {
            if (secForRecoverStamina > 2)
            {
                // 마지막으로 뛴 순간으로부터 2초 후
                // = 스테미나 회복 시작
                int weight = 10;
                if (InGameStatus.User.IsConditionExist(ConditionConstraint.PerformanceLack.RecoveryStamina))
                {
                    weight /= 2;
                }
                InGameStatus.User.status.staminaBar.LiveInfo = (Time.deltaTime * (float)weight);
                return;
            }
            secForRecoverStamina += Time.deltaTime;
        }

        public override void Dodge(Vector3 dir)
        {
            if (InGameStatus.User.IsConditionExist(ConditionConstraint.UtilBlock.Dodge)) return;
            if (InGameStatus.User.status.staminaBar.LiveInfo <= 0) return;
            dodgeDir = dir;
            timeDodgeTrack = 0;
            secForRecoverStamina = 0;
            isDodging = true;
            keyCodeLast = KeyCode.None;
        }
    }

}