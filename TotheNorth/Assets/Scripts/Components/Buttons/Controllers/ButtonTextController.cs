namespace Assets.Scripts.Components.Buttons.Controllers
{
    internal class ButtonTextController : ButtonBaseController
    {
        [UnityEngine.SerializeField]
        private TMPro.TextMeshProUGUI ugui;

        public void SetText(string text)
        {
            ugui.text = text;
        }

        public override void Clear()
        {
            base.Clear();
            ugui.text = string.Empty;
        }
    }
}
