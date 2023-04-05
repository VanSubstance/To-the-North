using Assets.Scripts.Components.Progress;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    [CreateAssetMenu(fileName = "Creature Info", menuName = "Data Objects/Creature Info", order = int.MaxValue)]
    public class CreatureInfo : ScriptableObject
    {
        [SerializeField]
        private int maxHp;
        [SerializeField]
        private bool isAttackFirst;

        public float sightRange = 5f, moveSpd = 3f, moveDis = 3f;
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

        public bool IsAttackFirst
        {
            get
            {
                return isAttackFirst;
            }
        }

        public static CreatureInfo GetClone(CreatureInfo _data)
        {
            CreatureInfo res = CreateInstance<CreatureInfo>();
            res.hp = new ProgressInfo(_data.maxHp);
            res.sightRange = _data.sightRange;
            res.moveSpd = _data.moveSpd;
            res.moveDis = _data.moveDis;
            res.isAttackFirst = _data.isAttackFirst;
            return res;
        }
    }
}
