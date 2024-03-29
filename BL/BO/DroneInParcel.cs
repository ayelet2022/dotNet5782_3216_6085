﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
            result += $"ID:{Id}.\n";
            result += $"Battery:{Battery}.\n";
            result += $"Location: \n{DroneLocation}.\n";
            return result;
        }
    }
}
