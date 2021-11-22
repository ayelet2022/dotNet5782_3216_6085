using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmptyCharges { get; set; }
        public Location BaseStationLocation { get; set; }
        public List<DroneInCharge> DronesInCharge { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Name is {Name}, \n";
            result += $"Location is {BaseStationLocation}, \n";
            result += $"EmptyCharges is {EmptyCharges}, \n";
            result += $"Drones in charge are {DronesInCharge},\n";
            return result;
        }
    }
}
