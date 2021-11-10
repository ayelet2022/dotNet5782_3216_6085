using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    class ParcelInCustomer
    {
        /// <summary>
        /// the parcel id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the parcel whight
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// the parcel priority
        /// </summary>
        public Priorities Priority { get; set; }
        public ParcelStatus Status { get; set; }
        public CustomerInParcel SenderOrRecepter { get; set; }
    }
}
