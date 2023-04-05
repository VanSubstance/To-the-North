using Assets.Scripts.Components.Progress;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    [CreateAssetMenu(fileName = "Creature Info", menuName = "Data Objects/Creature Info", order = int.MaxValue)]
    public class CreatureInfo : ScriptableObject
    {
        [SerializeField]
        private int maxHp;

        public float atkRange = 5f, moveSpd = 3f, moveDis = 3f;
        public ProgressInfo hp = null;

        public int LiveHp
        {
            set
            {
                hp.curValue += value;
            }
            get
            {
                return (int)hp.curValue;
            }
        }

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
