using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantville_Try3
{
    class Farmer
    {
        public int Money { get; set; }
        public int Land { get; set; }
        public string Name { get; set; }

        public Farmer(string Name)
        {
            Money = 100;
            Land = 15;
            this.Name = Name;
        }
    }
}
