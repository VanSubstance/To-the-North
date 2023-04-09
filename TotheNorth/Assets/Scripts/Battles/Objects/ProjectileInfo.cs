using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    [CreateAssetMenu(fileName = "Projectile Info", menuName = "Data Objects/Projectile Info", order = int.MaxValue)]
    public class ProjectileInfo : ScriptableObject
    {
        [SerializeField]
        private Vector3 endPos;
        [SerializeField]
        private float spd;

        public Vector3 EndPos
        {
            get
            {
                return endPos;
            }
            set
            {
                endPos = value;
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
        public static ProjectileInfo GetClone(ProjectileInfo _info)
        {
            ProjectileInfo res = CreateInstance<ProjectileInfo>();
            res.endPos = _info.endPos;
            res.spd = _info.spd;
            return res;
        }

        //public static ProjectileInfo GenerateInfo(WeaponInfo weapon)
        //{
        //    ProjectileInfo res = CreateInstance<ProjectileInfo>();
        //    return res;
        //}
    }
}
