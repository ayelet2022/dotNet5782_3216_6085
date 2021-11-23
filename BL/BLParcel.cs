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
                throw new InvalidInputException($"The parcel recepters id:{parcel.Id} is incorrect");
            if (parcel.Sender.Id < 100000000 || parcel.Sender.Id > 999999999)
                throw new InvalidInputException($"The parcel senders id:{parcel.Id} is incorrect");
            parcel.CreatParcel = DateTime.Now;
            parcel.Delivered = DateTime.MinValue;
            parcel.PickedUp = DateTime.MinValue;
            parcel.Scheduled = DateTime.MinValue;
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
                    Drone droneInParcel = GetDrone(dalParcel.DroneId);
                    parcel.ParecelDrone = new();
                    droneInParcel.CopyPropertiesTo(parcel.ParecelDrone);
                }
                return parcel;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidInputException($"The input id: {idParcel} does not exist.\n", ex);
            }
            catch(InvalidInputException ex)
            {
                throw new InvalidInputException($"The input id: {idParcel} does not exist.\n", ex);
            }
        }

        /// <summary>
        /// copyes the values of all the parcel in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the parceles</returns>
        public IEnumerable<ParcelList> GetParcels()
        {
            try
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
            catch(InvalidOperationException ex)
            {
                throw new InvalidInputException("The input does not exist.\n", ex);
            }
        }

        /// <summary>
        /// search for all the drones that are available and copy them to a new arrey  
        /// </summary>
        /// <returns>the new arrey that has all the drones that are availeble</returns>
        public IEnumerable<ParcelList> GetParcelThatWerenNotPaired()
        {
            try
            {
                ParcelList parcel = new();
                List<ParcelList> Parcels = new();
                foreach (var item in dal.GetParcels())
                {
                    if (item.Scheduled == DateTime.MinValue)
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
            catch (InvalidOperationException ex)
            {
                throw new InvalidInputException("The SenderName/RecepterName does not exist.\n", ex);
            }
        }
        public void PickUpParcel(int id)
        {
            try
            {
                Drone blDrone = GetDrone(id);
                Customer customerS = GetCustomer(blDrone.ParcelInTransfer.Sender.Id);
                double dis = Distance.Haversine(blDrone.DroneLocation.Longitude, blDrone.DroneLocation.Latitude, customerS.CustomerLocation.Longitude, customerS.CustomerLocation.Latitude);
                if (blDrone.ParcelInTransfer != default && blDrone.ParcelInTransfer.StatusParcel == false)
                {
                    blDrone.Battery -= (int)(dis * dal.AskForBattery()[(int)(blDrone.ParcelInTransfer.Weight) + 1]);
                    blDrone.DroneLocation = blDrone.ParcelInTransfer.PickUpLocation;
                    dal.PickUpParcel(blDrone.ParcelInTransfer.Id);
                }
                else
                    throw new FailedToPickUpParcelException("couldn't pick up the parcel");
            }
            catch(FailedToPickUpParcelException ex)
            {
                throw new FailedToPickUpParcelException(ex.ToString(), ex);
            }
            catch(NotFoundInputException ex)
            {
                throw new FailedToPickUpParcelException("couldn't pick up the parcel", ex);
            }
            catch(IDAL.DO.DoesNotExistException ex)
            {
                throw new FailedToPickUpParcelException("couldn't pick up the parcel", ex);
            }
        }
        public void DeliverParcel(int id)
        {
            try
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
            catch (FailedToDelieverParcelException ex)
            {
                throw new FailedToDelieverParcelException(ex.ToString(), ex);
            }
            catch (NotFoundInputException ex)
            {
                throw new FailedToDelieverParcelException("couldn't deliever the parcel", ex);
            }
            catch (IDAL.DO.DoesNotExistException ex)
            {
                throw new FailedToDelieverParcelException("couldn't deliever the parcel", ex);
            }
        }
    }
}
