using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Users.Controllers;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Users.Editors
{
    [CustomEditor(typeof(DetectionPassiveController))]
    internal class DetectionPassiveEditor : Editor
    {
        private void OnSceneGUI()
        {
            DetectionPassiveController fow = (DetectionPassiveController)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fow.transform.position, Vector3.back, Vector3.up, 360, fow.isAI ? fow.range : InGameStatus.User.Detection.Instinct.range);

            Handles.color = Color.red;
        }
    }
}
