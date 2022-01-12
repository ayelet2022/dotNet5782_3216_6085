using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using DO;
using DalApi;

namespace Dal
{
    internal sealed partial class DalObject : IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddBaseStation(BaseStation addBaseStation)
        {

            if (DataSource.Stations.Exists(item => item.Id == addBaseStation.Id && item.IsActive))
                throw new ExistsException($"Base station id: {addBaseStation.Id} already exists.");
            addBaseStation.IsActive = true;
            DataSource.Stations.Add(addBaseStation);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public BaseStation GetBaseStation(int idBaseStation)
        {
            try
            {
                //search for the base station that has the same id has the id that the user enterd
                return DataSource.Stations.First(item => item.Id == idBaseStation && item.IsActive);
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Base station id: {idBaseStation} does not exists.");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<BaseStation> GetBaseStations(Predicate<BaseStation> predicate = null)
        {
            return from item in DataSource.Stations
                   where (predicate == null ? true : predicate(item)) && item.IsActive
                   select item;
        }
        private int emptyCharging(int charges, int id)
        {
            int sum = DataSource.DroneCharges.Where(item => item.StationId == id).Count();
            return charges - sum;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(int id, string newName, int charges)
        {
            int baseStationIndex = DataSource.Stations.FindIndex(item => item.Id == id && item.IsActive);
            BaseStation baseStation = DataSource.Stations[baseStationIndex];
            if (newName != "\n")
                baseStation.Name = newName;
            baseStation.EmptyCharges = emptyCharging(charges, id);
            DataSource.Stations[baseStationIndex] = baseStation;//to update the station in the list of base stations
        }
        public void DeleteBaseStation(int id)
        {
            try
            {
                BaseStation baseStation = GetBaseStation(id);
                baseStation.IsActive = false;
                int stationIndex = DataSource.Stations.FindIndex(item => item.Id == id);
                DataSource.Stations[stationIndex] = baseStation;
            }
            catch (DoesNotExistException ex)
            {
                throw new ItemIsDeletedException($"Station: { id } is already deleted.");
            }
        }
    }
}
