using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Creatures.Controllers;
using Assets.Scripts.Creatures.Objects;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Creatures.Editors
{
    [CustomEditor(typeof(AIBaseEditor))]
    internal class AIBaseEditor : Editor
    {
        private void OnSceneGUI()
        {
            AIBaseController aiBase = (AIBaseController)target;

            // 목표 이동 트랙 그리기
            Handles.color = new Color(0, 1, 0, 0.5f);
            AIMoveInfo targetMoveInfo = aiBase.curTargetMoveInfo;
            if (targetMoveInfo != null)
            {
                Handles.DrawLine(aiBase.transform.localPosition, targetMoveInfo.point(), targetMoveInfo.spdMove);
            }
        }
    }
}
