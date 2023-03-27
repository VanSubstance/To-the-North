using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Maps.Controllers
{
    internal class ObstacleController : BaseSpriteController
    {
        private static ContactFilter2D noFilterOption = new ContactFilter2D().NoFilter();
        private void Awake()
        {
            Collider2D[] list = new Collider2D[10];
            Physics2D.OverlapCollider(GetComponent<CompositeCollider2D>(), noFilterOption, list);
            foreach (Collider2D col in list)
            {
                //if (col != null)
                //    Debug.Log(col);
                // @양승혁
            }
        }
    }
}
