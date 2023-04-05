using UnityEditor;

namespace Assets.Scripts.Battles.Editors
{
    [CustomEditor(typeof(ProjectileController))]
    internal class ProjectileEditor : Editor
    {
        private ProjectileController bc;
        private void OnSceneGUI()
        {
            bc = (ProjectileController)target;
            Handles.color = UnityEngine.Color.red;
            Handles.DrawLine(bc.startPos, bc.targetPos);
        }
    }
}
