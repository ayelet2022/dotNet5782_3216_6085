using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel
    {
        /// <summary>
        /// the parcel id
        /// </summary>
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Recepter { get; set; }
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
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }
        public DroneInParcel ParecelDrone { get; set; }
        public override string ToString()
        {
            String result = "";
            result += $"ID: {Id}.\n";
            result += $"Sender: \n{Sender}\n";
            result += $"Recepter: \n{Recepter}\n";
            result += $"Weight: {Weight}.\n";
            result += $"Priority: {Priority}.\n";
            result += $"Requested: {CreatParcel}.\n";
            result += $"Scheduled: {Scheduled}.\n";
            result += $"PickedUp: {PickedUp}.\n";
            result += $"Delivered: {Delivered}.\n";
            return result;
        }
    }
}
