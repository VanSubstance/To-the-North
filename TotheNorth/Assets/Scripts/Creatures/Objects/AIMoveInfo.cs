using UnityEngine;

namespace Assets.Scripts.Creatures.Objects
{
    [System.Serializable]
    internal class AIMoveInfo
    {
        public float x, y, spdMove;

        public AIMoveInfo()
        {
        }

        public AIMoveInfo(float x, float y, float spdMove)
        {
            this.x = x;
            this.y = y;
            this.spdMove = spdMove;
        }

        public Vector2 point()
        {
            return new Vector2(x, y);
        }

        public override string ToString()
        {
            return $"{x}, {y} <- {spdMove}";
        }

    }
}
