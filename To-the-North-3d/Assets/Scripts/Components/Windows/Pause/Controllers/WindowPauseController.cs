namespace Assets.Scripts.Components.Windows.Pause
{
    internal class WindowPauseController : WindowBaseController
    {
        public void SaveGame()
        {
            CommonGameManager.Instance.SaveGame();
        }
        public void ExitGame()
        {
            CommonGameManager.Instance.ExitGame();
        }
    }
}
