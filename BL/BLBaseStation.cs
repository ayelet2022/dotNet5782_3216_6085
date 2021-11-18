﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace BL
{
    public partial class BL
    {
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

        public BaseStation GetBaseStation(int idBaseStation)
        {
            IDAL.DO.BaseStation dalStation = dal.GetBaseStations().First(item => item.Id == idBaseStation);
            BaseStation station = new();
            station.CopyPropertiesTo(dalStation);
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

        /// <summary>
        /// copyes the values of al the base stations in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the base stations</returns>
        public IEnumerable<BaseStationList> GetBaseStations()
        {
            List<BaseStationList> listStations = new();
            List<BaseStation> blStations = new();
            IEnumerable<IDAL.DO.BaseStation> dalStations = dal.GetBaseStations();
            int i = 0;
            foreach (var item in dalStations)
            {
                blStations[i] = GetBaseStation(item.Id);
                listStations[i].CopyPropertiesTo(blStations[i]);
                listStations[i].FullChargingPositions = blStations[i].DronesInCharge.Count;
                i++;
            }
            return listStations;
        }

        /// <summary>
        /// search for tha base station that has available charges and copys them to a new arrey
        /// </summary>
        /// <returns>the new arrey that has all the base station that has free charges</returns>
        public IEnumerable<BaseStationList> GetBaseStationWithAvailableCharges()
        {
            List<BaseStationList> listStations = new();
            List<BaseStation> blStations = new();
            IEnumerable<IDAL.DO.BaseStation> dalStations = dal.GetBaseStations();
            int i = 0;
            foreach (var item in dalStations)
            {
                if (item.EmptyCharges != 0)
                {
                    blStations[i] = GetBaseStation(item.Id);
                    listStations[i].CopyPropertiesTo(blStations[i]);
                    listStations[i].FullChargingPositions = blStations[i].DronesInCharge.Count;
                    i++;
                }
            }
            return listStations;
        }
    }
}
