using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Detections;
using Assets.Scripts.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Users.Controllers
{
    internal class UserMouseController : MonoBehaviour
    {
        [SerializeField]
        private DetectionSightController sightCtrl;
        [SerializeField]
        private ItemWeaponController weapon;
        private void Update()
        {
            if (!InGameStatus.User.isPause)
            {
                TrackMousePosition(CameraTrackControlller.MousePosOnTerrain);
                TrackAim(CameraTrackControlller.MousePosOnTerrain);
                TrackAttack(CameraTrackControlller.MousePosOnTerrain);
            }
        }

        private void TrackMousePosition(Vector3 mousePos)
        {
            Vector2 t = new Vector2(mousePos.x - transform.position.x, mousePos.z - 2 - transform.position.z);

            InGameStatus.User.Movement.curdegree = (int)CalculationFunctions.AngleFromDir(t);

            sightCtrl.SetTrackByDegree(InGameStatus.User.Movement.curdegree);
        }

        private void TrackAim(Vector3 mousePos)
        {
            if (Input.GetMouseButton((int)MouseButton.RightMouse))
            {
                if (InGameStatus.User.IsConditionExist(ConditionConstraint.UtilBlock.Aim))
                {
                    return;
                }
                CameraTrackControlller.targetPos =
                    (
                    mousePos - UserBaseController.Instance.position
                    )
                    * 2 / 3f;
                if (!weapon.IsEmpty())
                {
                    weapon.Aim(
                    mousePos - UserBaseController.Instance.position);
                }
                InGameStatus.User.Detection.Sight.isControllInRealTime = true;
            }
            if (Input.GetMouseButtonUp((int)MouseButton.RightMouse))
            {
                CameraTrackControlller.targetPos = Vector3.zero;
                InGameStatus.User.Detection.Sight.isControllInRealTime = false;
            }
        }

        private void TrackAttack(Vector3 mousePos)
        {
            if (Input.GetMouseButton((int)MouseButton.LeftMouse))
            {
                if (!weapon.IsEmpty())
                {
                    weapon.Use(
                    mousePos - UserBaseController.Instance.position);
                }
            }
        }
    }
}
