using UnityEngine;

namespace Assets.Scripts.Battles
{
    internal class PartHitController : MonoBehaviour
    {
        [SerializeField]
        private PartType partType;
        private CreatureHitController hitController;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Equals("Attack"))
            {
                hitController.OnHit(partType);
            }
        }

        public void SetHitController(CreatureHitController _hitController)
        {
            hitController = _hitController;
        }
    }
}