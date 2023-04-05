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
            if (collision.CompareTag("Attack"))
            {
                ProjectileController prj = collision.GetComponent<ProjectileController>();
                if (prj.isAffected) return;
                prj.Arrive();
                hitController.OnHit(partType, prj.Info);
            }
        }

        public void SetHitController(CreatureHitController _hitController)
        {
            hitController = _hitController;
        }
    }
}
