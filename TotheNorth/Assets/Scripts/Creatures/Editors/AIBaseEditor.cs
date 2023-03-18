using Assets.Scripts.Creatures.Abstracts;
using Assets.Scripts.Creatures.Objects;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Creatures.Editors
{
    [CustomEditor(typeof(AAIBaseController))]
    internal class AIBaseEditor : Editor
    {
        private void OnSceneGUI()
        {
            AAIBaseController aiBase = (AAIBaseController)target;

            // 목표 이동 트랙 그리기
            Handles.color = new Color(0, 1, 0, 1f);
            Vector3 targetMoveInfo = aiBase.GetCurTargetPoint();
            if (targetMoveInfo != null)
            {
                Handles.DrawLine(aiBase.transform.localPosition, new Vector2(targetMoveInfo.x, targetMoveInfo.y), targetMoveInfo.z);
            }
        }
    }
}
