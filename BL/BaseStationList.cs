using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class BaseStationList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmptyCharges { get; set; }
        public int FullChargingPositions { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Name is {Name}, \n";
            result += $"EmptyChargingPositions is {EmptyCharges}, \n";
            result += $"FullChargingPositions is {FullChargingPositions},\n";
            return result;
        }
    }
}
