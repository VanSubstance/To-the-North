using Assets.Scripts.Commons.Constants;
using UnityEngine;

namespace Assets.Scripts.Users
{
    internal class HandController : MonoBehaviour
    {
        [SerializeField]
        Transform handL, handR;

        private void Update()
        {
            float curDegree = InGameStatus.User.Movement.curdegree;
            // 시야를 따라 회전
            handL.localRotation = Quaternion.Euler(0, 0, curDegree);
            handR.localRotation = Quaternion.Euler(0, 0, curDegree);

            if (
                -90 < curDegree &&
                curDegree <= 90
                )
            {
                // 오른쪽
                if (handL.childCount > 0)
                {
                    handL.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
                }
                if (handR.childCount > 0)
                {
                    handR.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else
            {
                // 왼쪽
                if (handL.childCount > 0)
                {
                    handL.GetChild(0).localRotation = Quaternion.Euler(180, 0, 0);
                }
                if (handR.childCount > 0)
                {
                    handR.GetChild(0).localRotation = Quaternion.Euler(180, 0, 0);
                }
            }
        }
    }
}
