using Assets.Scripts.Battles;
using Assets.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    public abstract class AbsCreatureBaseController : MonoBehaviour, ICreatureBattle
    {

        [SerializeField]
        protected Transform hitTf, visualTf;
        protected Dictionary<EquipBodyType, IItemEquipable> equipableBodies = new Dictionary<EquipBodyType, IItemEquipable>();
        [SerializeField]
        protected ItemWeaponController weaponL, weaponR;

        protected void Awake()
        {
            equipableBodies[EquipBodyType.Helmat] = hitTf.GetChild(0).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Mask] = hitTf.GetChild(1).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Head] = hitTf.GetChild(2).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Body] = hitTf.GetChild(3).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Leg] = hitTf.GetChild(4).GetChild(0).GetComponent<ItemArmorController>();
            equipableBodies[EquipBodyType.Right] = weaponL;
            equipableBodies[EquipBodyType.Left] = weaponR;
            weaponL.Owner = transform;
            weaponR.Owner = transform;
        }

        public abstract void OnHit(EquipBodyType partType, ItemArmorInfo armorInfo, AttackInfo attackInfo, int[] damage, Vector3 hitDir);
    }
}
