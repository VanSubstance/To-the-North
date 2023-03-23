using Assets.Scripts.Creatures.Bases;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Creatures.Editors
{
    internal class AIBaseEditor : Editor
    {
        protected void OnSceneGUI()
        {
            AIBaseController aiBase = (AIBaseController)target;
            Handles.color = Color.red;
            if (aiBase.targetToMove != null)
                Handles.DrawLine(aiBase.transform.position, (Vector3)aiBase.targetToMove);

            Handles.color = Color.yellow;
            if (aiBase.targetToGaze != null)
                Handles.DrawLine(aiBase.transform.position, (Vector3)aiBase.targetToGaze);
        }
    }
}
