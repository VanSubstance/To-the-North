using UnityEngine;

namespace Assets.Scripts.Battles
{
    [CreateAssetMenu(fileName = "Projectile Info", menuName = "Data Objects/Projectile Info", order = int.MaxValue)]
    public class ProjectileInfo : ScriptableObject
    {
        [SerializeField]
        private float spd;
        [SerializeField]
        private float heightCollider, powerKnockback;
        [SerializeField]
        private TrajectoryType trajectoryType;

        public float PowerKnockback
        {
            set
            {
                powerKnockback = value;
            }
            get
            {
                return powerKnockback;
            }
        }

        public float Spd
        {
            get
            {
                return spd;
            }
            set
            {
                spd = value;
            }
        }

        public float Height
        {
            get
            {
                return heightCollider;
            }
        }

        public TrajectoryType TrajectoryType
        {
            get
            {
                return trajectoryType;
            }
        }
        public static ProjectileInfo GetClone(ProjectileInfo _info)
        {
            ProjectileInfo res = CreateInstance<ProjectileInfo>();
            res.spd = _info.spd;
            res.heightCollider = _info.heightCollider;
            res.powerKnockback = _info.powerKnockback;
            res.trajectoryType = _info.trajectoryType;
            return res;
        }
    }
}
