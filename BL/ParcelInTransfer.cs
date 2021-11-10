using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class ParcelInTransfer
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
            result += $"ID is {Id}, \n";
            result += $"StatusParcel is {StatusParcel}, \n";
            result += $"priority is {Priority}, \n";
            result += $"Weight is {Weight}, \n";
            result += $"CustomerInParcel is {Sender}, \n";
            result += $"CustomerInParcel is {Recepter}, \n";
            result += $"Location is {PickUpLocation}, \n";
            result += $"Location is {DelieveredLocation}, \n";
            result += $"TransportDistance is {TransportDistance}, \n";
            return result;
        }
    }
}
