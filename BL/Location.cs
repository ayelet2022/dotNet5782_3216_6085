using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"Latitude: {Latitude}.\n";
            result += $"longitude: {Longitude}.\n";
            return result;
        }
    }
}
