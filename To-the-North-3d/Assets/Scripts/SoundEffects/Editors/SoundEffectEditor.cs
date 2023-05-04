using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.SoundEffects
{
    [CustomEditor(typeof(SoundEffectController))]
    internal class SoundEffectEditor : Editor
    {
        private SoundEffectController ctrl;
        private void OnSceneGUI()
        {
            ctrl = (SoundEffectController)target;
            Handles.DrawWireArc(ctrl.transform.position, Vector3.up, Vector3.right, 360, ctrl.impactDistance);
        }
    }
}
