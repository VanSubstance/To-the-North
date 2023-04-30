using Assets.Scripts.Items;
using UnityEngine;
using static Assets.Scripts.Battles.ProjectileController;

namespace Assets.Scripts.Battles
{
    internal class PartHitController : MonoBehaviour
    {
        [SerializeField]
        private bool isFixed;
        private Vector3 originPos;

        private CreatureHitController hitController;
        public Transform Owner
        {
            get { return hitController.Owner; }
        }
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
            originPos = transform.localPosition;
        }

        private void Update()
        {
            //if (Vector3.Distance(transform.localPosition, originPos) > 0.1f)
            //{
            //    // 돌아가려고 노력은 해야지
            //    transform.Translate(
            //        (originPos - transform.localPosition).normalized * Time.deltaTime * 10
            //        );
            //}
        }

        private void OnTriggerEnter(Collider collision)
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
        private void OnTriggerStay(Collider other)
        {
            if (isFixed) return;
            //Debug.Log(other.ClosestPointOnBounds(transform.position));
            Debug.Log(GetComponent<BoxCollider>().ClosestPointOnBounds(other.transform.position));
        }

        public static EquipBodyType DecideHitPart()
        {
            /** 피격 판정은 핼멧, 마스크, 가방, 몸통 중 하나 랜덤하여 진행한다
             * 확률은 아래와 같음
             * 헬멧 : 25
             * 마스크 : 5
             * 가방: 10
             * 몸통 : 60
             * 헬멧, 마스크가 없을 경우 <- 머리 <- 다이렉트 데미지
             * 가방이 없을 경우 <- 몸통
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
