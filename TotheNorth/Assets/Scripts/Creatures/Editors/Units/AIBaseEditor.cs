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
            Handles.color = Color.red;
            if (aiBase.targetToMove != null)
                Handles.DrawLine(aiBase.transform.position, (Vector3)aiBase.targetToMove, 2.5f);

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
            if ((detectPos = aiBase.targetPos) != null)
            {
                Handles.color = Color.red;
                Handles.DrawLine((Vector3)detectPos + new Vector3(-.5f, -.5f, 0f), (Vector3)detectPos + new Vector3(.5f, .5f, 0f), 2.5f);
                Handles.DrawLine((Vector3)detectPos + new Vector3(-.5f, .5f, 0f), (Vector3)detectPos + new Vector3(.5f, -.5f, 0f), 2.5f);
            }
        }
    }
}
