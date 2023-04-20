using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    internal class PartHitController : MonoBehaviour
    {
        [SerializeField]
        private EquipBodyType partType;
        private CreatureHitController hitController;
        private ItemArmorInfo info;
        public ItemArmorInfo Info
        {
            set
            {
                info = value;
            }
        }

        private void Awake()
        {
            hitController = transform.parent.GetComponent<CreatureHitController>();
            bool isArmor = false;
            if (transform.childCount == 0)
            {
                Info = ItemArmorInfo.GetPlainArmor();
            }
            else
            {
                if ((Info = transform.GetChild(0).GetComponent<ItemArmorController>().Info) == null)
                {
                    Info = ItemArmorInfo.GetPlainArmor();
                }
                else
                {
                    isArmor = true;
                }
            }
            switch (partType)
            {
                case EquipBodyType.Helmat:
                case EquipBodyType.Mask:
                    if (!isArmor)
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case EquipBodyType.Head:
                    break;
                case EquipBodyType.Body:
                    break;
                case EquipBodyType.Leg:
                    break;
                case EquipBodyType.Back:
                    break;
            }
        }

        private void OnTriggerEnter(Collider collision)
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
    }
}
