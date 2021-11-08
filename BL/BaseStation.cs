using System;

namespace IBL.BO
{
    public class BaseStation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmptyCharges { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Name is {Name}, \n";
            result += $"Latitude is {Latitude}, \n";
            result += $"Longitude is {Longitude}, \n";
            result += $"EmptyCharges is {EmptyCharges}, \n";
            return result;
        }
    }
}
