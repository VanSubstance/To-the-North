using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Users.Controllers
{
    internal class UserMouseController : MonoBehaviour
    {
        private ItemWeaponController weaponL, weaponR;

        private void Awake()
        {
            Transform temp = transform.Find("Hands");
            weaponR = temp.GetChild(0).GetChild(0).GetComponent<ItemWeaponController>();
            weaponL = temp.GetChild(1).GetChild(0).GetComponent<ItemWeaponController>();
        }
        private void Update()
        {
            if (!InGameStatus.User.isPause)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                TrackMousePosition(mousePos);
                TrackAim(mousePos);
                TrackAttack(mousePos);
            }
        }

        private void TrackMousePosition(Vector3 mousePos)
        {
            InGameStatus.User.Movement.curdegree = (int)
            Vector3.SignedAngle(Vector3.right, new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized, transform.forward);
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
                if (!weaponL.IsEmpty())
                {
                    weaponL.Aim(
                    mousePos - UserBaseController.Instance.position);
                }
                if (!weaponR.IsEmpty())
                {
                    weaponR.Aim(
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
                if (!weaponL.IsEmpty())
                {
                    weaponL.Use(
                    mousePos - UserBaseController.Instance.position);
                }
                if (!weaponR.IsEmpty())
                {
                    weaponR.Use(
                    mousePos - UserBaseController.Instance.position);
                }
            }
        }
    }
}
