using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    public class ProjectileInfo
    {
        private float spd, range;
        private float heightCollider, powerKnockback;
        private int levelLoudness;
        private TrajectoryType trajectoryType;
        private AttackInfo attackInfo;

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

        public int LevelLoudness
        {
            set
            {
                levelLoudness = value;
            }
            get
            {
                return levelLoudness;
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
            set
            {
                heightCollider = value;
            }
            get
            {
                return heightCollider;
            }
        }

        public TrajectoryType TrajectoryType
        {
            set
            {
                this.trajectoryType = value;
            }
            get
            {
                return trajectoryType;
            }
        }

        public AttackInfo AttackInfo
        {
            set
            {
                attackInfo = value;
            }
            get
            {
                return attackInfo;
            }
        }

        public float Range
        {
            get
            {
                return range;
            }

            set
            {
                range = value;
            }
        }

        public static ProjectileInfo GetClone(ProjectileInfo _info)
        {
            ProjectileInfo res = new()
            {
                spd = _info.spd,
                range = _info.range,
                heightCollider = _info.heightCollider,
                powerKnockback = _info.powerKnockback,
                trajectoryType = _info.trajectoryType,
                attackInfo = _info.attackInfo,
                levelLoudness = _info.levelLoudness,
            };
            return res;
        }
    }
}
