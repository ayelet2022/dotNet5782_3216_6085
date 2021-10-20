using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public struct Customer
    {
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public double longitud { get; set; }
        public double latitude { get; set; }
    }
}
