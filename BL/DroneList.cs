using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneList
    {
        /// <summary>
        /// the id of the drone
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the model of the dron
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// the max wight that the drone can carry
        /// </summary>
        public WeightCategories MaxWeight { get; set; }
        /// <summary>
        /// the pracent of battery
        /// </summary>
        public int Battery { get; set; }
        /// <summary>
        /// what status if the dron in(available\infix\delivering)
        /// </summary>
        public DroneStatus Status { get; set; }
        /// <summary>
        /// this funciot print all of the drones details
        /// </summary>
        /// <returns></returns>the string that collect all the delaies and then prints it
        public Location DroneLocation { get; set; }
        public int NumOfParcelOnTheWay { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID: {Id}.\n";
            result += $"Model: {Model}.\n";
            result += $"MaxWeight: {MaxWeight}.\n";
            result += $"Battery: { Battery}.\n";
            result += $"Status: {Status}.\n";
            result += $"Location: \n{DroneLocation}";
            result += $"Id of the parcel on the way: {NumOfParcelOnTheWay}.\n";
            return result;
        }
    }
}
