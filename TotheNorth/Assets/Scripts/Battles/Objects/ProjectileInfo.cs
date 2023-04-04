using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    [CreateAssetMenu(fileName = "Projectile Info", menuName = "Data Objects/Projectile Info", order = int.MaxValue)]
    internal class ProjectileInfo : ScriptableObject
    {
        public static ProjectileInfo GetClone(ProjectileInfo _info)
        {
            ProjectileInfo res = CreateInstance<ProjectileInfo>();
            return res;
        }

        //public static ProjectileInfo GenerateInfo(WeaponInfo weapon)
        //{
        //    ProjectileInfo res = CreateInstance<ProjectileInfo>();
        //    return res;
        //}
    }
}
