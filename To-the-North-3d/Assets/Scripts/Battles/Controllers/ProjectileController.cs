using Assets.Scripts.Commons.Functions;
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

        public void Fire(ProjectileInfo _info, Vector3 startPos, Vector3 targetDir, Transform _owner)
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
        private void OnCollisionEnter(Collision collision)
        {
            switch (collision.gameObject.layer)
            {
                case 7:
                    Arrive();
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
            private void OnCollisionEnter(Collision collision)
            {
                switch (collision.gameObject.layer)
                {
                    case 7:
                        gameObject.SetActive(false);
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
