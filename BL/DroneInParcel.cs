using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneInParcel
    {
        /// <summary>
        /// the id of the drone
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the pracent of battery
        /// </summary>
        public int Battery { get; set; }
        public Location DroneLocation { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is: {Id}.\n";
            result += $"Battery is: {Battery}.\n";
            result += $"Location is: \n{DroneLocation}.\n";
            return result;
        }
    }
}
