using Assets.Scripts.Events.Abstracts;
using UnityEngine;

namespace Assets.Scripts.Events.Controllers
{
    internal class EventPortalController : AbsEventBaseController
    {
        [SerializeField]
        private string targetSection;
        [SerializeField]
        private float[] targetInitLocation;

        private new void Awake()
        {
            base.Awake();
            if (targetSection == null || targetSection.Length == 0) Destroy(gameObject);
        }
        public override void OnInteraction()
        {
            GlobalStatus.userInitPosition = new float[] { targetInitLocation[0], targetInitLocation[1] };
            CommonGameManager.Instance.MoveScene(targetSection);
        }
    }
}
