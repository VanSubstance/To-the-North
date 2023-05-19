using UnityEngine;
using TMPro;

namespace Assets.Scripts.Components.Windows.Pause
{
    public class WindowPauseController : WindowCenterBaseController
    {
        [SerializeField]
        private Transform mainOption, optionOption;
        [SerializeField]
        private TextMeshProUGUI
            backToGame, goToOption, saveGame, goToDesktop, back, language;
        private new void Awake()
        {
            base.Awake();
            GlobalComponent.Common.Text.Option.back = back;
            GlobalComponent.Common.Text.Option.goToDesktop = goToDesktop;
            GlobalComponent.Common.Text.Option.saveGame = saveGame;
            GlobalComponent.Common.Text.Option.goToOption = goToOption;
            GlobalComponent.Common.Text.Option.backToGame = backToGame;
            GlobalComponent.Common.Text.Option.language = language;
            CloseOption();
        }
        public void SaveGame()
        {
            CommonGameManager.Instance.SaveGame();
        }
        public void ExitGame()
        {
            CommonGameManager.Instance.ExitGame();
        }

        public void OpenOption()
        {
            mainOption.gameObject.SetActive(false);
            optionOption.gameObject.SetActive(true);
        }

        public void CloseOption()
        {
            mainOption.gameObject.SetActive(true);
            optionOption.gameObject.SetActive(false);
        }

        public void ChangeLanguage(int lang)
        {
            switch (lang)
            {
                case 0:
                    GlobalSetting.Language = "Kor";
                    return;
                case 1:
                    GlobalSetting.Language = "Eng";
                    return;
            }
        }

        public override void OnOpen()
        {
        }

        public override void OnClose()
        {
            CloseOption();
        }
    }
}
