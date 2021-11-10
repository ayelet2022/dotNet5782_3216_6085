﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class BaseStationList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmptyChargingPositions { get; set; }
        public int FullChargingPositions { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Name is {Name}, \n";
            result += $"EmptyChargingPositions is {EmptyChargingPositions}, \n";
            result += $"FullChargingPositions is {FullChargingPositions},\n";
            return result;
        }
    }
}