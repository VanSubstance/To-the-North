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
    [CustomEditor(typeof(AIPetrolBaseController))]
    internal class AIPetrolBaseEditor : Editor
    {
        private void OnSceneGUI()
        {
            AIPetrolBaseController aiBase = (AIPetrolBaseController)target;

            // 목표 이동 트랙 그리기
            Handles.color = new Color(1, 1, 1, 0.5f);
            AIMoveInfo[] moveTrack = aiBase.GetMoveTrack();
            for (int i = 0; i < moveTrack.Length - 1; i++)
            {
                Handles.DrawLine(moveTrack[i].point(), moveTrack[i + 1].point(), moveTrack[i].spdMove);
            }
        }
    }
}
