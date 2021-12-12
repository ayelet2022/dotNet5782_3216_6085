using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
            result += $"ID: {Id}.\n";
            result += $"Name: {Name}.\n";
            result += $"Location:\n{BaseStationLocation}";
            result += $"EmptyCharges: {EmptyCharges}.\n";
            foreach (var item in DronesInCharge)
            {
                result += $"Drone in charge:\n{item}";
            }
            return result;
        }
    }
}
