using Assets.Scripts.Creatures.Controllers.Creatures;
using UnityEditor;

namespace Assets.Scripts.Creatures.Editors
{
    [CustomEditor(typeof(MonsterBaseController))]
    internal class MonsterBaseEditor : AIBaseEditor
    {
        private new void OnSceneGUI()
        {
            base.OnSceneGUI();
        }
    }
}
