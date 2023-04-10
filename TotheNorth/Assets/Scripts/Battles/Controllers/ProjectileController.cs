using Assets.Scripts.Commons.Functions;
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
            targetDir.z = 0f;

            this.startPos = startPos;

            info = ProjectileInfo.GetClone(_info);
            GetComponent<BoxCollider2D>().size = new Vector2(0.2f, info.Height);
            transform.localRotation = Quaternion.Euler(0f, 0f, CalculationFunctions.AngleFromDir(targetDir));
            transform.position = startPos;
            targetPos = LocalPostionToWorld(info.EndPos, targetDir);
            gameObject.SetActive(true);
            GetComponent<Rigidbody2D>().velocity = targetPos.normalized * info.Spd;
            targetPos *= (info.EndPos).magnitude;
            targetPos += transform.position;
            isAffected = false;
            isReady = true;
        }

        public void Arrive()
        {
            if (isAffected) return;
            isAffected = true;
            trajectory.Finish();
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void Update()
        {
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

        private void OnCollisionEnter2D(Collision2D collision)
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

        private Vector3 LocalPostionToWorld(Vector3 targetPos, Vector3 targetDir)
        {
            return CalculationFunctions.GetRotatedVector2(
                targetPos.magnitude * targetDir.normalized,
                CalculationFunctions.AngleFromDir(
                    targetPos.normalized
                    )
                ) * (1f + targetPos.magnitude);
        }
    }
}
