using Assets.Scripts.Items;
using Assets.Scripts.Users;

namespace Assets.Scripts.Components.Hovers
{
    /// <summary>
    /// 각 아이템 별 필요한 호버링 창을 변경하기 위한 델리게이트
    /// </summary>
    /// <param name="info">아이템 정보</param>
    public delegate void HoveringItemInfoChangeControl(ItemBaseInfo info);
}
