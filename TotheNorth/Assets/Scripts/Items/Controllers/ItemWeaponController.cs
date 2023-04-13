using Assets.Scripts.Battles;
using UnityEngine;
using static GlobalComponent.Common;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// 손에 장착 가능 + 무기 컨트롤러
    /// </summary>
    internal class ItemWeaponController : MonoBehaviour, IItemHandable
    {
        [SerializeField]
        private ItemWeaponInfo info;
        private Transform owner;

        private float delayAmongFire, timeFocus, timeFocusFull = 3f;
        private bool isAiming;
        private void Awake()
        {
            info.projectileInfo.PowerKnockback = info.powerKnockback;
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
                ProjectileManager.Instance.GetNewProjectile().Fire(info.range, info.projectileInfo, transform.position,
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
