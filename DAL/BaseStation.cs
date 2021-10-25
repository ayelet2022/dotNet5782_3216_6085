using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal struct BaseStation
    {
        internal int Id { get; set; }
        internal string Name { get; set; }
        internal int EmptyCharges { get; set; }
        internal double Longitude { get; set; }
        internal double Latitude { get; set; }
        public override string ToString()
        {
            String result = " ";
            result += $"ID is {Id}, \n";
            result += $"Name is {Name}, \n";
            result += $"Latitude is {Latitude}, \n";
            result += $"longitude is {Longitude}, \n";
            return result;
        }
    }
}
