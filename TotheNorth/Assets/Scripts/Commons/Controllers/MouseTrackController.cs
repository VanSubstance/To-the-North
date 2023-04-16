using UnityEngine;

namespace Assets.Scripts.Commons
{
    internal class MouseTrackController : MonoBehaviour
    {
        private void Update()
        {
            Vector3 nextPos;
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            nextPos = transform.localPosition;
            transform.localPosition = new Vector3(nextPos.x, nextPos.y, 0);
        }
    }
}
