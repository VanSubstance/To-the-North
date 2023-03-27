using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Items.Objects
{
    internal class ItemWeaponInfo : ItemEquipmentInfo
    {
        public int atk;
        public ItemWeaponInfo()
        {
        }
        public ItemWeaponInfo(string name, int atk)
        {
            this.name = name;
            this.atk = atk;
        }

        public new string ToString()
        {
            return $"{base.ToString()}공격력:: {atk}\n";
        }
    }
}
