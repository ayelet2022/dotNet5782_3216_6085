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
            try
            {
                IDAL.DO.Customer customerR = dal.GetCustomer(parcel.Recepter.Id);
                IDAL.DO.Customer customers = dal.GetCustomer(parcel.Sender.Id);
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
            catch(IDAL.DO.DoesNotExistException ex)
            {
                throw new InvalidInputException($"The parcel sender's id or recepter's id is incorrect.\n", ex);
            }
        }

        public Parcel GetParcel(int idParcel)
        {
            try
            {
                IDAL.DO.Parcel dalParcel = dal.GetParcel(idParcel);
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
            catch (IDAL.DO.DoesNotExistException ex)
            {
                throw new InvalidInputException($"The parcel id: {idParcel} was not found.\n", ex);
            }
        }

        public IEnumerable<ParcelList> GetParcels(Predicate<ParcelList> predicate = null)
        {
            ParcelList parcel = new();
            List<ParcelList> Parcels = new();
            foreach (var item in dal.GetParcels())
            {
                item.CopyPropertiesTo(parcel);//copy only:id,weight,priority
                //findes the name of the customer that send the parcel
                parcel.SenderName = dal.GetCustomer(item.SenderId).Name;
                //findes the name of the customer that is recepting the parcel
                parcel.RecepterName = dal.GetCustomer(item.TargetId).Name;
                if (item.Delivered != null)//if parcel was delivered
                    parcel.ParcelStatus = ParcelStatus.delivery;//state -> provided
                else
                {
                    if (item.PickedUp != null)//if parcel was picked up by drone
                        parcel.ParcelStatus = ParcelStatus.pickup;//state -> picked up
                    else
                    {
                        if (item.Scheduled != null)//if if parcel was assigned to drone
                            parcel.ParcelStatus = ParcelStatus.schedul;//state -> paired
                        else//if parcel was requested
                            parcel.ParcelStatus = ParcelStatus.creat;//state -> created
                    }
                }
                Parcels.Add(parcel);
                parcel = new();
            }
            return Parcels.FindAll(item => predicate == null ? true : predicate(item));
        }
    }
}
        
        
