using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Users.Controllers;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Creatures.Detections
{
    [CustomEditor(typeof(DetectionPassiveController))]
    internal class DetectionPassiveEditor : Editor
    {
        private void OnSceneGUI()
        {
            DetectionPassiveController fow = (DetectionPassiveController)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.isAI ? fow.range : InGameStatus.User.Detection.Instinct.range);

            Handles.color = Color.red;
        }
    }
}
