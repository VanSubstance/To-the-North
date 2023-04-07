using UnityEngine;

namespace Assets.Scripts.Maps
{
    internal class ObstacleController : BaseSpriteController
    {
        [SerializeField]
        private bool isDig = false;
        public bool IsDig
        {
            get
            {
                return isDig;
            }
        }
        public int CurrentLayer
        {
            get
            {
                return gameObject.layer;
            }
        }
        private static ContactFilter2D filterOption = new ContactFilter2D();
        private new void Awake()
        {
            base.Awake();
            if (isDig) spriteBottom.color = new Color(.2f, .2f, .2f, .5f);
            filterOption.SetLayerMask(GlobalStatus.Constant.obstacleMask);
        }

        public Collider2D[] GetContactedColliders()
        {
            Collider2D[] list;
            list = new Collider2D[20];
            Physics2D.OverlapCollider(spriteBottom.GetComponent<PolygonCollider2D>(), filterOption, list);
            return list;
        }

        public void Composite(Transform parent)
        {
            gameObject.layer = 14;
            spriteBottom.gameObject.layer = 0;
            transform.SetParent(parent);
        }
    }
}
