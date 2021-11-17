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
        public IEnumerable<ParcelInCustomer> ParcelsFromCustomers{ get; set; }
        public IEnumerable<ParcelInCustomer> ParcelsToCustomers{ get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Name is {Name}, \n";
            result += $"Phone is {Phone}, \n";
            result += $"Latitude is {CustomerLocation.Latitude}, \n";
            result += $"longitude is {CustomerLocation.Longitude}, \n";
            return result;
        }
    }
}
