﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BL
{
    public partial class BL
    {
        //public delegate bool conditionDelegate(IDAL.DO.BaseStation baseStation);

        public void AddBaseStation(BaseStation baseStation)
        {
            if (baseStation.Id > 9999 || baseStation.Id < 1000)
                throw new InvalidInputException($"The baseStations id: {baseStation.Id} is incorrect, the base station was not added.\n");
            if(baseStation.Name == "")
                throw new InvalidInputException($"The baseStations name: {baseStation.Name} is incorrect, the base station was not added.\n");
            if (baseStation.BaseStationLocation.Latitude < 30 || baseStation.BaseStationLocation.Latitude > 33)
                throw new InvalidInputException($"The baseStations Latitude: {baseStation.BaseStationLocation.Latitude} is incorrect, the base station was not added.\n");
            if (baseStation.BaseStationLocation.Longitude < 33 || baseStation.BaseStationLocation.Longitude > 37)
                throw new InvalidInputException($"The baseStations Longitude: {baseStation.BaseStationLocation.Longitude} is incorrect, the base station was not added.\n");
            if (baseStation.EmptyCharges < 0)
                 throw new InvalidInputException($"The number of empty charges: {baseStation.EmptyCharges} is incorrect, the base station was not added.\n");
            try
            {
                baseStation.DronesInCharge = null;
                DO.BaseStation station = new();
                object obj = station;
                baseStation.CopyPropertiesTo(obj);
                station = (DO.BaseStation)obj;
                station.Longitude = baseStation.BaseStationLocation.Longitude;
                station.Latitude = baseStation.BaseStationLocation.Latitude;
                dal.AddBaseStation(station);
            }
            catch (DO.ExistsException ex)
            {
                throw new FailedToAddException($"Base station id: {baseStation.Id} already exists, the base station was not added\n.", ex);
            }
        }

        public BaseStation GetBaseStation(int idBaseStation)
        {
            try
            {
                //search for the first base station in the list that has the same id
                DO.BaseStation dalStation = dal.GetBaseStation(idBaseStation);
                BaseStation station = new BaseStation();
                dalStation.CopyPropertiesTo(station);
                DroneInCharge droneInCharge = new();
                station.BaseStationLocation = new();
                station.BaseStationLocation.Latitude = dalStation.Latitude;
                station.BaseStationLocation.Longitude = dalStation.Longitude;
                //to go over all the drones that are in charger
                foreach (var item in dal.GetDroneCharges(x => x.StationId == idBaseStation))
                {
                    Drone drone = GetDrone(item.DroneId);
                    droneInCharge.Id = item.DroneId;
                    droneInCharge.Battery = drone.Battery;
                    station.DronesInCharge.Append(droneInCharge);
                    droneInCharge = new();
                }
                return station;
            }
            catch (DO.DoesNotExistException ex)
            {
                throw new NotFoundInputException($"The base station: {idBaseStation} was not found.\n", ex);
            }
        }

        public IEnumerable<BaseStationList> GetBaseStations(Predicate<BaseStationList> predicate = null)
        {
            List<BaseStationList> listStations = new List<BaseStationList>();
            BaseStationList baseStationList = new();
            BaseStation blStations = new();
            //to go over all of the base station list and copy only the onece that have avilable chargers
            foreach (var item in dal.GetBaseStations())
            {
                blStations = GetBaseStation(item.Id);
                blStations.CopyPropertiesTo(baseStationList);
                //meens there are no drones charging in the station
                if (blStations.DronesInCharge == null)
                    baseStationList.FullChargingPositions = 0;
                else//meens thare are drones that charging
                    baseStationList.FullChargingPositions = blStations.DronesInCharge.Count();
                listStations.Add(baseStationList);
                baseStationList = new();
            }
            return listStations.FindAll(item => predicate == null ? true : predicate(item));
        }

        public void UpdateStation(int id, string newName,int charges)
        {
            try
            {
                dal.GetBaseStation(id);
                //checks if the input of chargers in in the limit
                if (charges < 0)
                    throw new InvalidInputException();
                //checks if ther is such a station
                dal.UpdateStation(id, newName, charges);
            }
            //meens there is no station with such an id
            catch (InvalidInputException ex)
            {
                throw new InvalidInputException($"The number of empty charges: {charges} is incorrect, the base station: {id} was not updated.\n", ex);
            }
            catch(DO.DoesNotExistException ex)
            {
                throw new InvalidInputException($"The base station: {id} was not found, the base station was not updated.\n", ex);
            }
        }

    }
}