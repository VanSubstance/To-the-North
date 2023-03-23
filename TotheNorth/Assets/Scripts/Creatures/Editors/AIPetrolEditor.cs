using Assets.Scripts.Creatures.Controllers;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Creatures.Editors
{
    [CustomEditor(typeof(AIPetrolController))]
    internal class AIPetrolEditor : Editor
    {
        AIPetrolController conductionBase;
        private void OnSceneGUI()
        {
            conductionBase = (AIPetrolController)target;

            Transform[] tracks = conductionBase.GetPetrolTracks();
            if (tracks.Length < 2) return;
            for (int i = 0; i < tracks.Length - 1; i++)
            {
                Handles.color = Color.white;
                Handles.DrawLine(tracks[i].position, tracks[i + 1].position);
            }
        }
    }
}
