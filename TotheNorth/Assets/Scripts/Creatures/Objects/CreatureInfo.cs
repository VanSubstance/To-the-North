using Assets.Scripts.Components.Progress;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    [CreateAssetMenu(fileName = "Creature Info", menuName = "Data Objects/Creature Info", order = int.MaxValue)]
    internal class CreatureInfo : ScriptableObject
    {
        [SerializeField]
        private int maxHp;

        public ProgressInfo hp;
        public float atkRange = 5f, moveSpd = 3f, moveDis = 3f;

        public static CreatureInfo GetClone(CreatureInfo _data)
        {
            CreatureInfo res = CreateInstance<CreatureInfo>();
            res.hp = new ProgressInfo(_data.maxHp);
            res.atkRange = _data.atkRange;
            res.moveSpd = _data.moveSpd;
            res.moveDis = _data.moveDis;
            return res;
        }
    }
}
