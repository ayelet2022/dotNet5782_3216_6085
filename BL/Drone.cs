﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace IBL.BO
{
    public class Drone
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
        // public double Battery { get; set; }
        /// <summary>
        /// what status if the dron in(available\infix\delivering)
        /// </summary>
        //public DroneStatuses Status { get; set; }
        /// <summary>
        /// this funciot print all of the drones details
        /// </summary>
        /// <returns></returns>the string that collect all the delaies and then prints it
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Model is {Model}, \n";
            result += $"MaxWeight is {MaxWeight}, \n";
            //result += $"Status is {Status}, \n";
            //result += $"Battery is {Battery}, \n";
            return result;
        }
    }
}
