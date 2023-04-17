using Assets.Scripts.Items;

namespace Assets.Scripts.Components.Popups
{
    /// <summary>
    /// 각 아이템 별 필요한 호버링 창을 변경하기 위한 델리게이트
    /// </summary>
    /// <param name="info">아이템 정보</param>
    public delegate void HoveringItemInfoChangeControl(ItemBaseInfo info);
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
