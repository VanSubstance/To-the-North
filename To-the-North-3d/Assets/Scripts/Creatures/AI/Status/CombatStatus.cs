using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures.AI.Status
{
    public class CombatStatus : IAIStatus
    {
        public void UpdateAction(AIBaseController mover, Vector3? target)
        {
            Debug.Log("전투 상태");
        }
    }
}
