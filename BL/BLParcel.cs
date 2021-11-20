using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
namespace BL
{
    public partial class BL
    {
        public void AddParcel(Parcel parcel)
        {
            if (parcel.Id < 100000000 || parcel.Id > 999999999)
                throw new InvalidInputException($"The parcel id:{parcel.Id} is incorrect");
            if (parcel.Sender.Id < 100000000 || parcel.Sender.Id > 999999999)
                throw new InvalidInputException($"The parcel id:{parcel.Id} is incorrect");
            parcel.CreatParcel = DateTime.Now;
            parcel.Delivered = DateTime.MinValue;
            parcel.PickedUp = DateTime.MinValue;
            parcel.Scheduled = DateTime.MinValue;
            parcel.ParecelDrone = null;
            IDAL.DO.Parcel newParcel = new();
            newParcel.CopyPropertiesTo(newParcel);
            dal.AddParcel(newParcel);
        }

        public Parcel GetParcel(int idParcel)
        {
            IDAL.DO.Parcel dalParcel = dal.GetParcels().First(item => item.Id == idParcel);
            Parcel parcel = new();//the parcel to returne
            parcel.CopyPropertiesTo(dalParcel);
            Customer sender = GetCustomer(dalParcel.SenderId);
            parcel.Sender.CopyPropertiesTo(sender);
            Customer recepter = GetCustomer(dalParcel.TargetId);
            parcel.Recepter.CopyPropertiesTo(recepter);
            if (dalParcel.DroneId == 0)//if ther is no drone scheduled to the paecel
                parcel.ParecelDrone = default;
            else//ther is a drone
            {
                Drone droneInParcel = GetDrone(dalParcel.DroneId);
                parcel.ParecelDrone.CopyPropertiesTo(droneInParcel);
            }
            return parcel;
        }

        /// <summary>
        /// copyes the values of all the parcel in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the parceles</returns>
        public IEnumerable<ParcelList> GetParcels()
        {
            ParcelList parcel = new();
            List<ParcelList> Parcels = new();
            foreach (var item in dal.GetParcels())
            {
                parcel.CopyPropertiesTo(item);//copy only:id,weight,priority
                //findes the name of the customer that send the parcel
                parcel.SenderName = dal.GetCustomers().First(item1 => item1.Id == item.SenderId).Name;
                //findes the name of the customer that is recepting the parcel
                parcel.RecepterName=dal.GetCustomers().First(item1 => item1.Id == item.SenderId).Name;
                Parcels.Add(parcel);
            }
            return Parcels;
        }

        /// <summary>
        /// search for all the drones that are available and copy them to a new arrey  
        /// </summary>
        /// <returns>the new arrey that has all the drones that are availeble</returns>
        public IEnumerable<ParcelList> GetParcelThatWerenNotPaired()
        {
            ParcelList parcel = new();
            List<ParcelList> Parcels = new();
            foreach (var item in dal.GetParcels())
            {
                if(item.Scheduled==DateTime.MinValue)
                {
                    parcel.CopyPropertiesTo(item);//copy only:id,weight,priority
                                                  //findes the name of the customer that send the parcel
                    parcel.SenderName = dal.GetCustomers().First(item1 => item1.Id == item.SenderId).Name;
                    //findes the name of the customer that is recepting the parcel
                    parcel.RecepterName = dal.GetCustomers().First(item1 => item1.Id == item.SenderId).Name;
                    Parcels.Add(parcel);
                }
            }
            return Parcels;
        }
        public void PickUpParcel(int id)
        {
            Drone blDrone = GetDrone(id);
            if (blDrone.ParcelInTransfer != default && blDrone.ParcelInTransfer.StatusParcel == false)
            {
                blDrone.Battery -= (int)(blDrone.ParcelInTransfer.TransportDistance * dal.AskForBattery()[(int)(blDrone.ParcelInTransfer.Weight) + 1]);
                blDrone.DroneLocation = blDrone.ParcelInTransfer.PickUpLocation;
                dal.PickUpParcel(blDrone.ParcelInTransfer.Id);
            }
            else
                throw new FailedToPickUpParcelException("couldn't pick up the parcel");
        }
        public void DeliverParcel(int id)
        {
            Drone blDrone = GetDrone(id);
            Customer customerR = GetCustomer(blDrone.ParcelInTransfer.Recepter.Id);
            double dis = Distance.Haversine(blDrone.DroneLocation.Longitude, blDrone.DroneLocation.Latitude, customerR.CustomerLocation.Longitude, customerR.CustomerLocation.Latitude);
            if (blDrone.ParcelInTransfer.StatusParcel == true)
            {
                blDrone.Battery -= (int)(dis * dal.AskForBattery()[(int)(blDrone.ParcelInTransfer.Weight) + 1]);
                blDrone.DroneLocation = blDrone.ParcelInTransfer.DelieveredLocation;
                blDrone.Status = (DroneStatus)0;
                dal.ParcelToCustomer(blDrone.ParcelInTransfer.Id);
            }
            else
                throw new FailedToDelieverParcelException("couldn't deliever the parcel");

        }
    }
}
