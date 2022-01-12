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
        public void AddDrone(Drone drone, int idFirstStation)
        {
            lock (dal)
            {
                Random Rand = new Random();
                if (drone.Id < 100000 || drone.Id > 999999)
                    throw new InvalidInputException($"The drones id: {drone.Id} is incorrect, the drone was not added.\n");
                if ((int)drone.MaxWeight < 0 || (int)drone.MaxWeight > 2)
                    throw new InvalidInputException($"The drones weight: {drone.MaxWeight} is incorrect, the drone was not added.\n");
                if (drone.Model == "" || drone.Model == null)
                    throw new InvalidInputException($"The drones model: {drone.Model} is incorrect, the drone was not added.\n");
                if (idFirstStation < 1000 || idFirstStation > 9999)
                    throw new InvalidInputException($"The id: {idFirstStation} of the baseStation incorrect, the drone was not added.\n");
                try
                {
                    DO.BaseStation baseStation = dal.GetBaseStation(idFirstStation);
                    drone.Battery = Rand.Next(20, 41);
                    drone.Status = (DroneStatus)1;
                    drone.ParcelInTransfer = default;
                    DO.Drone drone1 = new();
                    object obj = drone1;
                    drone.CopyPropertiesTo(obj);
                    drone1 = (DO.Drone)obj;
                    drone1.IsActive = true;
                    dal.AddDrone(drone1);
                    DroneList droneList = new();
                    drone.CopyPropertiesTo(droneList);
                    droneList.NumOfParcelOnTheWay = 0;
                    droneList.DroneLocation = new();
                    droneList.DroneLocation.Latitude = baseStation.Latitude;
                    droneList.DroneLocation.Longitude = baseStation.Longitude;
                    droneList.IsActive = true;
                    Drones.Add(droneList);
                    dal.DronToCharger(drone.Id, idFirstStation);
                }
                catch (DO.ExistsException ex)
                {
                    throw new FailedToAddException($"The drones id: {drone.Id} already exists.\n", ex);
                }
                catch (DO.DoesNotExistException ex)
                {
                    throw new FailedToAddException($"Drone id: {drone.Id} does not exist.\n", ex);
                }
                catch (InvalidInputException ex)
                {
                    throw new FailedToAddException($"Drone id: {drone.Id} does not exist.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int idDrone)
        {
            lock (dal)
            {
                try
                {
                    DroneList droneList = new();
                    //searching for the drone that has the same id
                    droneList = Drones.First(item => item.Id == idDrone && item.IsActive);
                    //meens ther is no drone with that id
                    Drone returningDrone = new();
                    droneList.CopyPropertiesTo(returningDrone);
                    returningDrone.DroneLocation = new();
                    returningDrone.DroneLocation = droneList.DroneLocation;
                    if (droneList.NumOfParcelOnTheWay == 0)
                        returningDrone.ParcelInTransfer = default;
                    else
                    {
                        DO.Parcel dalParcel = dal.GetParcel(droneList.NumOfParcelOnTheWay);
                        ParcelInTransfer parcelInT = new();
                        Parcel blParcel = GetParcel(dalParcel.Id);
                        blParcel.CopyPropertiesTo(parcelInT);
                        Customer blSenderCustomer = GetCustomer(blParcel.Sender.Id);
                        Customer blRecepterCustomer = GetCustomer(blParcel.Recepter.Id);
                        parcelInT.Sender = new();
                        parcelInT.Recepter = new();
                        parcelInT.Sender.Id = blSenderCustomer.Id;
                        parcelInT.Sender.Name = blSenderCustomer.Name;
                        parcelInT.Recepter.Id = blRecepterCustomer.Id;
                        parcelInT.Recepter.Name = blRecepterCustomer.Name;
                        parcelInT.PickUpLocation = blSenderCustomer.CustomerLocation;
                        parcelInT.DelieveredLocation = blRecepterCustomer.CustomerLocation;
                        if (blParcel.PickedUp == null)
                            parcelInT.StatusParcel = false;
                        else
                            parcelInT.StatusParcel = true;
                        //the distatnce between the sender of the parcel to the resever
                        parcelInT.TransportDistance = Distance.Haversine
                         (blSenderCustomer.CustomerLocation.Latitude, blSenderCustomer.CustomerLocation.Longitude, blRecepterCustomer.CustomerLocation.Latitude, blRecepterCustomer.CustomerLocation.Longitude);
                        returningDrone.ParcelInTransfer = parcelInT;
                    }
                    return returningDrone;
                }
                catch (InvalidOperationException ex)
                {
                    throw new NotFoundInputException($"The drone: {idDrone} was not found.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneList> GetDrones(Predicate<DroneList> predicate = null)
        {
            lock (dal)
            {
                IEnumerable<DroneList> droneList = from item in Drones
                                                   where (predicate == null || predicate(item))&&item.IsActive
                                                   select item;
                return droneList;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(Drone drone, string newModel)
        {
            lock (dal)
            {
                try
                {
                    DroneList drone1 = Drones.First(x => x.Id == drone.Id && x.IsActive);
                    drone1.Model = newModel;
                    //to update the drone in the drone list
                    dal.UpdateDrone(drone.Id, newModel);
                }
                catch (DO.DoesNotExistException ex)
                {
                    throw new FailToUpdateException($"The drone: {drone.Id} was not found, the drone was not updated.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SendDroneToCharging(int id)
        {
            lock (dal)
            {
                try
                {
                    DroneList droneList = Drones.First(item => item.Id == id && item.IsActive);
                    DO.Drone dalDrone = dal.GetDrone(id);
                    DO.BaseStation baseStation = new();
                    //droneList = GetDrone(id);
                    baseStation = FindMinDistanceOfDToBSWithEempChar(droneList);
                    //fined the distance frome a drone to a base station
                    double distance = Distance.Haversine(baseStation.Longitude, baseStation.Latitude, droneList.DroneLocation.Latitude, droneList.DroneLocation.Longitude);
                    //if the status is avilable or if the battery of the drone will last for him to get to the station in order to charge
                    if (droneList.Status == DroneStatus.available && droneList.Battery > distance * dal.AskForBattery()[0])
                    {
                        //to reduse from the battery the battery that he lost on the way to the station
                        droneList.Battery -= (int)(distance * dal.AskForBattery()[0]);
                        droneList.DroneLocation.Longitude = baseStation.Longitude;
                        droneList.DroneLocation.Latitude = baseStation.Latitude;
                        droneList.Status = (DroneStatus)1;//in fix
                        dal.DronToCharger(droneList.Id, baseStation.Id);//to add to the list of the drones that are charging
                    }
                    else
                        throw new FailToUpdateException($"Didn't send the drone: {id} to charging.\n");
                }
                catch (InvalidOperationException ex)
                {
                    throw new FailToUpdateException($"The drone: {id} was not found, didn't send the drone: {id} to charging..\n", ex);
                }
                catch (FailToUpdateException ex)
                {
                    throw new FailToUpdateException($"The drone: {id} is not charging.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void FreeDroneFromeCharger(int id)
        {
            lock (dal)
            {
                try
                {
                    DroneList drone = Drones.First(item => item.Id == id && item.IsActive);
                    if (drone.Status == DroneStatus.inFix)
                    {
                        //calculate how much battery the drone have now after charging
                        TimeSpan timeInCharger = (TimeSpan)(DateTime.Now - dal.GetDroneCharge(drone.Id).StartCharging);
                        // timeInCharger = DateTime.Now - dal.GetDroneCharge(x => x.DroneId == drone.Id).StartCharging;
                        drone.Battery += (int)(timeInCharger.TotalHours * dal.AskForBattery()[4]);
                        if (drone.Battery > 100)
                            drone.Battery = 100;
                        drone.Status = DroneStatus.available;//availble
                                                             //to delet the drone from the list of the drones that are charging
                        dal.FreeDroneFromBaseStation(drone.Id);
                    }
                    else
                        throw new FailToUpdateException($"Failed to free the drone: {id} from The Charger.\n");
                }
                catch (InvalidOperationException ex)
                {
                    throw new FailToUpdateException($"The drone: {id} was not found, the drone: {id} was not freed from Charger.\n", ex);
                }
                catch (FailToUpdateException ex)
                {
                    throw new FailToUpdateException($"Failed to free the drone: {id} Frome The Charger.\n", ex);
                }
                catch (DO.NotFoundInputException ex)
                {
                    throw new FailToUpdateException($"The drone: {id} is not charging.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ScheduledAParcelToADrone(int droneId)
        {
            lock (dal)
            {
                try
                {
                    DroneList drone = Drones.First(item => item.Id == droneId && item.IsActive);
                    if (drone.Status == DroneStatus.delivery)//meens the drone is in delivery
                        throw new FailedToScheduledAParcelToADroneException($"Failed to sceduled a parcel to drone:{droneId}.\n");
                    ParcelList parcel = default;
                    //bool foundParcel = false;
                    //int battery = 0;
                    //to go over all the parcels
                    parcel = GetParcels(p => p.ParcelStatus == ParcelStatus.creat
                                           && p.Weight <= drone.MaxWeight
                                           && UseOfBattery(p.Id, drone.Id) < drone.Battery)
                                           .OrderByDescending(x => x.Priority)
                                           .ThenByDescending(x => x.Weight)
                                           .FirstOrDefault();
                    if (parcel != default)
                    {
                        Drones.Find(item => item.Id == drone.Id).Status = DroneStatus.delivery;//update the drone to be in delivery
                        dal.DronToAParcel(droneId, parcel.Id);
                        drone.NumOfParcelOnTheWay = parcel.Id;
                    }
                    else
                        throw new FailToUpdateException($"Failed to sceduled a parcel to drone:{droneId}.\n");
                }
                catch (InvalidOperationException ex)
                {
                    throw new FailToUpdateException($"The Drone: {droneId} was not found, a parcel was not scheduled to the drone: {droneId}.\n", ex);
                }
                catch (FailToUpdateException ex)
                {
                    throw new FailToUpdateException($"Failed to sceduled a parcel to drone:{droneId}.\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void PickUpParcel(int id)
        {
            lock (dal)
            {
                try
                {
                    Drone drone = GetDrone(id);
                    DroneList droneList = Drones.First(item => item.Id == id && item.IsActive);
                    if (drone.ParcelInTransfer == null)
                        throw new FailToUpdateException($"The drone: {id} could not pick up a parcel. \n");
                    Customer customerS = new();
                    customerS = GetCustomer(drone.ParcelInTransfer.Sender.Id);
                    //the distance between the drone to the customer that send the parcel
                    double dis = Distance.Haversine(droneList.DroneLocation.Longitude, droneList.DroneLocation.Latitude, customerS.CustomerLocation.Longitude, customerS.CustomerLocation.Latitude);
                    //if there is no such drone and that ther are no parcels that the drone is in the middle of delivering 
                    if (droneList.NumOfParcelOnTheWay != 0 && drone.ParcelInTransfer.StatusParcel == false)
                    {
                        //to reduse frome the battery that the drone lost because of the way
                        droneList.Battery -= (int)(dis * dal.AskForBattery()[(int)drone.ParcelInTransfer.Weight + 1]);
                        droneList.DroneLocation = drone.ParcelInTransfer.PickUpLocation;
                        dal.PickUpParcel(drone.ParcelInTransfer.Id);
                    }
                    else
                        throw new FailToUpdateException($"The drone: {id} could not pick up a parcel ");
                }
                catch (InvalidOperationException ex)
                {
                    throw new FailToUpdateException($"The Drone: {id} was not found, the dron: {id} could not pick up the parcel\n", ex);
                }
                catch (FailToUpdateException ex)
                {
                    throw new FailToUpdateException($"The drone: {id} could not pick up a parcel.\n", ex);
                }
                catch (NotFoundInputException ex)
                {
                    throw new FailToUpdateException($"The Drone: {id} was not found, the dron: {id} couldn't pick up the parcel\n", ex);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeliverParcel(int id)
        {
            lock (dal)
            {
                try
                {
                    Drone drone = GetDrone(id);
                    DroneList droneList = Drones.First(item => item.Id == id && item.IsActive);
                    if (drone.ParcelInTransfer == null)
                        throw new FailToUpdateException($"The drone: {id} could not deliever the parcel.\n");
                    Customer customerR = GetCustomer(drone.ParcelInTransfer.Recepter.Id);
                    //the distance between the drone and the customer that reseved the parcel
                    double dis = Distance.Haversine(droneList.DroneLocation.Longitude, droneList.DroneLocation.Latitude, customerR.CustomerLocation.Longitude, customerR.CustomerLocation.Latitude);
                    //if there are parcels that are on the way on the drone
                    if (drone.ParcelInTransfer.StatusParcel == true)
                    {
                        //reduse the battery that the drone used on the way to deliver the parcel
                        droneList.Battery -= (int)(dis * dal.AskForBattery()[(int)(drone.ParcelInTransfer.Weight) + 1]);
                        droneList.DroneLocation = drone.ParcelInTransfer.DelieveredLocation;
                        droneList.Status = DroneStatus.available;//the drone is avilable
                        dal.ParcelToCustomer(drone.ParcelInTransfer.Id);
                        droneList.NumOfParcelOnTheWay = 0;
                    }
                    else
                        throw new FailToUpdateException($"The drone: {id} could not deliever the parcel.\n");
                }
                catch (InvalidOperationException ex)
                {
                    throw new FailToUpdateException($"The Drone: {id} was not found, the dron: {id} could not deliever the parcel\n", ex);
                }
                catch (FailToUpdateException ex)
                {
                    throw new FailToUpdateException($"The drone: {id} could not deliever a parcel ", ex);
                }
                catch (NotFoundInputException ex)
                {
                    throw new FailToUpdateException($"The Drone: {id} was not found, the dron: {id} could not deliever the parcel\n", ex);
                }
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int idDrone)
        {
            lock (dal)
            {
                try
                {
                    Drone drone = GetDrone(idDrone);
                    int dronIndex = Drones.FindIndex(item => item.Id == drone.Id);
                    if (drone.Status == DroneStatus.inFix)
                    {
                        dal.DeleteDrone(idDrone);
                        Drones.RemoveAt(dronIndex);
                    }
                    else
                    {
                        dal.NotActiveDrone(idDrone);
                        Drones[dronIndex].IsActive = false;
                    }
                }
                catch (NotFoundInputException ex)
                {
                    throw new NotFoundInputException($"The drone: {idDrone} was not found.\n", ex);
                }
                catch (DO.ItemIsDeletedException ex)
                {
                    throw new ItemIsDeletedException($"Drone: { idDrone } is already deleted.");
                }
            }
        }

        #region INTERNAL FUNC

        /// <summary>
        /// calculate how much battery did the drone used in the delivery of parcel
        /// </summary>
        /// <param name="parcel">the parcel that the drone is delivering</param>
        /// <param name="drone">the drone that  we are calulating the battery for </param>
        /// <returns>the battery that will be left in the dron if he will do the delivery of parcel</returns>
        internal int UseOfBattery(int parcelId, int dronId)
        {
            lock (dal)
            {
                DO.Parcel parcel = dal.GetParcel(parcelId);
                Drone drone = GetDrone(dronId);
                Customer senderOfParcel = GetCustomer(parcel.SenderId);
                Customer resepterOfParcel = GetCustomer(parcel.TargetId);
                BaseStation baseStationToCharge = new();
                //the closest base station to the resever of the parcel
                baseStationToCharge = GetBaseStation(FindMinDistanceOfCToBS(resepterOfParcel.CustomerLocation.Latitude, resepterOfParcel.CustomerLocation.Longitude).Id);
                double disDroneToSenderP = DisDronToCustomer(drone.Id, senderOfParcel);
                double disReseverToBS = DisDronToBS(resepterOfParcel, baseStationToCharge);
                double disSenderToResepter = DisSenderToResever(senderOfParcel, resepterOfParcel);
                //culcilates how much batery will leght in the drone after he get to the parcel to delever the parcel and if he needs to get also to a base station 
                int battery = drone.Battery - ((int)(disDroneToSenderP * dal.AskForBattery()[(int)parcel.Weight + 1]) + (int)(disSenderToResepter * dal.AskForBattery()[(int)parcel.Weight + 1]) + (int)(disReseverToBS * dal.AskForBattery()[(int)parcel.Weight + 1]));
                return battery;
            }
        }

        /// <summary>
        /// fineds the closest base station to drone
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        internal DO.BaseStation FindMinDistanceOfDToBS(double longitude, double latitude)
        {
            lock (dal)
            {
                DO.BaseStation baseStation = new();
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
        }
        /// <summary>
        /// fineds the closest station to drone that has empty charger
        /// </summary>
        /// <param name="drone">the drone that we want to find the station for</param>
        /// <returns>the closest base station to drone that has an empty charger</returns>
        internal DO.BaseStation FindMinDistanceOfDToBSWithEempChar(DroneList drone)
        {
            lock (dal)
            {
                DO.BaseStation baseStation = new();
                bool foundParcel = false;
                double minDistance = 0;
                double distance = 0;
                //to fo over all the station
                foreach (var item in dal.GetBaseStations(item => item.EmptyCharges != 0))
                {
                    //the distance between the drone and this station
                    distance = Distance.Haversine(item.Latitude, item.Longitude, drone.DroneLocation.Latitude, drone.DroneLocation.Longitude);
                    //check if the distance of this station is less then the one before
                    if (minDistance == 0 || minDistance > distance)
                    {
                        minDistance = distance;
                        baseStation = item;
                        foundParcel = true;
                    }
                }
                //meens there are no station that has empty chargers
                if (foundParcel == false)
                    throw new FailedToChargeDroneException();
                return baseStation;
            }
        }
        #endregion

        #region PRIVATE FUNC


        /// <summary>
        /// returne the distance between a drone and a customer
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        private double DisDronToCustomer(int droneId, Customer customer)
        {
            Drone drone=GetDrone(droneId);
            lock (dal)
                return Distance.Haversine(drone.DroneLocation.Longitude, drone.DroneLocation.Latitude, customer.CustomerLocation.Longitude, customer.CustomerLocation.Latitude);
        }
        /// <summary>
        /// returne the distance between a drone and a base station
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        private double DisDronToBS(Customer customer, BaseStation station)
        {
            lock (dal)
                return Distance.Haversine(customer.CustomerLocation.Longitude, customer.CustomerLocation.Latitude, station.BaseStationLocation.Longitude, station.BaseStationLocation.Latitude);
        }
        #endregion
    }
}
