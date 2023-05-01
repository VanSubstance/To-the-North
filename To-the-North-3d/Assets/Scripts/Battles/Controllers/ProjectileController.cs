using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures;
using Assets.Scripts.Items;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    internal class ProjectileController : MonoBehaviour
    {
        [SerializeField]
        private ProjectileInfo info;
        private bool isReady;

        [SerializeField]
        private Transform lowHit;

        private SubHitDetectController subHit;

        public bool isAffected = false;

        public Vector3 startPos, targetPos;
        public ProjectileInfo Info
        {
            get
            {
                return info;
            }
        }

        public AttackInfo AttackInfo
        {
            get
            {
                return info.AttackInfo;
            }
        }

        private Transform owner;
        public Transform Owner
        {
            get
            {
                return owner;
            }
        }

        private TrajectoryController trajectory;
        [SerializeField]
        private AudioClip[]
            audGunPerLoudness,
            audArrowPerLoudness,
            audSwingPerLoudness;

        private void Awake()
        {
            if ((subHit = lowHit.GetComponent<SubHitDetectController>()) == null)
            {
                subHit = lowHit.AddComponent<SubHitDetectController>();
            }
            subHit.Parent = this;
            subHit.SetActive(false);
            gameObject.SetActive(false);
        }

        public void Fire(ProjectileInfo _info, Vector3 startPos, Vector3 targetDir, Transform _owner, ItemBulletType bulletType)
        {
            float h = startPos.y;
            trajectory = TrajectoryManager.Instance.GetNewTrajectory();
            owner = _owner;
            startPos = new Vector3(startPos.x, 0, startPos.z);
            targetDir = new Vector3(targetDir.x, 0, targetDir.z); ;
            this.startPos = startPos;

            info = ProjectileInfo.GetClone(_info);
            subHit.size = GetComponent<BoxCollider>().size = new Vector3(0.2f, info.Height, .2f);
            subHit.SetActive(true);

            transform.localRotation = Quaternion.Euler(0f, 0f, CalculationFunctions.AngleFromDir(new Vector2(targetDir.x, targetDir.z)));
            transform.position = new Vector3(startPos.x, h, startPos.z);
            gameObject.SetActive(true);
            GetComponent<Rigidbody>().velocity = new Vector3(targetDir.x, 0, targetDir.z).normalized * info.Spd;
            targetPos = startPos + targetDir.normalized * _info.Range;
            targetPos = new Vector3(targetPos.x, h, targetPos.z);
            isAffected = false;
            isReady = true;
            if (info.TrajectoryType == TrajectoryType.Curve)
            {
                trajectory.PlayCurve(startPos, CalculationFunctions.AngleFromDir(new Vector2(targetDir.x, targetDir.z)), 45, _info.Range);
                trajectory = null;
            }
            ImpactSound(bulletType);
        }

        /// <summary>
        /// 시작될 때 소리 관련 처리 함수
        /// </summary>
        private void ImpactSound(ItemBulletType bulletType)
        {
            AudioClip c = null;
            switch (bulletType)
            {
                case ItemBulletType.None:
                    c = audSwingPerLoudness[info.LevelLoudness];
                    break;
                case ItemBulletType.Bullet_mm9:
                    c = audGunPerLoudness[info.LevelLoudness];
                    break;
                case ItemBulletType.Arrow:
                    c = audArrowPerLoudness[info.LevelLoudness];
                    break;
            }
            // 소리 재생
            SoundEffects.SoundEffectManager.Instance.GetNewSoundEffect().PlaySound(transform.position, c, info.LevelLoudness * 10);
            // 소리 수준에 따른 주변 크리쳐들에게 전달
            Collider[] enemies;
            if ((enemies = Physics.OverlapSphere(transform.position, info.LevelLoudness * 10, GlobalStatus.Constant.creatureMask)).Length > 0)
            {
                IInteractionWithSight s;
                foreach (Collider enemy in enemies)
                {
                    if ((s = enemy.GetComponent<IInteractionWithSight>()) != null)
                    {
                        s.DetectSound(transform.position);
                    }
                }
            }
        }

        public void Arrive()
        {
            if (isAffected) return;
            isAffected = true;
            if (trajectory)
            {
                trajectory.Finish();
            }
            subHit.SetActive(false);
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!GlobalStatus.Loading.System.CommonGameManager)
            {
                // 파괴
                Arrive();
            }
            if (!isReady) return;
            if (trajectory)
            {
                trajectory.MoveTo(transform.position);
            }
            if (Vector3.Distance(transform.position, startPos) >= Vector3.Distance(targetPos, startPos))
            {
                Arrive();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.layer)
            {
                case 7:
                    Arrive();
                    break;
                case 17:
                    // 크리쳐 충돌
                    other.GetComponent<PartHitController>().CheckProjectileHit(transform);
                    break;
            }
        }

        private void OnDisable()
        {
            startPos = Vector3.zero;
            targetPos = Vector3.zero;
            isReady = false;
            transform.position = Vector3.zero;
            trajectory = null;
        }

        public class SubHitDetectController : MonoBehaviour
        {
            private BoxCollider hit;
            private ProjectileController parent;

            public Vector3 size
            {
                set
                {
                    hit.size = value;
                }
            }

            public ProjectileController Parent
            {
                set
                {
                    parent = value;
                }
                get
                {
                    return parent;
                }
            }

            public void SetActive(bool active)
            {
                gameObject.SetActive(active);
            }

            private void Awake()
            {
                hit = GetComponent<BoxCollider>();
            }
            private void OnTriggerEnter(Collider other)
            {
                switch (other.gameObject.layer)
                {
                    case 7:
                        gameObject.SetActive(false);
                        break;
                    case 17:
                        // 크리쳐 충돌
                        other.GetComponent<PartHitController>().CheckProjectileHit(parent.transform);
                        break;
                }
            }

            private void Update()
            {
                transform.position = parent.transform.position + Vector3.down;
            }
        }
    }
}
