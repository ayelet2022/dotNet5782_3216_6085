using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DAL
{
    public struct Parcel
    {
        public int id { get; set; }
        public int senderId { get; set; }
        public int targetId { get; set; }
        public int droneId { get; set; }
        public WeightCategories weight { get; set; }
        public Priorities priority { get; set; }
        public DateTime creatParcel { get; set; }
        public DateTime paired { get; set; }
        public DateTime pickedUp { get; set; }
        public DateTime arrival { get; set; }
    }
}
