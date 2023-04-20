using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    internal class ProjectileController : MonoBehaviour
    {
        [SerializeField]
        private ProjectileInfo info;
        private bool isReady;

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
            trajectory = TrajectoryManager.Instance.GetNewTrajectory();
            owner = _owner;
            this.startPos = startPos;

            info = ProjectileInfo.GetClone(_info);
            GetComponent<BoxCollider>().size = new Vector2(0.2f, info.Height);
            transform.localRotation = Quaternion.Euler(0f, 0f, CalculationFunctions.AngleFromDir(targetDir));
            transform.position = startPos;
            gameObject.SetActive(true);
            GetComponent<Rigidbody>().velocity = targetDir.normalized * info.Spd;
            targetPos = startPos + targetDir.normalized * _info.Range;
            isAffected = false;
            isReady = true;
            if (info.TrajectoryType == TrajectoryType.Curve)
            {
                trajectory.PlayCurve(startPos, CalculationFunctions.AngleFromDir(targetDir), 45, _info.Range);
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
            gameObject.SetActive(false);
        }

        private void Awake()
        {
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
                case 14:
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
    }
}
