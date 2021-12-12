using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelInCustomer
    {
        public int Id { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus Status { get; set; }
        public CustomerInParcel SenderOrRecepter { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID: {Id}.\n";
            result += $"Weight: {Weight}.\n";
            result += $"Priority: {Priority}.\n";
            result += $"Status: {Status}.\n";
            if(Status==ParcelStatus.delivery)
                result += $"Resepter: \n{SenderOrRecepter}\n";
            else
                result += $"Sender: \n{SenderOrRecepter}\n";
            return result;
        }
    }
}
