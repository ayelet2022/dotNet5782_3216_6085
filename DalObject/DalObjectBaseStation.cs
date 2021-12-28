using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;

namespace Dal
{
    internal sealed partial class DalObject : IDal
    {
        public void AddBaseStation(BaseStation addBaseStation)
        {
            if (DataSource.Stations.Exists(item => item.Id == addBaseStation.Id))
                throw new ExistsException($"Base station id: {addBaseStation.Id} already exists.");
            DataSource.Stations.Add(addBaseStation);
        }
        public BaseStation GetBaseStation(int idBaseStation)
        {
            //search for the base station that has the same id has the id that the user enterd
            int baseStationIndex = DataSource.Stations.FindIndex(item => item.Id == idBaseStation);
            if (baseStationIndex == -1)
                throw new DoesNotExistException($"Base station id: {idBaseStation} does not exists.");
            return DataSource.Stations[baseStationIndex];
        }
        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> predicate = null)
        {
            return from item in DataSource.Stations
                   where predicate == null ? true : predicate(item)
                   select item;
        }
        private int emptyCharging(int charges, int id)
        {
            int sum = DataSource.DroneCharges.Where(item => item.StationId == id).Count();
            return charges - sum;
        }
        public void UpdateStation(int id, string newName, int charges)
        {
            int baseStationIndex = DataSource.Stations.FindIndex(item => item.Id == id);
            BaseStation baseStation = DataSource.Stations[baseStationIndex];
            if (newName != "\n")
                baseStation.Name = newName;
            baseStation.EmptyCharges = emptyCharging(charges, id);
            DataSource.Stations[baseStationIndex] = baseStation;//to update the station in the list of base stations
        }
    }
}
