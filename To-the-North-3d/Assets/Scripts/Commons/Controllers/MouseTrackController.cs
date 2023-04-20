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
            Vector3 nextPos;
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nextPos = transform.localPosition;
            transform.localPosition = new Vector3(
                nextPos.x,
                nextPos.y - transform.GetComponent<RectTransform>().sizeDelta.y,
                0
                );
        }
    }
}
