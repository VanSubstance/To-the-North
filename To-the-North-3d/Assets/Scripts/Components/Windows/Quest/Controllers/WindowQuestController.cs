using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Commons;

namespace Assets.Scripts.Components.Windows
{
    public class WindowQuestController : WindowFlexBaseController
    {
        private new void Awake()
        {
            base.Awake();
            MouseInteractionController.AttachMouseInteractor<MouseInteractionMoveableController>(transform);
        }
        public override void OnClose()
        {
        }

        public override void OnOpen()
        {
        }
    }
}
