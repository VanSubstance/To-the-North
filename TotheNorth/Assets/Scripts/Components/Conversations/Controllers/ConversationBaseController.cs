using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Components.Conversations.Managers;
using UnityEngine;

namespace Assets.Scripts.Components.Conversations.Controllers
{
    internal class ConversationBaseController : MonoBehaviour
    {
        private void OnEnable()
        {
            InGameStatus.User.isPause = true;
            GlobalStatus.Util.MouseEvent.Left.setActions(
                actionClick: (tf, mp) =>
                {
                    ConversationManager.FinishConversation();
                });
        }
        private void OnDisable()
        {
            InGameStatus.User.isPause = false;
            GlobalStatus.Util.MouseEvent.Left.setActions(
                actionClick: null);
        }
    }
}
