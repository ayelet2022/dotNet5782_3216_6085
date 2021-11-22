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
                    throw new InvalidInputException($"The baseStations id:{baseStation.Id} is incorrect");
                if(baseStation.Name == "\n")
                    throw new InvalidInputException($"The baseStations name:{baseStation.Name} is incorrect");
                if (baseStation.BaseStationLocation.Latitude < -90 || baseStation.BaseStationLocation.Latitude > 90)
                    throw new InvalidInputException($"The baseStations Latitude:{baseStation.BaseStationLocation.Latitude} is incorrect");
                if (baseStation.BaseStationLocation.Longitude < -180 || baseStation.BaseStationLocation.Longitude > 180)
                    throw new InvalidInputException($"The baseStations Longitude:{baseStation.BaseStationLocation.Longitude} is incorrect");
                if (baseStation.EmptyCharges < 0)
                    throw new InvalidInputException($"The number of empty charges is incorrect");
                baseStation.DronesInCharge = null;
                IDAL.DO.BaseStation station = new();
                baseStation.CopyPropertiesTo(station);
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
                IDAL.DO.BaseStation dalStation = dal.GetBaseStations().First(item => item.Id == idBaseStation);
                BaseStation station = new();
                dalStation.CopyPropertiesTo(station);
                //station.BaseStationLocation.Latitude = dalStation.Latitude;
                //station.BaseStationLocation.Longitude = dalStation.Longitude;
                int i = 0;
                foreach (var item in dal.GetDroneCharge())
                {
                    station.DronesInCharge[i].Id = item.DroneId;
                    Drone drone = GetDrone(item.DroneId);
                    station.DronesInCharge[i++].Battery = drone.Battery;
                }
                return station;
            }
            catch (NotFoundInputException ex)
            {
                throw new FailedToPickUpParcelException("couldn't pick up the parcel", ex);
            }
            catch (IDAL.DO.DoesNotExistException ex)
            {
                throw new FailedToPickUpParcelException("couldn't pick up the parcel", ex);
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
                List<BaseStationList> listStations = new();
                BaseStationList baseStationList = new();
                BaseStation blStations = new();
                foreach (var item in dal.GetBaseStations())
                {
                    blStations = GetBaseStation(item.Id);
                    blStations.CopyPropertiesTo(baseStationList);
                    baseStationList.FullChargingPositions = blStations.DronesInCharge.Count;
                    listStations.Add(baseStationList);
                }
                return listStations;
            }
            catch (NotFoundInputException ex)
            {
                throw new FailedToPickUpParcelException("couldn't pick up the parcel", ex);
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
                foreach (var item in dal.GetBaseStations())
                {
                    if (item.EmptyCharges != 0)
                    {
                        blStations = GetBaseStation(item.Id);
                        blStations.CopyPropertiesTo(baseStationList);
                        baseStationList.FullChargingPositions = blStations.DronesInCharge.Count;
                        listStations.Add(baseStationList);
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
                if (charges < 0)
                    throw new InvalidInputException("The number of empty charges is incorrect");
                dal.GetBaseStations().First(item => item.Id == id);
                dal.UpdateStation(id, newName, charges);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidInputException("The input does not exist.\n", ex);
            }
        }

    }
}
