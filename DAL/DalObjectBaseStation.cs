﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    public partial class DalObject
    {
        /// <summary>
        /// adds a new base station to the arrey
        /// </summary>
        public void AddBaseStation(BaseStation addBaseStation)
        {
            if (!DataSource.Stations.Exists(item => item.Id == addBaseStation.Id))
                throw new ExistsException("Base station already exists.");
            DataSource.Stations.Add(addBaseStation);
        }

        /// <summary>
        ///  search for the base station in the arrey thet has the same id that the user enterd and returnes it
        /// </summary>
        /// <param name="idBaseStation"><the id(that was enterd by the user in main) of the basestation that the user wants to print/param>
        /// <returns>resturn the base station that needs to be printed</returns>
        public BaseStation GetBaseStation(int idBaseStation)
        {
            if (DataSource.Stations.Exists(item => item.Id != idBaseStation))
                throw new DoesNotExistException("Base station does not exists.");
            int baseStationIndex = 0;
            while (DataSource.Stations[baseStationIndex].Id != idBaseStation)//search for the base station that has the same id has the id that the user enterd
                baseStationIndex++;
            return DataSource.Stations[baseStationIndex];

        }

        /// <summary>
        /// copyes the values of al the base stations in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the base stations</returns>
        public IEnumerable<BaseStation> GetBaseStations()
        {
            List<BaseStation> Stations = new List<BaseStation>();
            foreach (var itBS in DataSource.Stations)
            {
                    Stations.Add(itBS);
            }
            return Stations;
        }

        /// <summary>
        /// search for tha base station that has available charges and copys them to a new arrey
        /// </summary>
        /// <returns>the new arrey that has all the base station that has free charges</returns>
        public IEnumerable<BaseStation> GetBaseStationWithAvailableCharges()
        {
            List<BaseStation> Stations = new();
            foreach (var itBS in DataSource.Stations)
            {
                if (itBS.EmptyCharges > 0)
                    Stations.Add(itBS);
            }
            return Stations;
        }

        public void UpdateStation(int id, string newName, int emptyCharges)
        {
            BaseStation baseStation = new();
            int index = 0;
            foreach (var item in DataSource.Stations)
            {
                if (item.Id == id)
                {
                    if (newName != "\n")
                        baseStation.Name = newName;
                    baseStation.EmptyCharges = emptyCharges;
                    break;
                }
                index++;
            }
            DataSource.Stations[index] = baseStation;
        }
    }
}
