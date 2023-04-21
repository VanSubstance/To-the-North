using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers.Creatures
{
    internal class NpcBaseController : AIBaseController
    {

        public override void OnDetectUser(Vector3? targetPos)
        {
            Debug.Log("User Detect!!");
        }
    }
}
