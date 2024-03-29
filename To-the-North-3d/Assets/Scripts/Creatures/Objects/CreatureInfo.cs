using Assets.Scripts.Components.Progress;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    [CreateAssetMenu(fileName = "Creature Info", menuName = "Data Objects/Creature Info", order = int.MaxValue)]
    public class CreatureInfo : ScriptableObject
    {
        [SerializeField]
        private string npcPath;

        public string NpcPath
        {
            get
            {
                return $"Npc/{npcPath}";
            }
        }

        [SerializeField]
        private int maxHp;
        [SerializeField]
        private bool isActiveBehaviour, isRunAway;

        public float sightRange = 5f, moveSpd = 3f;
        public ProgressInfo hp = null;

        public int LiveHp
        {
            set
            {
                hp.CurValue += value;
            }
            get
            {
                return (int)hp.CurValue;
            }
        }

        public bool IsActiveBehaviour
        {
            set
            {
                isActiveBehaviour = value;
            }
            get
            {
                return isActiveBehaviour;
            }
        }

        public bool IsRunAway
        {
            get
            {
                return isRunAway;
            }
        }

        public static CreatureInfo GetClone(CreatureInfo _data)
        {
            if (_data == null) return null;
            CreatureInfo res = CreateInstance<CreatureInfo>();
            res.hp = new ProgressInfo(_data.maxHp);
            res.sightRange = _data.sightRange;
            res.moveSpd = _data.moveSpd;
            res.isActiveBehaviour = _data.isActiveBehaviour;
            res.isRunAway = _data.isRunAway;
            return res;
        }
    }
}
