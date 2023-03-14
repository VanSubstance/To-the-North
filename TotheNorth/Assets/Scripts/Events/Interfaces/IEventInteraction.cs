using UnityEngine;

namespace Assets.Scripts.Events.Interfaces
{
    internal interface IEventInteraction
    {
        public void StartTrackingInteraction(Transform targetTf);
        public void OnInteraction();
    }
}
