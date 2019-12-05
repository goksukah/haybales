using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantville_Try3
{
    public class Seed
    {

        public string Name { get; set; }
        public int SeedPrice { get; set; }
        public int HarvestPrice { get; set; }
        public TimeSpan HarvestDuration { get; set; }
        public DateTime PlantedTime { get; set; }

        public Seed()

        {
            //string Name, int SeedPrice, int HarvestPrice, TimeSpan HarvestDuration
            this.Name = Name;
            this.SeedPrice = SeedPrice;
            this.HarvestPrice = HarvestPrice;
            this.HarvestDuration = HarvestDuration;
            this.PlantedTime = DateTime.Now;

        }

        public override string ToString()
        {
         
            return Name;
            //            return Name + ", $ " + SeedPrice.ToString();

        }



    }
}
