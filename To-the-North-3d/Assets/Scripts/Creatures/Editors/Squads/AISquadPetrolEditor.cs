using System;
using System.Collections.Generic;
using Assets.Scripts.Creatures.Bases;
using Assets.Scripts.Creatures.Controllers.Conductions;
using Assets.Scripts.Maps.Controllers;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Creatures.Editors
{
    [CustomEditor(typeof(AISquadPetrolController))]
    internal class AISquadPetrolEditor : Editor
    {
        AISquadPetrolController aiBase;
        AISquadBaseController squadBase;
        private void OnSceneGUI()
        {
            float sightAngle = 45, sightRange = 3;
            aiBase = (AISquadPetrolController)target;
            squadBase = aiBase.GetAIBase();
            Transform[] tracks = aiBase.GetPetrolTracks();
            if (tracks.Length < 2) return;
            List<TrackBaseController> bumpsForInitRotation = new List<TrackBaseController>();
            Transform prevPoint = null;
            for (int i = 0; i < tracks.Length; i++)
            {
                TrackBaseController trBase = tracks[i].GetComponent<TrackBaseController>();
                if (prevPoint == null)
                {
                    if (trBase.isToMove)
                    {
                        prevPoint = trBase.transform;
                        foreach (TrackBaseController t in bumpsForInitRotation)
                        {
                            Handles.color = Color.green;
                            Handles.DrawWireArc(prevPoint.position, Vector3.forward, t.transform.position - prevPoint.position, sightAngle / 2, sightRange, t.timeStay);
                            Handles.DrawWireArc(prevPoint.position, Vector3.forward, t.transform.position - prevPoint.position, sightAngle / -2, sightRange, t.timeStay);
                        }
                        Handles.color = Color.white;
                    }
                    else
                    {
                        bumpsForInitRotation.Add(trBase);
                    }
                }
                else
                {
                    if (trBase.isToGaze)
                    {
                        Handles.color = Color.green;
                        Handles.DrawWireArc(prevPoint.position, Vector3.forward, trBase.transform.position - prevPoint.position, sightAngle / 2, sightRange, trBase.timeStay);
                        Handles.DrawWireArc(prevPoint.position, Vector3.forward, trBase.transform.position - prevPoint.position, sightAngle / -2, sightRange, trBase.timeStay);
                    }
                    if (trBase.isToMove)
                    {
                        Handles.color = Color.white;
                        Handles.DrawLine(prevPoint.position, tracks[i].position);
                        prevPoint = tracks[i];
                    }
                }
            }
            try
            {
                foreach (AIBaseController unitBase in squadBase.GetUnitsAll())
                {
                    Vector3? detectPos;
                    if ((detectPos = unitBase.targetPos) != null)
                    {
                        Handles.color = Color.red;
                        Handles.DrawLine((Vector3)detectPos + new Vector3(-.5f, -.5f, 0f), (Vector3)detectPos + new Vector3(.5f, .5f, 0f), 2.5f);
                        Handles.DrawLine((Vector3)detectPos + new Vector3(-.5f, .5f, 0f), (Vector3)detectPos + new Vector3(.5f, -.5f, 0f), 2.5f);
                    }
                }
            }
            catch (NullReferenceException)
            {

            }
        }
    }
}