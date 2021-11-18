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
        public void AddDrone(Drone drone,int idFirstStation)
        {
            Random Rand = new Random(DateTime.Now.Millisecond);
            try
            {
                if (drone.Id < 100000 || drone.Id > 999999)
                    throw new InvalidInputException("The drones id is incorrect");
                if ((int)drone.MaxWeight < 0 || (int)drone.MaxWeight > 3)
                    throw new InvalidInputException("The drones weight is incorrect");
                if (drone.Model.Length != 6)
                    throw new InvalidInputException("The drones model is incorrect");
                if (idFirstStation < 1000 || idFirstStation > 9999)
                    throw new InvalidInputException("The id of the baseStation incorrect");
                IDAL.DO.BaseStation baseStation = dal.GetBaseStation(idFirstStation);
                drone.Battery = Rand.Next(20, 41);
                drone.Status = (DroneStatus)1;
                drone.DroneLocation.Latitude = baseStation.Latitude;
                drone.DroneLocation.Longitude = baseStation.Longitude;
                drone.ParcelInTransfer = default;
                IDAL.DO.DroneCharge droneCharge = new();
                droneCharge.DroneId = drone.Id;
                droneCharge.StationId = baseStation.Id;
                IDAL.DO.Drone drone1 = new();
                drone.CopyPropertiesTo(drone1);
                dal.AddDrone(drone1);
                dal.AddDroneCharge(droneCharge);
                DroneList droneList = new();
                droneList.CopyPropertiesTo(drone);
                droneList.NumOfParcelOnTheWay = 0;
                Drones.Add(droneList);
            }
            catch (IDAL.DO.ExistsException ex)
            {
                throw new FailedToAddException(ex.ToString(), ex);
            }
            catch (IDAL.DO.DoesNotExistException ex)
            {
                throw new FailedToAddException($"The base station id:{idFirstStation} does not exist.", ex);
            }
        }

        public Drone GetDrone(int idDrone)
        {
            DroneList droneList = Drones.Find(item => item.Id == idDrone);
            if (droneList == default)
                throw new NotFoundInputException($"The input id: {idDrone} does not exist.\n");
            Drone returningDrone = new();
            returningDrone.CopyPropertiesTo(droneList);
            if (droneList.NumOfParcelOnTheWay == 0)
                returningDrone.ParcelInTransfer = default;
            else
            {
                IDAL.DO.Parcel dalParcel = dal.GetParcel(droneList.NumOfParcelOnTheWay);
                ParcelInTransfer parcelInT = new();
                Parcel blParcel = GetParcel(dalParcel.Id);
                parcelInT.CopyPropertiesTo(blParcel);
                Customer blSenderCustomer = GetCustomer(blParcel.Sender.Id);
                Customer blRecepterCustomer = GetCustomer(blParcel.Recepter.Id);
                returningDrone.ParcelInTransfer.PickUpLocation = blSenderCustomer.CustomerLocation;
                returningDrone.ParcelInTransfer.DelieveredLocation = blRecepterCustomer.CustomerLocation;
                if (blParcel.PickedUp == DateTime.MinValue)
                    parcelInT.StatusParcel = false;
                else
                    parcelInT.StatusParcel = true;
                returningDrone.ParcelInTransfer.TransportDistance = Distance.Haversine
                (blSenderCustomer.CustomerLocation.Latitude, blSenderCustomer.CustomerLocation.Longitude, blRecepterCustomer.CustomerLocation.Latitude, blRecepterCustomer.CustomerLocation.Longitude);
            }
            return returningDrone;
        }

        /// <summary>
        /// copyes the values of all the drones in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the drones</returns>
        public IEnumerable<DroneList> GetDrones()
        {
            List<Drone> blDrones = new List<Drone>();
            List<DroneList> listDrones = new List<DroneList>();
            IEnumerable<IDAL.DO.Drone> dalDrones = dal.GetDrones();
            int i = 0;
            foreach (var item in dalDrones)
            {
                blDrones[i] = GetDrone(item.Id);
                listDrones[i].CopyPropertiesTo(blDrones[i]);
                listDrones[i].NumOfParcelOnTheWay = blDrones[i].ParcelInTransfer.Id;
                i++;
            }
            return listDrones;
        }
    }
}
