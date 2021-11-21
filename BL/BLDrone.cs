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
                returningDrone.ParcelInTransfer.TransportDistance = Distance.Haversine(blSenderCustomer.CustomerLocation.Latitude, blSenderCustomer.CustomerLocation.Longitude, blRecepterCustomer.CustomerLocation.Latitude, blRecepterCustomer.CustomerLocation.Longitude);
            }
            return returningDrone;
        }

        /// <summary>
        /// copyes the values of all the drones in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the drones</returns>
        public IEnumerable<DroneList> GetDrones()
        {
            return Drones;
        }

        public void UpdateDrone(int id, string newModel)
        {
            dal.GetDrones().First(item => item.Id == id);
            dal.UpdateDrone(id, newModel);
        }

        public void SendDroneToCharging(int id)
        {
            Drone blDrone = GetDrone(id);
            try 
            {
                IDAL.DO.BaseStation baseStation = FindMinDistanceDtoS(blDrone);
                double distance = Distance.Haversine(baseStation.Longitude, baseStation.Latitude, blDrone.DroneLocation.Latitude, blDrone.DroneLocation.Longitude);
                if (blDrone.Status == (DroneStatus)0 && blDrone.Battery > distance * dal.AskForBattery()[0])
                {
                    blDrone.Battery -= (int)(distance * dal.AskForBattery()[0]);
                    blDrone.DroneLocation.Longitude = baseStation.Longitude;
                    blDrone.DroneLocation.Latitude = baseStation.Latitude;
                    blDrone.Status = (DroneStatus)1;
                    dal.DronToCharger(blDrone.Id, baseStation.Id);
                }
                else
                    throw new FailedToChargeDroneException();
            }
            catch (Exception ex)
            {
                throw new FailedToChargeDroneException("couldn't charge the drone.");
            }    
        }

        public IDAL.DO.BaseStation FindMinDistanceOfDToBS(Drone drone)
        {
            IDAL.DO.BaseStation baseStation = new();
            bool flag = false;
            double minDistance = 0;
            double distance = 0;
            foreach (var item in dal.GetBaseStations())
            {
                distance = Distance.Haversine(item.Latitude, item.Longitude, drone.DroneLocation.Latitude, drone.DroneLocation.Longitude);
                if (minDistance > distance && item.EmptyCharges != 0)
                {
                    minDistance = distance;
                    baseStation = item;
                    flag= true;
                }
            }
            if (flag == false)
                throw new FailedToChargeDroneException();
            return baseStation;
        }
        public IDAL.DO.BaseStation FindMinDistanceOfCToBS(Customer customer)
        {
            IDAL.DO.BaseStation baseStation = new();
            double minDistance = 0;
            double distance = 0;
            foreach (var item in dal.GetBaseStations())
            {
                distance = Distance.Haversine(item.Latitude, item.Longitude, customer.CustomerLocation.Latitude, customer.CustomerLocation.Longitude);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    baseStation = item;
                }
            }
            return baseStation;
        }
        public void FreeDroneFromeCharger(int id,DateTime timeInCharger)
        {

            Drone drone = GetDrone(id);
            int stationId=0;
            if (drone.Status == (DroneStatus)1)
            {
                drone.Battery = (int)(dal.AskForBattery()[4] * timeInCharger.Hour+ (dal.AskForBattery()[4] / 60) * timeInCharger.Minute+(dal.AskForBattery()[4]/360)*timeInCharger.Second);
                drone.Status = (DroneStatus)0;//availble
                foreach (var item in dal.GetDroneCharge())//fineds the base station id where the drone is charging
                {
                    if (item.DroneId == id)
                    {
                        stationId = item.StationId;
                        break;
                    }
                }
                GetBaseStation(stationId).EmptyCharges++;
                dal.DeleteDroneFromeCharger(stationId);
            }
            throw new FailedFreeADroneFromeTheChargerException($"Failed to free the drone:{id} Frome The Charger");
        }

        public void ScheduledAParcelToADrone(int droneId)
        {
            Drone drone = GetDrone(droneId);
            Parcel parcel = new();
            bool flag = false;
            foreach (var item in dal.GetParcels())
            {
                Customer senderOfParcel = GetCustomer(parcel.Sender.Id);
                Customer senderOfItem = GetCustomer(item.SenderId);
                Customer resepterOfItem = GetCustomer(item.TargetId);
                BaseStation baseStationToCharge = GetBaseStation(FindMinDistanceOfCToBS(resepterOfItem).Id);
                double disDroneToSenderP = DisDronToCustomer(drone, senderOfParcel);
                double disDroneToSenderI = DisDronToCustomer(drone, senderOfItem);
                double disReseverToBS = DisDronToBS(resepterOfItem, baseStationToCharge);
                double disSenderToResepter = DisSenderToResever(senderOfItem, resepterOfItem);
                //culcilates how much batery will leght in the drone after he get to the parcel to delever the parcel and if he needs to get also to a base station 
                int battery = drone.Battery-((int)(disDroneToSenderI * dal.AskForBattery()[(int)item.Weight+ 1])+ (int)(disSenderToResepter * dal.AskForBattery()[(int)item.Weight + 1]) + (int)(disReseverToBS * dal.AskForBattery()[(int)item.Weight + 1]));
                if ((int)item.Priority > (int)parcel.Priority&&battery>0)
                    if((int)item.Weight > (int)parcel.Weight)
                    {
                        if (disDroneToSenderI < disDroneToSenderP)
                        {
                            parcel = GetParcel(item.Id);
                            flag = true;
                        }
                    }
            }
            if (flag == true)
            {
                Drones.Find(item => item.Id == drone.Id).Status = (DroneStatus)2;//update the drone to be in delivery
                dal.UpdateParcelsScheduled(parcel.Id, droneId);
            }
            else
                throw new FailedToScheduledAParcelToADroneException($"Failed sceduled a parcel to drone:{droneId}");
        }

        public double DisDronToCustomer(Drone drone,Customer customer)
        {
            return Distance.Haversine(drone.DroneLocation.Longitude, drone.DroneLocation.Latitude, customer.CustomerLocation.Longitude, customer.CustomerLocation.Latitude);
        }
        public double DisDronToBS(Customer customer, BaseStation station)
        {
            return Distance.Haversine(customer.CustomerLocation.Longitude, customer.CustomerLocation.Latitude, station.BaseStationLocation.Longitude, station.BaseStationLocation.Latitude);
        }
        public double DisSenderToResever(Customer sender, Customer resever)
        {
            return Distance.Haversine(sender.CustomerLocation.Longitude, sender.CustomerLocation.Latitude, resever.CustomerLocation.Longitude, resever.CustomerLocation.Latitude);
        }
    }
}
