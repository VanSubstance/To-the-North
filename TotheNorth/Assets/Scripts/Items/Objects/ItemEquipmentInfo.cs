using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Items.Objects
{
    internal class ItemEquipmentInfo
    {
        public string name = "장비 이름";

        public ItemEquipmentInfo()
        {
        }

        public ItemEquipmentInfo(string name)
        {
            this.name = name;
        }

        public new string ToString()
        {
            return $"장비 이름:: {name}\n";
        }
    }
}
