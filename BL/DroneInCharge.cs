﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneInCharge
    {
        public int Id { get; set; }
        public int Battery { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"DroneId: {Id}.\n";
            result += $"Battery: {Battery}.\n";
            return result;
        }
    }
}
