using System.Collections;
using Assets.Scripts.Commons.Functions;
using Assets.Scripts.Creatures.Abstracts;
using Assets.Scripts.Creatures.Objects;
using UnityEngine;

namespace Assets.Scripts.Creatures.Controllers
{
    internal class AIMonsterController : AAIBaseController
    {
        public override void OnDetectUser()
        {
            Debug.Log("몬스터 >> 유저 식별");
        }

        public override void OnDetectSuspicious()
        {
            Debug.Log("몬스터 >> 의심 식별");
        }
    }
}
