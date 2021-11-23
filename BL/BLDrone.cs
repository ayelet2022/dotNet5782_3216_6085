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
        /// <summary>
        /// adds a new drone to the list
        /// </summary>
        /// <param name="drone">the drone to add</param>
        /// <param name="idFirstStation">the id of the first station that the drone is in</param>
        public void AddDrone(Drone drone, int idFirstStation)
        {
            Random Rand = new Random();
            try
            {
                if (drone.Id < 100000 || drone.Id > 999999)
                    throw new InvalidInputException($"The drones id:{drone.Id} is incorrect");
                if ((int)drone.MaxWeight < 0 || (int)drone.MaxWeight > 3)
                    throw new InvalidInputException($"The drones weight:{drone.MaxWeight} is incorrect");
                if (drone.Model.Length != 6)
                    throw new InvalidInputException($"The drones model{drone.Model} is incorrect.\n");
                if (idFirstStation < 1000 || idFirstStation > 9999)
                    throw new InvalidInputException($"The id{idFirstStation} of the baseStation incorrect.\n");
                IDAL.DO.BaseStation baseStation = dal.GetBaseStation(idFirstStation);
                drone.Battery = Rand.Next(20, 41);
                drone.Status = (DroneStatus)1;
                drone.ParcelInTransfer = default;
                IDAL.DO.DroneCharge droneCharge = new();
                droneCharge.DroneId = drone.Id;
                droneCharge.StationId = baseStation.Id;
                IDAL.DO.Drone drone1 = new();
                object obj = drone1;
                drone.CopyPropertiesTo(obj);
                drone1 = (IDAL.DO.Drone)obj;
                dal.AddDrone(drone1);
                dal.AddDroneCharge(droneCharge);
                DroneList droneList = new();
                drone.CopyPropertiesTo(droneList);
                droneList.NumOfParcelOnTheWay = 0;
                droneList.DroneLocation = new();
                droneList.DroneLocation.Latitude = baseStation.Latitude;
                droneList.DroneLocation.Longitude = baseStation.Longitude;
                Drones.Add(droneList);
            }
            catch (InvalidInputException ex)
            {
                throw new InvalidInputException(ex.ToString(), ex);
            }
            catch (FailedToAddException ex)
            {
                throw new FailedToAddException($"A base station with the  id:{idFirstStation} already exist.", ex);
            }
        }

        /// <summary>
        /// returne the drone that has the same id as what wes enterd
        /// </summary>
        /// <param name="idDrone">the id of the drone we want to returne</param>
        /// <returns>the drone that has the same id</returns>
        public Drone GetDrone(int idDrone)
        {
            try
            {
                DroneList droneList = new();
                //searching for the drone that has the same id
                droneList = Drones.Find(item => item.Id == idDrone);
                //meens ther is no drone with that id
                if (droneList == null)
                    throw new NotFoundInputException($"The input id: {idDrone} does not exist.\n");
                Drone returningDrone = new();
                droneList.CopyPropertiesTo(returningDrone);
                returningDrone.DroneLocation = new();
                returningDrone.DroneLocation = droneList.DroneLocation;
                if (droneList.NumOfParcelOnTheWay == 0)
                    returningDrone.ParcelInTransfer = default;
                else
                {
                    IDAL.DO.Parcel dalParcel = dal.GetParcel(droneList.NumOfParcelOnTheWay);
                    ParcelInTransfer parcelInT = new();
                    Parcel blParcel = GetParcel(dalParcel.Id);
                    blParcel.CopyPropertiesTo(parcelInT);
                    Customer blSenderCustomer = GetCustomer(blParcel.Sender.Id);
                    Customer blRecepterCustomer = GetCustomer(blParcel.Recepter.Id);
                    returningDrone.ParcelInTransfer.PickUpLocation = blSenderCustomer.CustomerLocation;
                    returningDrone.ParcelInTransfer.DelieveredLocation = blRecepterCustomer.CustomerLocation;
                    if (blParcel.PickedUp == DateTime.MinValue)
                        parcelInT.StatusParcel = false;
                    else
                        parcelInT.StatusParcel = true;
                    //the distatnce between the sender of the parcel to the resever
                    returningDrone.ParcelInTransfer.TransportDistance = Distance.Haversine
                        (blSenderCustomer.CustomerLocation.Latitude, blSenderCustomer.CustomerLocation.Longitude, blRecepterCustomer.CustomerLocation.Latitude, blRecepterCustomer.CustomerLocation.Longitude);
                }
                return returningDrone;
            }
            catch (IDAL.DO.DoesNotExistException ex)
            {
                throw new NotFoundInputException(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// copyes the values of all the drones in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the drones</returns>
        public IEnumerable<DroneList> GetDrones()
        {
            return Drones;
        }

        /// <summary>
        /// updates the drone that has the same id that was enterd
        /// </summary>
        /// <param name="id">the id of the drone we want to update</param>
        /// <param name="newModel">the new model of the drone</param>
        public void UpdateDrone(int id, string newModel)
        {
            try
            {
                //search if ther is such a dron with that id 
                dal.GetDrones().First(item => item.Id == id);
                //to update the drone in the drone list
                dal.UpdateDrone(id, newModel);
            }
            catch (InvalidOperationException ex)
            {
                throw new FailToUpdateException($"Couldn't update a drone with the id:{id} because it was not found", ex);
            }
        }

        /// <summary>
        /// sends the drone with the id that was enterd to a charger
        /// </summary>
        /// <param name="id">the id of the drone that we want to send to charging</param>
        public void SendDroneToCharging(int id)
        {
            try
            {
                DroneList droneList = Drones.First(item => item.Id == id);
                IDAL.DO.Drone dalDrone = dal.GetDrones().First(item => item.Id == id);
                IDAL.DO.BaseStation baseStation = new();
                //blDrone = GetDrone(id);
                baseStation = FindMinDistanceOfDToBSWithEempChar(droneList);
                //fined the distance frome a drone to a base station
                double distance = Distance.Haversine(baseStation.Longitude, baseStation.Latitude, droneList.DroneLocation.Latitude, droneList.DroneLocation.Longitude);
                //if the status is avilable or if the battery of the drone will last for him to get to the station in order to charge
                if (droneList.Status == (DroneStatus)0 && droneList.Battery > distance * dal.AskForBattery()[0])
                {
                    //to reduse from the battery the battery that he lost on the way to the station
                    droneList.Battery -= (int)(distance * dal.AskForBattery()[0]);
                    droneList.DroneLocation.Longitude = baseStation.Longitude;
                    droneList.DroneLocation.Latitude = baseStation.Latitude;
                    droneList.Status = (DroneStatus)1;//in fix
                    dal.DronToCharger(droneList.Id, baseStation.Id);//to add to the list of the drones that are charging
                }
                else
                    throw new FailedToChargeDroneException($"couldn't charge the drone:{id}.");
            }
            catch (IDAL.DO.DoesNotExistException ex)
            {
                throw new FailedToChargeDroneException($"couldn't charge the drone:{id} because the drone:{id} does not exist .", ex);
            }
            catch (FailedToChargeDroneException ex)
            {
                throw new FailedToChargeDroneException($"couldn't charge the drone:{id}.", ex);
            }
            catch (NotFoundInputException ex)
            {
                throw new FailedToChargeDroneException($"couldn't charge the drone:{id}.", ex);
            }
        }

        /// <summary>
        /// fineds the closest station to drone that has empty charger
        /// </summary>
        /// <param name="drone">the drone that we want to find the station for</param>
        /// <returns>the closest base station to drone that has an empty charger</returns>
        public IDAL.DO.BaseStation FindMinDistanceOfDToBSWithEempChar(DroneList drone)
        {
            IDAL.DO.BaseStation baseStation = new();
            bool flag = false;
            double minDistance = 0;
            double distance = 0;
            //to fo over all the station
            foreach (var item in dal.GetBaseStations())
            {
                //the distance between the drone and this station
                distance = Distance.Haversine(item.Latitude, item.Longitude, drone.DroneLocation.Latitude, drone.DroneLocation.Longitude);
                //check if the distance of this station is less then the one before
                if (minDistance == 0 || (minDistance > distance && item.EmptyCharges != 0))
                {
                    minDistance = distance;
                    baseStation = item;
                    flag = true;
                }
            }
            //meens there are no station that has empty chargers
            if (flag == false)
                throw new FailedToChargeDroneException();
            return baseStation;
        }
        /// <summary>
        /// findes the closest base station to the drone
        /// </summary>
        /// <param name="drone">the drone that we are searching for the closest station to him</param>
        /// <returns>the closest base station to the drone</returns>
        public IDAL.DO.BaseStation FindMinDistanceOfDToBS(Drone drone)
        {
            IDAL.DO.BaseStation baseStation = new();
            double minDistance = 0;
            double distance = 0;
            //to fo over all the station
            foreach (var item in dal.GetBaseStations())
            {
                //the distance between the drone and this station
                distance = Distance.Haversine(item.Latitude, item.Longitude, drone.DroneLocation.Latitude, drone.DroneLocation.Longitude);
                //check if the distance of this station is less then the one before
                if (minDistance == 0 || minDistance > distance)
                {
                    minDistance = distance;
                    baseStation = item;
                }
            }
            return baseStation;
        }
        /// <summary>
        /// fineds the closest base station to drone
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public IDAL.DO.BaseStation FindMinDistanceOfDToBS(double longitude, double latitude)
        {
            IDAL.DO.BaseStation baseStation = new();
            double minDistance = 0;
            double distance = 0;
            //to fo over all the station
            foreach (var item in dal.GetBaseStations())
            {
                //the distance between the drone and this station
                distance = Distance.Haversine(item.Latitude, item.Longitude, latitude, longitude);
                //check if the distance of this station is less then the one before
                if (minDistance == 0 || minDistance > distance)
                {
                    minDistance = distance;
                    baseStation = item;
                }
            }
            return baseStation;
        }

        /// <summary>
        /// to free a drone from a charger
        /// </summary>
        /// <param name="id">the id of the  drone we want to free</param>
        /// <param name="timeInCharger">how long was the drone charging</param>
        public void FreeDroneFromeCharger(int id, double timeInCharger)
        {
            try
            {
                DroneList drone = Drones.First(item=>item.Id==id);
                Drones.Remove(drone);
                int stationId = 0;
                if (drone.Status == (DroneStatus)1)
                {
                    //calculate how much battery the drone have now after charging
                    drone.Battery+= (int)(dal.AskForBattery()[4] * timeInCharger/60 + (dal.AskForBattery()[4] / 60) * timeInCharger%60);
                    if (drone.Battery > 100)
                        drone.Battery = 100;
                    drone.Status = (DroneStatus)0;//availble
                    //fineds the base station id where the drone is charging
                    foreach (var item in dal.GetDroneCharge())
                    {
                        if (item.DroneId == id)
                        {
                            stationId = item.StationId;
                            break;
                        }
                    }
                    //to delet the drone from the list of the drones that are charging
                    dal.FreeDroneFromBaseStation(drone.Id);
                    Drones.Add(drone);
                }
                else
                    throw new FailedFreeADroneFromeTheChargerException($"Failed to free the drone:{id} Frome The Charger.\n");
            }
            catch (NotFoundInputException ex)
            {
                throw new FailedFreeADroneFromeTheChargerException($"Failed to free the drone:{id} Frome The Charger.\n", ex);
            }
        }

        /// <summary>
        /// to paire a parcel to the drone that has the same id as what was enterd
        /// </summary>
        /// <param name="droneId">the drone we want to paire a parcel to</param>
        public void ScheduledAParcelToADrone(int droneId)
        {
            try
            {
                Drone drone = GetDrone(droneId);
                Parcel parcel = new();
                bool flag = false;
                //to go over all the parcels
                foreach (var item in dal.GetParcels())
                {
                    Customer senderOfParcel = GetCustomer(parcel.Sender.Id);
                    Customer senderOfItem = GetCustomer(item.SenderId);
                    Customer resepterOfItem = GetCustomer(item.TargetId);
                    BaseStation baseStationToCharge = GetBaseStation(FindMinDistanceOfCToBS(resepterOfItem.CustomerLocation.Latitude, resepterOfItem.CustomerLocation.Longitude).Id);
                    double disDroneToSenderP = DisDronToCustomer(drone, senderOfParcel);
                    double disDroneToSenderI = DisDronToCustomer(drone, senderOfItem);
                    double disReseverToBS = DisDronToBS(resepterOfItem, baseStationToCharge);
                    double disSenderToResepter = DisSenderToResever(senderOfItem, resepterOfItem);
                    //culcilates how much batery will leght in the drone after he get to the parcel to delever the parcel and if he needs to get also to a base station 
                    int battery = drone.Battery - ((int)(disDroneToSenderI * dal.AskForBattery()[(int)item.Weight + 1]) + (int)(disSenderToResepter * dal.AskForBattery()[(int)item.Weight + 1]) + (int)(disReseverToBS * dal.AskForBattery()[(int)item.Weight + 1]));
                    //if the priority of this parcel is higher then the one before and the drone has enugh battery in order to do the delivery and the wight is ok
                    if ((int)item.Priority > (int)parcel.Priority && battery > 0 && (int)item.Weight <= (int)drone.MaxWeight)
                        //if the wight of this parcel is heavier then the on before
                        if ((int)item.Weight > (int)parcel.Weight)
                        {
                            //the distatnce of this parcel his smaller then the distance of the parcel before
                            if (disDroneToSenderI < disDroneToSenderP)
                            {
                                parcel = GetParcel(item.Id);
                                flag = true;
                            }
                        }
                }
                //meens if found a parcel
                if (flag == true)
                {
                    Drones.Find(item => item.Id == drone.Id).Status = (DroneStatus)2;//update the drone to be in delivery
                    dal.UpdateParcelsScheduled(parcel.Id, droneId);
                }
                else
                    throw new FailedToScheduledAParcelToADroneException($"Failed sceduled a parcel to drone:{droneId}\n");
            }
            catch (NotFoundInputException ex)
            {
                throw new FailedFreeADroneFromeTheChargerException($"Failed sceduled a parcel to drone:{droneId}.\n", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new FailedToScheduledAParcelToADroneException($"Failed sceduled a parcel to drone:{droneId}.\n", ex);

            }
        }

        /// <summary>
        /// returne the distance between a drone and a customer
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        public double DisDronToCustomer(Drone drone, Customer customer)
        {
            return Distance.Haversine(drone.DroneLocation.Longitude, drone.DroneLocation.Latitude, customer.CustomerLocation.Longitude, customer.CustomerLocation.Latitude);
        }
        /// <summary>
        /// returne the distance between a drone and a base station
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        public double DisDronToBS(Customer customer, BaseStation station)
        {
            return Distance.Haversine(customer.CustomerLocation.Longitude, customer.CustomerLocation.Latitude, station.BaseStationLocation.Longitude, station.BaseStationLocation.Latitude);
        }

        /// <summary>
        /// a drone piched up a parcel
        /// </summary>
        /// <param name="id">the id of the drone that picked up the parcel</param>
        public void PickUpParcel(int id)
        {
            try
            {
                Drone blDrone = GetDrone(id);
                Customer customerS = GetCustomer(blDrone.ParcelInTransfer.Sender.Id);
                //the distance between the drone to the customer that send the parcel
                double dis = Distance.Haversine(blDrone.DroneLocation.Longitude, blDrone.DroneLocation.Latitude, customerS.CustomerLocation.Longitude, customerS.CustomerLocation.Latitude);
                //if there is no such drone and that ther are no parcels that the drone is in the middle of delivering 
                if (blDrone.ParcelInTransfer != default && blDrone.ParcelInTransfer.StatusParcel == false)
                {
                    //to reduse frome the battery that the drone lost because of the way
                    blDrone.Battery -= (int)(dis * dal.AskForBattery()[(int)(blDrone.ParcelInTransfer.Weight) + 1]);
                    blDrone.DroneLocation = blDrone.ParcelInTransfer.PickUpLocation;
                    dal.PickUpParcel(blDrone.ParcelInTransfer.Id);
                }
                else
                    throw new FailedToPickUpParcelException("couldn't pick up the parcel\n");
            }
            catch (FailedToPickUpParcelException ex)
            {
                throw new FailedToPickUpParcelException(ex.ToString(), ex);
            }
            catch (NotFoundInputException ex)
            {
                throw new FailedToPickUpParcelException("couldn't pick up the parcel\n", ex);
            }
            catch (IDAL.DO.DoesNotExistException ex)
            {
                throw new FailedToPickUpParcelException("couldn't pick up the parcel\n", ex);
            }
        }
        /// <summary>
        /// a drone deliverd a parcel
        /// </summary>
        /// <param name="id">the is of the drone that deliverd the parcel</param>
        public void DeliverParcel(int id)
        {
            try
            {
                Drone blDrone = GetDrone(id);
                Customer customerR = GetCustomer(blDrone.ParcelInTransfer.Recepter.Id);
                //the distance between the drone and the customer that reseved the parcel
                double dis = Distance.Haversine(blDrone.DroneLocation.Longitude, blDrone.DroneLocation.Latitude, customerR.CustomerLocation.Longitude, customerR.CustomerLocation.Latitude);
                //if there are parcels that are on the way on the drone
                if (blDrone.ParcelInTransfer.StatusParcel == true)
                {
                    //reduse the battery that the drone used on the way to deliver the parcel
                    blDrone.Battery -= (int)(dis * dal.AskForBattery()[(int)(blDrone.ParcelInTransfer.Weight) + 1]);
                    blDrone.DroneLocation = blDrone.ParcelInTransfer.DelieveredLocation;
                    blDrone.Status = (DroneStatus)0;//the drone is avilable
                    dal.ParcelToCustomer(blDrone.ParcelInTransfer.Id);
                }
                else
                    throw new FailedToDelieverParcelException("couldn't deliever the parcel\n");
            }
            catch (FailedToDelieverParcelException ex)
            {
                throw new FailedToDelieverParcelException(ex.ToString(), ex);
            }
            catch (NotFoundInputException ex)
            {
                throw new FailedToDelieverParcelException("couldn't deliever the parcel\n", ex);
            }
            catch (IDAL.DO.DoesNotExistException ex)
            {
                throw new FailedToDelieverParcelException("couldn't deliever the parcel\n", ex);
            }
        }
    }
}
