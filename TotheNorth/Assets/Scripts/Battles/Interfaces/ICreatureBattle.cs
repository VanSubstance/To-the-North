using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    public interface ICreatureBattle
    {
        public void OnHit(PartType partType, ProjectileInfo _info, Vector3 hitDir);
    }
}
