using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// 실제 데미지 계산을 할 때 쓰이는 데이터 오브젝트
    /// </summary>
    public class AttackInfo
    {
        public float powerPenetration;
        public float powerImpact;
        public float amountPenetration;
        public float amountImpact;
        public float powerKnockback;
    }
}
