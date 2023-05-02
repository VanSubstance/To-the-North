using Assets.Scripts.Creatures.Bases;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    internal class BodyRotateController : MonoBehaviour
    {
        [SerializeField]
        Transform body, handL, handR;

        [SerializeField]
        private AIBaseController aiBase;

        private void Awake()
        {
        }

        private void Update()
        {
            float curDegree = aiBase ? aiBase.CurDegree : InGameStatus.User.Movement.curdegree;
            if (
                270 < curDegree ||
                curDegree <= 90
                )
            {
                // 오른쪽
                body.localScale = Vector3.one;
                if (handL == null) return;
                handL.localRotation = Quaternion.Euler(0, 0, curDegree);
                handR.localRotation = Quaternion.Euler(0, 0, curDegree);
            }
            else
            {
                // 왼쪽
                body.localScale = new Vector3(-1, 1, 1);
                if (handL == null) return;
                handL.localRotation = Quaternion.Euler(0, 0, 180 - curDegree);
                handR.localRotation = Quaternion.Euler(0, 0, 180 - curDegree);
            }
        }
    }
}
