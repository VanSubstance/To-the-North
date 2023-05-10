using Assets.Scripts.Events.Abstracts;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts.Events.Controllers
{
    public class EventRootingController : AbsEventBaseController
    {
        [SerializeField]
        private bool isItemFixed;
        [SerializeField]
        private ItemBaseInfo[] itemsFixed;
        [SerializeField]
        private int amountIfRandom;
        [SerializeField]
        private ItemInfoWithWeight[] itemsRandom;

        /// <summary>
        /// SpaceBar 눌렀을 때 작동하는 함수
        /// </summary>
        public override void OnInteraction()
        {
            Debug.Log("루팅 이벤트 발생!");
        }

        [System.Serializable]
        private class ItemInfoWithWeight
        {
            public ItemBaseInfo info;
            public int weight;
        }
    }
}
