﻿ using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerInParcel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID: {Id}.\n";
            result += $"Name: {Name}.\n";
            return result;
        }
    }
}
