using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BO;
namespace BL
{
    public partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel parcel)
        {
            lock (dal)
            {
                try
                {
                    DO.Customer customerR = dal.GetCustomer(parcel.Recepter.Id);
                    DO.Customer customers = dal.GetCustomer(parcel.Sender.Id);
                    parcel.CreatParcel = DateTime.Now;
                    parcel.Delivered = null;
                    parcel.PickedUp = null;
                    parcel.Scheduled = null;
                    parcel.ParecelDrone = null;
                    DO.Parcel newParcel = new();
                    object obj = newParcel;
                    parcel.CopyPropertiesTo(obj);
                    newParcel = (DO.Parcel)obj;
                    newParcel.SenderId = parcel.Sender.Id;
                    newParcel.TargetId = parcel.Recepter.Id;
                    dal.AddParcel(newParcel);
                }
                catch (DO.DoesNotExistException ex)
                {
                    throw new InvalidInputException($"The parcel sender's id or recepter's id is incorrect.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int idParcel)
        {
            lock (dal)
            {
                try
                {
                    DO.Parcel dalParcel = dal.GetParcel(idParcel);
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
                catch (DO.DoesNotExistException ex)
                {
                    throw new InvalidInputException($"The parcel id: {idParcel} was not found.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelList> GetParcels(Predicate<ParcelList> predicate = null)
        {
            lock (dal)
            {
                IEnumerable<ParcelList> parcels;
                parcels = from item in dal.GetParcels()
                         select new ParcelList
                         {
                             Id = item.Id,
                             SenderName = dal.GetCustomer(item.SenderId).Name,
                             RecepterName = dal.GetCustomer(item.TargetId).Name,
                             Weight = (WeightCategories)item.Weight,
                             Priority = (Priorities)item.Priority,
                             ParcelStatus= item.Delivered != null ? BO.ParcelStatus.delivery :
                             (item.PickedUp != null ? BO.ParcelStatus.pickup :
                             (item.Scheduled != null ? BO.ParcelStatus.schedul : BO.ParcelStatus.creat))
            };
                return parcels.Where(item => predicate == null ? true : predicate(item));
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(int id)
        {
            try
            {
                lock (dal)
                    dal.DeleteParcel(id);
            }
            catch (DO.ItemIsDeletedException ex)
            {
                throw new ItemIsDeletedException($"Parcel: { id } is already deleted.");
            }
        }
    }
}


