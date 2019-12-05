using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Plantville_Try3
{
    public class Plant
    {
        public Seed Seed { get; set; }
        public DateTime PlantedTime { get; set; }
        public TimeSpan HarvestDuration { get; set; }

        public Plant(Seed seed) {

            Seed = seed;
            PlantedTime = DateTime.Now;
            HarvestDuration = seed.HarvestDuration;
        }
        public string TimeLeft(DateTime PlantedTime, TimeSpan HarvestDuration)
        {
            if (!IsSpoiled() && IsRipe())
            {
                return Seed.Name + " (Harvest) ";

            }

            else if (!IsSpoiled()) //PlantedTime, HarvestDuration)
            {
                TimeSpan time = DateTime.Now - PlantedTime;
                return Seed.Name + ", " + (HarvestDuration.Subtract(time)).ToString(@"hh\:mm\:ss") + " left ";
                //                return Name + ", " + (HarvestDuration.Subtract(TimeSpan.FromSeconds(1))) + " minutes left ";

            }

            else
            {
                return Seed.Name + " (spoiled) ";
            }
                
        }





        public Boolean IsSpoiled() //DateTime PlantedTime, TimeSpan HarvestDuration
        {
            Boolean isSpoiled = false;

            if ((DateTime.Now - PlantedTime) > (HarvestDuration.Add(HarvestDuration))) //will spoil if 2*Harvestduration value is exceeded
            {
                isSpoiled = true;
            }

            return isSpoiled;
        }



        public Boolean IsRipe() //DateTime PlantedTime, TimeSpan HarvestDuration
        {
            Boolean isRipe = false;

            if (((DateTime.Now - PlantedTime) >= HarvestDuration))
            {
                isRipe = true;
            }

            return isRipe;
        }

          public override string ToString()
        {
            return Seed.Name + ", $ " + Seed.SeedPrice.ToString();
        }
    }
    }

