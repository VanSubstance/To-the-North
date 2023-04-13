using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class ItemArmorController : MonoBehaviour
    {
        [SerializeField]
        private ItemArmorInfo info;
        public ItemArmorInfo Info
        {
            get
            {
                return info;
            }
        }
    }
}
