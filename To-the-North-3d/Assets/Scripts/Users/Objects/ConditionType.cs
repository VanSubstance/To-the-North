namespace Assets.Scripts.Users
{
    public enum ConditionType
    {
        None, 

        // 얕은 출혈
        Bleeding_Light,
        // 심각한 출혈
        Bleeding_Heavy,
        // 골절
        Fracture,
        // 통증
        Pain,
        // 어지러움
        Dizziness,
        //감염
        Infection,

        // 허기
        Hunger,
        // 갈즐
        Thirst,
        // 피로
        Exhaust,
        // 더위
        Hot,
        // 추위
        Cold,
    }
}
