using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    public class MoveController : IEffectControl
    {
        private Vector3 DirectionToVector(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Vector3.up;
                case Direction.Down:
                    return Vector3.down;
                case Direction.Left:
                    return Vector3.left;
                case Direction.Right:
                    return Vector3.right;
            }
            return Vector3.zero;
        }
        public IEnumerator CoroutineEffect(Transform target, EffectInfo _info)
        {
            if (_info is MoveInfo info)
            {
                Vector3 dirVector = DirectionToVector(info.direction);
                while (info.timeLeft >= 0f)
                {
                    info.timeLeft -= Time.deltaTime;
                    yield return new WaitForSeconds(0.01f);
                    if (target == null) break;
                    target.Translate(dirVector * info.distance * Time.deltaTime * GlobalSetting.accelSpeed);
                }
                info.actionAfter?.Invoke();
            }
            throw new NotImplementedException();
        }
    }
}
