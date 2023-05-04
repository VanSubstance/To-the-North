using UnityEngine;

namespace Assets.Scripts.Events
{
    public interface IEventInteraction
    {
        public void StartTrackingInteraction(Transform targetTf);
        public void OnInteraction();
    }
}
