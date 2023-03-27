using Assets.Scripts.Creatures.Controllers.Creatures;
using UnityEditor;

namespace Assets.Scripts.Creatures.Editors
{
    [CustomEditor(typeof(NpcBaseController))]
    internal class NpcBaseEditor : AIBaseEditor
    {
        private new void OnSceneGUI()
        {
            base.OnSceneGUI();
        }
    }
}
