using System;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    class CreatureHitController : MonoBehaviour
    {
        private CreatureInfo info;
        public CreatureInfo Info
        {
            set
            {
                info = value;
            }
        }
        private void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<PartHitController>().SetHitController(this);
            }
        }

        /// <summary>
        /// 피격당했을 때 작동하는 함수
        /// </summary>
        /// <param name="partType">피격당한 부위</param>
        public void OnHit(PartType partType, ProjectileInfo _info)
        {
            switch (partType)
            {
                case PartType.Helmat:
                    break;
                case PartType.Mask:
                    break;
                case PartType.Head:
                    break;
                case PartType.Body:
                    break;
                case PartType.Leg:
                    break;
            }
            if (info)
            {
                // AI 기준
                info.LiveHp = -10;
                if (info.LiveHp <= 0)
                {
                    transform.parent.gameObject.SetActive(false);
                }
                return;
            }
            // 유저
            InGameStatus.User.status.hpBar.LiveInfo = -10;
            if (InGameStatus.User.status.hpBar.LiveInfo <= 0)
            {
                //InGameStatus.User.isPause = true;
            }
            return;
        }
    }
}
