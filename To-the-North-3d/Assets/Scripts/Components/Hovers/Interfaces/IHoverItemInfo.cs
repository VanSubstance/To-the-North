using Assets.Scripts.Items;

namespace Assets.Scripts.Components.Hovers
{
    public interface IHoverItemInfo
    {
        /// <summary>
        /// 호버링 아이템 정보 변경 함수
        /// null 전달 시: 초기화
        /// </summary>
        /// <param name="_info"></param>
        public void OnItemInfoChanged(ItemBaseInfo _info);
    }
}
