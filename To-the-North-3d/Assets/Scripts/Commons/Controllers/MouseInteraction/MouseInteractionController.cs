using UnityEngine;

namespace Assets.Scripts.Commons
{
    public abstract class MouseInteractionController : MonoBehaviour, IMouseInteractable
    {
        public static void AttachMouseInteractor<T>(Transform _tf) where T : MouseInteractionController
        {
            _tf.gameObject.AddComponent<T>();
            if (!_tf.TryGetComponent(typeof(Collider), out Component col))
            {
                BoxCollider t = _tf.gameObject.AddComponent<BoxCollider>();
                t.size = _tf.gameObject.GetComponent<RectTransform>().sizeDelta;
                //t.center = new Vector3(t.size.x / 2, -t.size.y / 2, 1);
            }
        }

        private void OnMouseDown()
        {
            ActionMouseDown();
        }

        private void OnMouseDrag()
        {
            ActionMouseDrag();
        }
        private void OnMouseUp()
        {
            ActionMouseUp();
        }

        public abstract void ActionMouseDown();
        public abstract void ActionMouseDrag();
        public abstract void ActionMouseUp();
    }
}
