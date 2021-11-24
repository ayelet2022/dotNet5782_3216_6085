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
            result += $"ID: {Id}.\n";
            result += $"Name: {Name}.\n";
            result += $"Location: {BaseStationLocation}\n";
            result += $"EmptyCharges: {EmptyCharges}.\n";
            result += $"Drones in charge: {DronesInCharge}\n";
            return result;
        }
    }
}
