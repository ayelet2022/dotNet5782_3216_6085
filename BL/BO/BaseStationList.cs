using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
            result += $"ID: {Id}.\n";
            result += $"Name: {Name}.\n";
            result += $"Emount of empty charging positions: {EmptyCharges}.\n";
            result += $"Emount of full charging positions: {FullChargingPositions}.\n";
            return result;
        }
    }
}
