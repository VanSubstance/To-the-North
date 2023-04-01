using System;
using UnityEngine;

namespace Assets.Scripts.Battles
{
    class CreatureHitController : MonoBehaviour
    {
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
        public void OnHit(PartType partType)
        {
            switch (partType)
            {
                case PartType.Helmat:
                    Debug.Log("헬멧 맞음!");
                    break;
                case PartType.Mask:
                    Debug.Log("마스크 맞음!");
                    break;
                case PartType.Head:
                    Debug.Log("머리 맞음!");
                    break;
                case PartType.Body:
                    Debug.Log("몸텅 맞음!");
                    break;
                case PartType.Leg:
                    Debug.Log("다리 맞음!");
                    break;
            }
        }
    }
}
