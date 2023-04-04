using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Items;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Users.Controllers
{
    internal class UserMouseController : MonoBehaviour
    {
        [SerializeField]
        Transform handL, handR;
        private void Update()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TrackMousePosition(mousePos);
            TrackAim(mousePos);
            TrackAttack(mousePos);
        }

        private void TrackMousePosition(Vector3 mousePos)
        {
            if (!InGameStatus.User.isPause)
            {
                InGameStatus.User.Movement.curdegree = (int)
                Vector3.SignedAngle(Vector3.right, new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized, transform.forward);
            }
        }

        private void TrackAim(Vector3 mousePos)
        {
            if (Input.GetMouseButton((int)MouseButton.RightMouse))
            {
                CameraTrackControlller.targetPos =
                    (
                    mousePos - GlobalComponent.Common.userTf.position
                    )
                    * 2 / 3f;
                if (handL.childCount > 0)
                {
                    handL.GetChild(0).GetComponent<IItemHandable>().Aim(
                    mousePos - GlobalComponent.Common.userTf.position);
                }
                if (handR.childCount > 0)
                {
                    handR.GetChild(0).GetComponent<IItemHandable>().Aim(
                    mousePos - GlobalComponent.Common.userTf.position);
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
                if (handL.childCount > 0)
                {
                    handL.GetChild(0).GetComponent<IItemHandable>().Use(
                    mousePos - GlobalComponent.Common.userTf.position);
                }
                if (handR.childCount > 0)
                {
                    handR.GetChild(0).GetComponent<IItemHandable>().Use(
                    mousePos - GlobalComponent.Common.userTf.position);
                }
            }
        }
    }
}
