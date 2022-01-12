﻿using System;
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
            result += $"Latitude: {Latitude:0.###}.\n";
            result += $"longitude: {Longitude:0.###}.\n";
            return result;
        }
    }
}
