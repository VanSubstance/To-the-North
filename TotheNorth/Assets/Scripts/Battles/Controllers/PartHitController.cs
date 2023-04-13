using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    internal class PartHitController : MonoBehaviour
    {
        [SerializeField]
        private PartType partType;
        private CreatureHitController hitController;
        private ItemArmorInfo info;

        private void Awake()
        {
            bool isArmor = false;
            if (transform.childCount == 0)
            {
                info = ItemArmorInfo.GetPlainArmor();
            }
            else
            {
                if ((info = transform.GetChild(0).GetComponent<ItemArmorController>().Info) == null)
                {
                    info = ItemArmorInfo.GetPlainArmor();
                }
                else
                {
                    isArmor = true;
                }
            }
            switch (partType)
            {
                case PartType.Helmat:
                case PartType.Mask:
                    if (!isArmor)
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case PartType.Head:
                    break;
                case PartType.Body:
                    break;
                case PartType.Leg:
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Attack"))
            {
                ProjectileController prj = collision.GetComponent<ProjectileController>();
                if (prj.isAffected) return;
                if (prj.Owner.Equals(hitController.Owner)) return;
                hitController.OnHit(partType, info, prj.Info.AttackInfo, (prj.startPos - transform.position));
                prj.Arrive();
            }
        }

        public void SetHitController(CreatureHitController _hitController)
        {
            hitController = _hitController;
        }
    }
}
