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
        /// adds a new base station to the list
        /// </summary>
        /// <param name="baseStation">the new base station that we want to add</param>
        public void AddBaseStation(BaseStation baseStation)
        {
            try
            {
                if (baseStation.Id > 9999 || baseStation.Id < 1000)
                    throw new InvalidInputException($"The baseStations id:{baseStation.Id} is incorrect.\n");
                if(baseStation.Name == "\n")
                    throw new InvalidInputException($"The baseStations name:{baseStation.Name} is incorrect.\n");
                if (baseStation.BaseStationLocation.Latitude < 30 || baseStation.BaseStationLocation.Latitude > 33)
                    throw new InvalidInputException($"The baseStations Latitude:{baseStation.BaseStationLocation.Latitude} is incorrect.\n");
                if (baseStation.BaseStationLocation.Longitude < 33 || baseStation.BaseStationLocation.Longitude > 37)
                    throw new InvalidInputException($"The baseStations Longitude:{baseStation.BaseStationLocation.Longitude} is incorrect.\n");
                if (baseStation.EmptyCharges < 0)
                    throw new InvalidInputException($"The number of empty charges is incorrect.\n");
                baseStation.DronesInCharge = null;
                IDAL.DO.BaseStation station = new();
                object obj = station;
                baseStation.CopyPropertiesTo(obj);
                station = (IDAL.DO.BaseStation)obj;
                station.Longitude = baseStation.BaseStationLocation.Longitude;
                station.Latitude = baseStation.BaseStationLocation.Latitude;
                dal.AddBaseStation(station);
            }
            catch (IDAL.DO.ExistsException ex)
            {
                throw new FailedToAddException(ex.ToString(), ex);
            }
        }

        /// <summary>
        /// returens the besa station that has the id that was enterd 
        /// </summary>
        /// <param name="idBaseStation">the id of the base station that we want to retirne</param>
        /// <returns>the base station that has the same id</returns>
        public BaseStation GetBaseStation(int idBaseStation)
        {
            try
            {
                //search for the first base station in the list that has the same id
                IDAL.DO.BaseStation dalStation = dal.GetBaseStations().First(item => item.Id == idBaseStation);
                BaseStation station = new BaseStation();
                dalStation.CopyPropertiesTo(station);
                station.DronesInCharge = new();
                DroneInCharge droneInCharge = new();
                station.BaseStationLocation = new();
                station.BaseStationLocation.Latitude = dalStation.Latitude;
                station.BaseStationLocation.Longitude = dalStation.Longitude;
                //to go over all the drones that are in charger
                foreach (var item in dal.GetDroneCharge())
                {
                    Drone drone = GetDrone(item.DroneId);
                    droneInCharge.Id = item.DroneId;
                    droneInCharge.Battery = drone.Battery;
                    station.DronesInCharge.Add(droneInCharge);
                    droneInCharge = new();
                }
                return station;
            }
            catch (NotFoundInputException ex)
            {
                throw new FailedToPickUpParcelException("couldn't pick up the parcel.\n", ex);
            }
            catch (IDAL.DO.DoesNotExistException ex)
            {
                throw new FailedToPickUpParcelException("couldn't pick up the parcel.\n", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidInputException("The input does not exist.\n", ex);
            }
        }

        /// <summary>
        /// returens the all base station list
        /// </summary>
        /// <returns>the all base station list</returns>
        public IEnumerable<BaseStationList> GetBaseStations()
        {
            try
            {
                List<BaseStationList> listStations = new List<BaseStationList>();
                BaseStationList baseStationList = new();
                BaseStation blStations = new();
                //to copy all the station from the station list
                foreach (var item in dal.GetBaseStations())
                {
                    blStations = GetBaseStation(item.Id);
                    blStations.CopyPropertiesTo(baseStationList);
                    //meens ther are no drones that are charging in the base station
                    if (blStations.DronesInCharge == null)
                        baseStationList.FullChargingPositions = 0;
                    else//meens thar are drones that are charging in the base station
                        baseStationList.FullChargingPositions = blStations.DronesInCharge.Count;
                    listStations.Add(baseStationList);
                    baseStationList = new();
                }
                return listStations;
            }
            catch (NotFoundInputException ex)
            {
                throw new FailedToPickUpParcelException("couldn't pick up the parcel.\n", ex);
            }
        }

        /// <summary>
        /// search for tha base station that has available charges and copys them to a new list
        /// </summary>
        /// <returns>the new list that has all the base station that has free charges</returns>
        public IEnumerable<BaseStationList> GetBaseStationWithAvailableCharges()
        {
            try
            {
                List<BaseStationList> listStations = new();
                BaseStationList baseStationList = new();
                BaseStation blStations = new();
                //to go over all of the base station list and copy only the onece that have avilable chargers
                foreach (var item in dal.GetBaseStations())
                {
                    //meens there are empty chargers
                    if (item.EmptyCharges != 0)
                    {
                        blStations = GetBaseStation(item.Id);
                        blStations.CopyPropertiesTo(baseStationList);
                        //meens there are no drones charging in the station
                        if (blStations.DronesInCharge == null)
                            baseStationList.FullChargingPositions = 0;
                        else//meens thare are drones that charging
                            baseStationList.FullChargingPositions = blStations.DronesInCharge.Count;
                        listStations.Add(baseStationList);
                        baseStationList = new();
                    }
                }
                return listStations;
            }
            catch (NotFoundInputException ex)
            {
                throw new FailedToPickUpParcelException("couldn't pick up the parcel", ex);
            }
        }

        /// <summary>
        /// update the base station that has the id that was enterd
        /// </summary>
        /// <param name="id">the id of the base station that we want to update</param>
        /// <param name="newName">the new name to the base station </param>
        /// <param name="emptyCharges">the new emount of empty chargers in the base station</param>
        public void UpdateStation(int id, string newName,int charges)
        {
            try
            {
                //checks if the input of chargers in in the limit
                if (charges < 0)
                    throw new InvalidInputException("The number of empty charges is incorrect.\n");
                //checks if ther is such a station
                dal.GetBaseStations().First(item => item.Id == id);
                dal.UpdateStation(id, newName, charges);
            }
            //meens there is no station with such an id
            catch (InvalidOperationException ex)
            {
                throw new InvalidInputException("The input does not exist.\n", ex);
            }
        }
    }
}
