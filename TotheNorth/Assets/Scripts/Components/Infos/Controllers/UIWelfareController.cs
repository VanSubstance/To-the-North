using Assets.Scripts.Components.Progress;
using UnityEngine;

namespace Assets.Scripts.Components.Infos
{
    public class UIWelfareController : MonoBehaviour
    {
        [SerializeField]
        public BarBaseController barForHunger, barForThirst, barForTemperature;
    }
}
