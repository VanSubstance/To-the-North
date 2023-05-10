namespace Assets.Scripts.Components.Windows.Pause
{
    public class WindowPauseController : WindowBaseController
    {
        public void SaveGame()
        {
            CommonGameManager.Instance.SaveGame();
        }
        public void ExitGame()
        {
            CommonGameManager.Instance.ExitGame();
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
        }
    }
}
