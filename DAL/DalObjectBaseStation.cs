using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;
namespace DalObject
{
    public partial class DalObject
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
        public IEnumerable<BaseStation> GetBaseStations()
        {
            List<BaseStation> Stations = new List<BaseStation>();
            foreach (var itBS in DataSource.Stations)
                    Stations.Add(itBS);
            return Stations;
        }
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
            int baseStationIndex = DataSource.Stations.FindIndex(item => item.Id == id);
            BaseStation baseStation = DataSource.Stations[baseStationIndex];
            if (newName != "\n")
                baseStation.Name = newName;
            baseStation.EmptyCharges = emptyCharges;
            DataSource.Stations[baseStationIndex] = baseStation;
        }
    }
}
