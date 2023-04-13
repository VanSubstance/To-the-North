namespace Assets.Scripts.Items
{
    public interface IItemEquipable
    {
        /// <summary>
        /// 장비 변경
        /// </summary>
        /// <param name="_info">새로운 장비 정보</param>
        public void ChangeEquipment(ItemEquipmentInfo _info);

        /// <summary>
        /// 장비를 장착하였는지 판별
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty();
    }
}
