using Assets.Scripts.Commons.Constants;
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

        private void Update()
        {
            float curDegree = aiBase ? aiBase.CurDegree : InGameStatus.User.Movement.curdegree;
            // 시야를 따라 회전
            handL.localRotation = Quaternion.Euler(0, 0, curDegree);
            handR.localRotation = Quaternion.Euler(0, 0, curDegree);
            if (
                -90 < curDegree &&
                curDegree <= 90
                )
            {
                // 오른쪽
                body.localScale = Vector3.one;
            }
            else
            {
                // 왼쪽
                body.localScale = new Vector3(-1, 1, 1);
            }

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
