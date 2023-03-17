using UnityEngine;

namespace Assets.Scripts.Creatures.Objects
{
    [System.Serializable]
    internal class AIMoveInfo
    {
        public float x, y, spdMove;

        public Vector2 point()
        {
            return new Vector2(x, y);
        }

    }
}
