using System.Collections;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Abstracts;
using Assets.Scripts.Creatures.Objects;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    internal class AINpcController : AAIBaseController
    {
        public override void OnDetectUser()
        {
            Debug.Log("NPC >> 유저 식별");
        }

        public override void OnDetectSuspicious()
        {
            Debug.Log("NPC >> 의심 식별");
        }
    }
}
