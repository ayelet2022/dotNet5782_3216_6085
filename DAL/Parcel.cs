using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.IDAL.DO;
namespace DAL
{
    public struct Parcel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public int DroneId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public DateTime CreatParcel { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }
        public override string ToString()
        {
            String result = " ";
            result += $"ID is {Id}, \n";
            result += $"SenderId is {SenderId}, \n";
            result += $"TargetId is {TargetId}, \n";
            result += $"Weight is {Weight}, \n";
            result += $"Priority is {Priority}, \n";
            result += $"Requested is {CreatParcel}, \n";
            result += $"Droneld is {DroneId}, \n";
            result += $"Scheduled is {Scheduled}, \n";
            result += $"PickedUp is {PickedUp}, \n";
            result += $"Delivered is {Delivered}, \n";
            return result;
        }
    }
}
