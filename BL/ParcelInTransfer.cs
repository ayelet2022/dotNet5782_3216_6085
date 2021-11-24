﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInTransfer
    {
        /// <summary>
        /// the parcel id
        /// </summary>
        public int Id { get; set; }
        public bool StatusParcel { get; set; }
        public Priorities Priority { get; set; }
        public WeightCategories Weight { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Recepter { get; set; }
        public Location PickUpLocation { get; set; }
        public Location DelieveredLocation { get; set; }
        public double TransportDistance { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID: {Id}.\n";
            result += $"StatusParcel: {StatusParcel}.\n";
            result += $"priority: {Priority}.\n";
            result += $"Weight: {Weight}.\n";
            result += $"CustomerInParcel: \n{Sender}\n";
            result += $"CustomerInParcel: \n{Recepter}\n";
            result += $"Location: \n{PickUpLocation}\n";
            result += $"Location: \n{DelieveredLocation}\n";
            result += $"TransportDistance: {TransportDistance}.\n";
            return result;
        }
    }
}
