using Assets.Scripts.Battles;
using UnityEngine;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// 손에 장착 가능 + 원거리 무기용 컨트롤러
    /// </summary>
    internal class ItemRangeController : MonoBehaviour, IItemHandable
    {
        [SerializeField]
        private ItemWeaponInfo info;
        private Transform owner;

        private float delayAmongFire, timeFocus, timeFocusFull = 3f;
        private bool isAiming;
        private void Awake()
        {
            delayAmongFire = 0f;
            timeFocus = 0f;
            isAiming = false;
            owner = transform.parent.parent.parent;
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(info.imagePath);
            delayAmongFire = info.delayAmongFire;
        }

        private void Update()
        {
            if (delayAmongFire >= info.delayAmongFire) return;
            delayAmongFire += Time.deltaTime;
            if (!isAiming)
            {
                timeFocus = Mathf.Max(timeFocus - Time.deltaTime, 0);
            }
            isAiming = false;
        }
        public void Aim(Vector3 targetDir)
        {
            isAiming = true;
            timeFocus = Mathf.Min(timeFocus + Time.deltaTime, timeFocusFull);
        }

        public void Use(Vector3 targetDir)
        {
            // 투사체 발사
            if (delayAmongFire >= info.delayAmongFire)
            {
                float randRange = (3 - timeFocus) / 3f;
                ProjectileManager.Instance.GetNewProjectile().Fire(info.projectileInfo, transform.position,
                    new Vector2(targetDir.x + randRange * Random.Range(-1f, 1f), targetDir.y + randRange * Random.Range(-1f, 1f))
                    , owner);
                delayAmongFire = 0f;
            }
        }

        public float Range()
        {
            return info.range;
        }
    }
}
