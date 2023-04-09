using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Creatures;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    class CreatureHitController : MonoBehaviour
    {
        private ICreatureBattle battleFunction;
        private Transform ownerTf;
        public Transform Owner
        {
            get
            {
                return ownerTf;
            }
        }

        private void Awake()
        {
            ownerTf = transform.parent;
            battleFunction = transform.parent.GetComponent<ICreatureBattle>();
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<PartHitController>().SetHitController(this);
            }
        }

        /// <summary>
        /// 피격당했을 때 작동하는 함수
        /// </summary>
        /// <param name="partType">피격당한 부위</param>
        public void OnHit(PartType partType, ProjectileInfo _info, Vector3 hitPos)
        {
            battleFunction.OnHit(partType, _info, hitPos);
        }
    }
}
