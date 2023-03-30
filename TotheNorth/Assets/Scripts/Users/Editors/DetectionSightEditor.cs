using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Users.Controllers;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Users.Editors
{
    [CustomEditor(typeof(DetectionSightController))]
    internal class DetectionSightEditor : Editor
    {
        private void OnSceneGUI()
        {
            DetectionSightController fow = (DetectionSightController)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fow.transform.position, Vector3.back, Vector3.up, 360, (fow.isAI ? fow.range : InGameStatus.User.Detection.Sight.range));
            Vector3 viewAngleA = fow.DirFromAngle((fow.isAI ? fow.curDegree : InGameStatus.User.Movement.curdegree) + ((fow.isAI ? -fow.degree : -InGameStatus.User.Detection.Sight.degree) / 2), false);
            Vector3 viewAngleB = fow.DirFromAngle((fow.isAI ? fow.curDegree : InGameStatus.User.Movement.curdegree) + ((fow.isAI ? fow.degree : InGameStatus.User.Detection.Sight.degree) / 2), false);

            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * (fow.isAI ? fow.range : InGameStatus.User.Detection.Sight.range));
            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * (fow.isAI ? fow.range : InGameStatus.User.Detection.Sight.range));

            Handles.color = Color.red;
        }
    }
}
