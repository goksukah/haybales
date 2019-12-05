using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantville_Try3
{
    class Spinach:Seed
    {

        public Spinach()
        {
            this.Name = "spinach";
            this.SeedPrice = 5;
            this.HarvestPrice = 10;
            this.HarvestDuration = new TimeSpan(00, 01, 00);
            this.PlantedTime = DateTime.Now;
        }
    }
}
