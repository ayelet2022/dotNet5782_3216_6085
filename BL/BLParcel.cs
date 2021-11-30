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
            if (parcel.Recepter.Id < 100000000 || parcel.Recepter.Id > 999999999)
                throw new InvalidInputException($"The parcel recepters id: {parcel.Id} is incorrect.\n");
            if (parcel.Sender.Id < 100000000 || parcel.Sender.Id > 999999999)
                throw new InvalidInputException($"The parcel senders id: {parcel.Id} is incorrect.\n");
            parcel.CreatParcel = DateTime.Now;
            parcel.Delivered = null;
            parcel.PickedUp = null;
            parcel.Scheduled = null;
            parcel.ParecelDrone = null;
            IDAL.DO.Parcel newParcel = new();
            object obj = newParcel;
            parcel.CopyPropertiesTo(obj);
            newParcel = (IDAL.DO.Parcel)obj;
            newParcel.SenderId = parcel.Sender.Id;
            newParcel.TargetId = parcel.Recepter.Id;
            dal.AddParcel(newParcel);
        }

        public Parcel GetParcel(int idParcel)
        {
            try
            {
                IDAL.DO.Parcel dalParcel = dal.GetParcels().First(item => item.Id == idParcel);
                Parcel parcel = new();//the parcel to returne
                dalParcel.CopyPropertiesTo(parcel);
                Customer sender = new();
                sender = GetCustomer(dalParcel.SenderId);
                parcel.Sender = new();
                sender.CopyPropertiesTo(parcel.Sender);
                Customer recepter = new();
                recepter = GetCustomer(dalParcel.TargetId);
                parcel.Recepter = new();
                recepter.CopyPropertiesTo(parcel.Recepter);
                if (dalParcel.DroneId == 0)//if ther is no drone scheduled to the paecel
                    parcel.ParecelDrone = default;
                else//ther is a drone
                {
                    DroneList droneInParcel = Drones.First(item => item.Id == dalParcel.DroneId);
                    parcel.ParecelDrone = new();
                    droneInParcel.CopyPropertiesTo(parcel.ParecelDrone);
                    parcel.ParecelDrone.DroneLocation = droneInParcel.DroneLocation;
                }
                return parcel;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidInputException($"The parcel id: {idParcel} was not found.\n", ex);
            }
        }

        public IEnumerable<ParcelList> GetParcels()
        {
            ParcelList parcel = new();
            List<ParcelList> Parcels = new();
            foreach (var item in dal.GetParcels())
            {
                item.CopyPropertiesTo(parcel);//copy only:id,weight,priority
                                              //findes the name of the customer that send the parcel
                parcel.SenderName = dal.GetCustomers().First(item1 => item1.Id == item.SenderId).Name;
                //findes the name of the customer that is recepting the parcel
                parcel.RecepterName = dal.GetCustomers().First(item1 => item1.Id == item.TargetId).Name;
                Parcels.Add(parcel);
                parcel = new();
            }
            return Parcels;
        }

        public IEnumerable<ParcelList> GetParcelThatWerenNotPaired()
        {
            ParcelList parcel = new();
            List<ParcelList> Parcels = new();
            foreach (var item in dal.GetParcels())
            {
                if (item.Scheduled == null)
                {
                    item.CopyPropertiesTo(parcel);//copy only:id,weight,priority
                    //findes the name of the customer that send the parcel
                    parcel.SenderName = dal.GetCustomers().First(item1 => item1.Id == item.SenderId).Name;
                    //findes the name of the customer that is recepting the parcel
                    parcel.RecepterName = dal.GetCustomers().First(item1 => item1.Id == item.TargetId).Name;
                    Parcels.Add(parcel);
                    parcel = new();
                }
            }
            return Parcels;
        }
    }
}
        
        
