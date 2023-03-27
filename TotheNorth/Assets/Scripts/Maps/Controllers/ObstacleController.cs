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
        private static ContactFilter2D filterOption = new ContactFilter2D();
        private void Awake()
        {
            filterOption.SetLayerMask(GlobalStatus.Constant.obstacleMask);
        }

        public Collider2D[] GetContactedColliders()
        {
            Collider2D[] list;
            list = new Collider2D[20];
            Physics2D.OverlapCollider(backSprite.GetComponent<PolygonCollider2D>(), filterOption, list);
            return list;
        }

        public void Composite(Transform parent)
        {
            gameObject.layer = 14;
            backSprite.gameObject.layer = 0;
            transform.SetParent(parent);
        }
    }
}
