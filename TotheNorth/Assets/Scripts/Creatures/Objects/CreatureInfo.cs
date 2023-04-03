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

        public CreatureInfo()
        {
        }
        public CreatureInfo(CreatureInfo _data)
        {
            hp = new ProgressInfo(_data.maxHp);
            atkRange = _data.atkRange;
            moveSpd = _data.moveSpd;
            moveDis = _data.moveDis;
        }
    }
}
