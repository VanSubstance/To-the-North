using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Components.Progress;

namespace Assets.Scripts.Components.Infos
{
    public class UINumericController : MonoBehaviour
    {
        [SerializeField]
        public BarBaseController barForHp, barForStamina;
    }
}
