using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus Status { get; set; }
        public CustomerInParcel SenderOrRecepter { get; set; }
    }
}
