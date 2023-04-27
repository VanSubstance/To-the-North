using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Assets.Scripts.Items
{
    [CreateAssetMenu(fileName = "Magazine Info", menuName = "Data Objects/Items/Equipments/Magazine", order = int.MaxValue)]
    public class ItemMagazineInfo : ItemEquipmentInfo
    {
        [HideInInspector]
        public new EquipmentType equipmentType
        {
            get
            {
                return EquipmentType.Magazine;
            }
        }
        public ItemBulletType bulletType;
        [SerializeField]
        private int maxCount;
        private int curCount = 0;
        private int CurCount
        {

            get
            {
                return curCount;
            }
            set
            {
                curCount = value;
                if (amountDisplay)
                {
                    amountDisplay.text = curCount.ToString();
                }
            }
        }
        private Queue<ItemBulletInfo> bullets = new Queue<ItemBulletInfo>();
        private ItemBulletInfo currentBullet;

        private TextMeshProUGUI amountDisplay;
        public TextMeshProUGUI AmountDisplay
        {
            set
            {
                amountDisplay = value;
                if (amountDisplay)
                {
                    amountDisplay.text = curCount.ToString();
                    amountDisplay.gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        /// 장전 함수
        /// </summary>
        /// <param name="bulletInfo">장전할 탄환 뭉치 정보</param>
        /// <returns>장전 후 남은 탄환 뭉치 (없다면 null)</returns>
        public ItemBulletInfo LoadMagazine(ItemBulletInfo bulletInfo)
        {
            if (bulletInfo.bulletType != bulletType) return bulletInfo; // 탄창과 호환되지 않음
            // 삽입 가능 최대 탄환수
            int availableCount = maxCount - CurCount;
            if (availableCount == 0) return bulletInfo;
            ItemBulletInfo bulletToLoad = Instantiate(bulletInfo);
            if (availableCount >= bulletInfo.AmountCount)
            {
                // 전부 장전
                bulletToLoad.AmountCount = bulletInfo.AmountCount;
                CurCount = bulletToLoad.AmountCount;
                bullets.Enqueue(bulletToLoad);
                bulletInfo.AmountCount = 0;
                return null;
            }
            else
            {
                // 일부 장전
                bulletToLoad.AmountCount = availableCount;
                CurCount += bulletToLoad.AmountCount;
                bullets.Enqueue(bulletToLoad);
                bulletInfo.AmountCount -= availableCount;
                return bulletInfo;
            }
        }

        /// <summary>
        /// 다음 탄환 반환 함수
        /// </summary>
        /// <param name="bullet">다음 탄환을 받을 변수</param>
        /// <returns>탄환 있으면 true, 없으면 false</returns>
        public bool GetNextBullet(out ItemBulletInfo bullet)
        {
            if (currentBullet == null ||
                currentBullet.AmountCount == 0)
            {
                // 튜에서 다음 탄환 꺼내기
                if (!bullets.TryDequeue(out currentBullet))
                {
                    // 다음 탄환 없음 = 재장전 필요
                    bullet = null;
                    return false;
                }
            }
            CurCount--;
            currentBullet.AmountCount--;
            bullet = currentBullet;
            return true;
        }
    }
}
