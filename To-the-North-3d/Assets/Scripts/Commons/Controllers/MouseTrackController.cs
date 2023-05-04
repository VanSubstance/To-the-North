using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class MouseTrackController : MonoBehaviour
    {
        protected void Update()
        {
            TracksMouse();
        }

        protected void TracksMouse()
        {
            Vector3 nextPos = UIManager.Instance.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            transform.localPosition = new Vector3(
                nextPos.x,
                nextPos.y,
                0);
        }
    }
}
