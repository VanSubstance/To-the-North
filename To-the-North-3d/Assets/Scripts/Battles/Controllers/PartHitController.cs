using Assets.Scripts.Items;
using UnityEngine;
using static Assets.Scripts.Battles.ProjectileController;

namespace Assets.Scripts.Battles
{
    internal class PartHitController : MonoBehaviour
    {

        private CreatureHitController hitController;
        public Transform Owner
        {
            get
            {
                return hitController.Owner;
            }
        }
        private ItemArmorInfo info;
        public ItemArmorInfo Info
        {
            set
            {
                info = value;
            }
        }

        [SerializeField]
        private bool isFixed;
        [SerializeField]
        private float anchorZ, minZ;
        private Vector3 colSize;

        private BoxCollider bc;

        private void Awake()
        {
            hitController = transform.parent.GetComponent<CreatureHitController>();
            bc = GetComponent<BoxCollider>();
            colSize = bc.size;
            if (isFixed) return;
        }

        private void FixedUpdate()
        {
            if (isFixed) return;
            if (transform.localPosition.z > anchorZ || transform.localPosition.z < -anchorZ)
            {
                transform.localPosition = new Vector3(
                    0,
                    0,
                    .1f
                    );
            }
            float z = transform.localPosition.z;
            if (z < minZ)
            {
                // 콜라이더 크기 조절
                bc.size = new Vector3(colSize.x, colSize.y, colSize.z - (minZ - z) * 2);
            }
            else
            {
                bc.size = colSize;
            }
        }

        private void OnTriggerEnter(Collider collision)
        {
            CheckHit(collision.transform);
        }

        private void OnCollisionEnter(Collision collision)
        {
            CheckHit(collision.transform);
        }

        private void CheckHit(Transform collision)
        {
            ProjectileController prj = null;
            if (collision.CompareTag("Attack Low"))
            {
                prj = collision.GetComponent<SubHitDetectController>().Parent;
            }
            if (collision.CompareTag("Attack"))
            {
                prj = collision.GetComponent<ProjectileController>();
            }
            if (prj != null)
            {
                if (prj.isAffected) return;
                if (prj.Owner.Equals(hitController.Owner)) return;
                hitController.OnHit(info, prj.Info.AttackInfo, (prj.startPos - transform.position));
                prj.Arrive();
            }
        }

        public static EquipBodyType DecideHitPart()
        {
            /** 피격 판정은 핼멧, 마스크, 가방, 몸통 중 하나 랜덤하여 진행한다
             * 확률은 아래와 같음
             * 헬멧 : 25
             * 마스크 : 5
             * 가방: 10
             * 몸통 : 60
            */
            int p = Random.Range(0, 20);
            if (p < 5)
            {
                return EquipBodyType.Mask;
            }
            if (p < 30)
            {
                return EquipBodyType.Helmat;
            }
            if (p < 40)
            {
                return EquipBodyType.BackPack;
            }
            return EquipBodyType.Body;
        }
    }
}
