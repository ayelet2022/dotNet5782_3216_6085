﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DO
{
    public struct BaseStation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int EmptyCharges { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool IsActive { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Name is {Name}, \n";
            result += $"Location is {Util.SexagesimalCoordinate(Longitude, Latitude)}, \n";
            result += $"EmptyCharges is {EmptyCharges}, \n";
            return result;
        }
    }
}
