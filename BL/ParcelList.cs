using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
   public class ParcelList
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string RecepterName { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Sender {SenderName}, \n";
            result += $"Recepter {RecepterName}, \n";
            result += $"Weight is {Weight}, \n";
            result += $"Priority is {Priority}, \n";
            return result;
        }
    }
}
