using Assets.Scripts.Events.Abstracts;
using UnityEngine;

namespace Assets.Scripts.Events.Controllers
{
    internal class EventPortalController : AEventBaseController
    {
        public override void OnInteraction()
        {
            Debug.Log("포탈 상호작용");
        }
    }
}
