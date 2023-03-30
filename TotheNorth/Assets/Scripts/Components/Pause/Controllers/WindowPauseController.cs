using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Components.Pause.Controllers
{
    internal class WindowPauseController : AWindowBaseContentController
    {
        public void SaveGame()
        {
            CommonGameManager.Instance.SaveGame();
        }
        public void ExitGame()
        {
            CommonGameManager.Instance.ExitGame();
        }

        public override void ClearContent()
        {
        }

        protected override void InitComposition()
        {
        }
    }
}
