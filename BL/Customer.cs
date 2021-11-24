using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location CustomerLocation { get; set; }
        public List<ParcelInCustomer> ParcelsFromCustomers{ get; set; }
        public List<ParcelInCustomer> ParcelsToCustomers{ get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID: {Id}.\n";
            result += $"Name: {Name}.\n";
            result += $"Phone: {Phone}.\n";
            result += $"Location: \n{CustomerLocation}\n";
            return result;
        }
    }
}
