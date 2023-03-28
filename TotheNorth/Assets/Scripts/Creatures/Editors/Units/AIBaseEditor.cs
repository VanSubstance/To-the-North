using Assets.Scripts.Creatures.Bases;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Creatures.Editors
{
    internal class AIBaseEditor : Editor
    {
        AIBaseController aiBase;
        protected void OnSceneGUI()
        {
            aiBase = (AIBaseController)target;
            Handles.color = Color.cyan;
            if (aiBase.vectorToMove != null)
                Handles.DrawLine(aiBase.transform.position, (Vector3)aiBase.vectorToMove + aiBase.transform.position, 1f);

            if (aiBase.targetPos != null)
            {
                Handles.color = Color.yellow;
                Handles.DrawLine(aiBase.transform.position, (Vector3)aiBase.targetPos, 2.5f);
            }
            //if (aiBase.targetToGaze != null)
            //    Handles.DrawLine(aiBase.transform.position, (Vector3)aiBase.targetToGaze);
            MarkIndicationPosition();
        }

        private void MarkIndicationPosition()
        {
            Vector3? detectPos;
            if ((detectPos = aiBase.targetToMove) != null)
            {
                Handles.color = Color.red;
                Handles.DrawLine((Vector3)detectPos + new Vector3(-.25f, -.25f, 0f), (Vector3)detectPos + new Vector3(.25f, .25f, 0f), 1f);
                Handles.DrawLine((Vector3)detectPos + new Vector3(-.25f, .25f, 0f), (Vector3)detectPos + new Vector3(.25f, -.25f, 0f), 1f);
            }
        }
    }
}
