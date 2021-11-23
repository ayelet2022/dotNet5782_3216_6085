using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class CustomerList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int ParcelsSentAndDel { get; set; }
        public int ParcelsSentAndNotDel { get; set; }
        public int ParcelsResepted { get; set; }
        public int ParcelsOnTheWay { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID:{Id}.\n";
            result += $"Name:{Name}.\n";
            result += $"Phone:{Phone}.\n";
            result += $"Parcels that were Sent And Deleverd:{ParcelsSentAndDel}.\n";
            result += $"Parcels that were Sent And wasnt deleverd:{ParcelsSentAndNotDel}.\n";
            result += $"Parcels that were deleverd:{ParcelsResepted}.\n";
            result += $"Parcels that are on the way:{ParcelsOnTheWay}.\n";
            return result;
        }
    }
}
