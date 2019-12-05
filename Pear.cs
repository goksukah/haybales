using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantville_Try3
{
    class Pear:Seed
    {

        public Pear()
        {
            this.Name = "pear";
            this.SeedPrice = 3;
            this.HarvestPrice = 5;
            this.HarvestDuration = new TimeSpan(00, 03, 00);
            this.PlantedTime = DateTime.Now;
        }

    }
}
