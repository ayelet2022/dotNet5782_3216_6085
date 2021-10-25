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
        /// <summary>
        /// the parcel id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// the sender id
        /// </summary>
        public int SenderId { get; set; }
        /// <summary>
        /// the target id
        /// </summary>
        public int TargetId { get; set; }
        /// <summary>
        /// the drone id
        /// </summary>
        public int DroneId { get; set; }
        /// <summary>
        /// the parcel whight
        /// </summary>
        public WeightCategories Weight { get; set; }
        /// <summary>
        /// the parcel priority
        /// </summary>
        public Priorities Priority { get; set; }
        /// <summary>
        /// the time that the parcel was created
        /// </summary>
        public DateTime CreatParcel { get; set; }
        /// <summary>
        /// the time that a drone was peered to the parcel
        /// </summary>
        public DateTime Scheduled { get; set; }
        /// <summary>
        /// 
        /// </summary>
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
