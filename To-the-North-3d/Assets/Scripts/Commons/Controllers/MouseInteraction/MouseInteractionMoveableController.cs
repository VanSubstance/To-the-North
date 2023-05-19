using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class MouseInteractionMoveableController : MouseInteractionController
    {
        private Vector3 correctionVector;
        public override void ActionMouseDown()
        {
            Vector3 startPos = UIManager.Instance.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            correctionVector = transform.localPosition - startPos;
        }

        public override void ActionMouseDrag()
        {
            Vector3 nextPos = UIManager.Instance.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            transform.localPosition = new Vector3(
                nextPos.x + correctionVector.x,
                nextPos.y + correctionVector.y,
                0);
        }

        public override void ActionMouseUp()
        {
        }
    }
}
