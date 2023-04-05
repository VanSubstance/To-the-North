using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Battles.Managers;
using UnityEngine;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// 손에 장착 가능 + 원거리 무기용 컨트롤러
    /// </summary>
    internal class ItemRangeController : MonoBehaviour, IItemHandable
    {
        [SerializeField]
        private ItemBaseInfo info;

        private float delayAmongFire = 0f;
        private void Awake()
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(info.imagePath);
            delayAmongFire = info.delayAmongFire;
        }

        private void Update()
        {
            if (delayAmongFire >= info.delayAmongFire) return;
            delayAmongFire += Time.deltaTime;
        }
        public void Aim(Vector3 targetDir)
        {
            //Debug.Log("원거리 조준중 ...");
        }

        public void Use(Vector3 targetDir)
        {
            // 투사체 발사
            if (delayAmongFire >= info.delayAmongFire)
            {
                ProjectileManager.Instance.GetNewProjectile().Fire(info.projectileInfo, transform.position, targetDir);
                delayAmongFire = 0f;
            }
        }

        public float Range()
        {
            return info.range;
        }
    }
}
