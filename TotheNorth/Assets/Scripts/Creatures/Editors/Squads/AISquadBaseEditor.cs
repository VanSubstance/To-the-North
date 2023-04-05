using System.Collections.Generic;
using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Creatures.Controllers.Creatures;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Creatures.Editors.Squads
{
    [CustomEditor(typeof(AISquadBaseController))]
    internal class AISquadBaseEditor : Editor
    {
        AISquadBaseController aiBase;
        private void OnSceneGUI()
        {
            aiBase = (AISquadBaseController)target;
            Handles.color = Color.green;
            Handles.DrawSolidArc(aiBase.transform.position, Vector3.forward, Vector3.right, 360, 0.3f);
            MarkUnits();
        }

        private void MarkUnits()
        {
            List<AIBaseController> tanks, bruisers, rangers;
            tanks = aiBase.GetUnitsTank();
            bruisers = aiBase.GetUnitsBruiser();
            rangers = aiBase.GetUnitsRanger();

            if (tanks != null)
            {
                Handles.color = Color.blue;
                for (int i = 0; i < tanks.Count; i++)
                {
                    if (tanks[i])
                    {
                        Handles.DrawSolidArc(tanks[i].transform.position, Vector3.forward, Vector3.right, 360, 0.15f);
                    }
                }
            }

            if (bruisers != null)
            {
                Handles.color = Color.yellow;
                for (int i = 0; i < bruisers.Count; i++)
                {
                    if (bruisers[i])
                    {
                        Handles.DrawSolidArc(bruisers[i].transform.position, Vector3.forward, Vector3.right, 360, 0.15f);
                    }
                }
            }

            if (rangers != null)
            {
                Handles.color = Color.red;
                for (int i = 0; i < rangers.Count; i++)
                {
                    if (rangers[i])
                    {
                        Handles.DrawSolidArc(rangers[i].transform.position, Vector3.forward, Vector3.right, 360, 0.15f);
                    }
                }
            }
        }
    }
}
