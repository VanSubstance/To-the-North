using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class MouseInteractionMoveableController : MouseInteractionController
    {
        public override void ActionMouseDown()
        {
            Debug.Log($"Down");
        }

        public override void ActionMouseDrag()
        {
            Debug.Log($"Drag");
        }

        public override void ActionMouseUp()
        {
            Debug.Log($"Up");
        }
    }
}
