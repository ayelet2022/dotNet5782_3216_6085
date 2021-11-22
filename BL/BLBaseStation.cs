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
            try
            {
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
                //fineds the first base station that has the same id has idbasestation
                IDAL.DO.BaseStation dalStation = dal.GetBaseStations().First(item => item.Id == idBaseStation);
                BaseStation station = new();
                dalStation.CopyPropertiesTo(station);
                DroneInCharge droneInCharge = new();
                //station.BaseStationLocation.Latitude = dalStation.Latitude;
                //station.BaseStationLocation.Longitude = dalStation.Longitude;
                foreach (var item in dal.GetDroneCharge())
                {
                    droneInCharge.Id = item.DroneId;
                    Drone drone = GetDrone(item.DroneId);
                    droneInCharge.Battery = drone.Battery;
                    station.DronesInCharge.Add(droneInCharge);
                }
                return station;
            }
            catch (InvalidOperationException ex)
            {
                throw new NotFoundInputException($"A base station with the id:{idBaseStation} was not found", ex);
            }
        }
        /// <summary>
        /// returens the all base station list
        /// </summary>
        /// <returns>the all base station list</returns>
        public IEnumerable<BaseStationList> GetBaseStations()
        {
            List<BaseStationList> listStations = new();
            BaseStationList baseStationList = new();
            BaseStation blStations;
            //to copy all the base station in the list to the new base station list
            foreach (var item in dal.GetBaseStations())
            {
                blStations = GetBaseStation(item.Id);
                blStations.CopyPropertiesTo(baseStationList);
                baseStationList.FullChargingPositions = blStations.DronesInCharge.Count;
                listStations.Add(baseStationList);
            }
            return listStations;
        }

        /// <summary>
        /// search for tha base station that has available charges and copys them to a new list
        /// </summary>
        /// <returns>the new list that has all the base station that has free charges</returns>
        public IEnumerable<BaseStationList> GetBaseStationWithAvailableCharges()
        {
            List<BaseStationList> listStations = new();
            BaseStationList baseStationList = new();
            BaseStation blStations = new();
            //to copy all the base station in the list to the new base station list
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

        /// <summary>
        /// update the base station that has the id that was enterd
        /// </summary>
        /// <param name="id">the id of the base station that we want to update</param>
        /// <param name="newName">the new name to the base station </param>
        /// <param name="emptyCharges">the new emount of empty chargers in the base station</param>
        public void UpdateStation(int id, string newName,int charges)
        {
            if (charges < 0)
                throw new InvalidInputException("The number of empty charges is incorrect");
            try
            {
                dal.GetBaseStations().First(item => item.Id == id);
                //calculate how many full chargers ther are int th base station and reduse it from the emount of all the chargers in the base station
                int emptyCharges = charges - GetBaseStation(id).DronesInCharge.Count;
                dal.UpdateStation(id, newName, emptyCharges);
            }
            catch (InvalidOperationException ex)
            {
                throw new FailToUpdateException($"Couldn't update a base station with the id:{id} because it was not found", ex);
            }
        }
    }
}
