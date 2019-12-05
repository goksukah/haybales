using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantville_Try3
{
    class Strawberry : Seed
    {
        public Strawberry()
        {   this.Name = "strawberry";
            this.SeedPrice = 2;
            this.HarvestPrice = 3;
            this.HarvestDuration = new TimeSpan(00, 00, 30);
            this.PlantedTime = DateTime.Now;
        }
    }
}
