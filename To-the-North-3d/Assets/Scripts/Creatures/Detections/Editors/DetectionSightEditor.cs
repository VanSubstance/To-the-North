using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Creatures.Detections
{
    [CustomEditor(typeof(DetectionSightController))]
    internal class DetectionSightEditor : Editor
    {
        private void OnSceneGUI()
        {
            DetectionSightController fow = (DetectionSightController)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, (InGameStatus.User.Detection.Sight.Range));
            Vector3 viewAngleA = fow.DirFromAngle((InGameStatus.User.Movement.curdegree) + ((-InGameStatus.User.Detection.Sight.Degree) / 2), false);
            Vector3 viewAngleB = fow.DirFromAngle((InGameStatus.User.Movement.curdegree) + ((InGameStatus.User.Detection.Sight.Degree) / 2), false);

            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * (InGameStatus.User.Detection.Sight.Range));
            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * (InGameStatus.User.Detection.Sight.Range));

            Handles.color = Color.red;
        }
    }
}
