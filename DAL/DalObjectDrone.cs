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
        public void AddDrone(Drone addDrone)
        {
            if (DataSource.Drones.Exists(item => item.Id == addDrone.Id))
                throw new ExistsException($"Drone id: {addDrone.Id} already exists.");
            DataSource.Drones.Add(addDrone);
        }
        public Drone GetDrone(int idDrone)
        {
            //search for the drone that has the same id has the id that the user enterd
            int droneIndex = DataSource.Drones.FindIndex(item => item.Id == idDrone);
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {idDrone} does not exist.");
            return DataSource.Drones[droneIndex];
        }
        public void DronToCharger(int dronesId, int idOfBaseStation)
        {
            //search for the drone that has the same id has the id that the user enterd
            int droneIndex = DataSource.Drones.FindIndex(item => item.Id == dronesId);
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {dronesId} does not exist.");
            //search for the base station that has the same id has the id that the user enterd
            int baseStationIndex = DataSource.Stations.FindIndex(item => item.Id == idOfBaseStation);
            if (baseStationIndex == -1)
                throw new DoesNotExistException($"Base station id: {idOfBaseStation} does not exists.");               
            DroneCharge updateADrone = new();
            updateADrone.DroneId = dronesId;
            updateADrone.StationId = idOfBaseStation;
            DataSource.DroneCharges.Add(updateADrone);
            BaseStation updateAStation = DataSource.Stations[baseStationIndex];
            updateAStation.EmptyCharges--;
            DataSource.Stations[baseStationIndex] = updateAStation;
        }
        public void FreeDroneFromBaseStation(int dronesId)
        {
            //search for the drone that has the same id has the id that the user enterd
            int droneIndex = DataSource.Drones.FindIndex(item => item.Id == dronesId);
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {dronesId} does not exist.");
            int droneChargesIndex = DataSource.DroneCharges.FindIndex(item => item.DroneId == dronesId);
            if (droneChargesIndex == -1)
                throw new NotFoundInputException($"Drone id: {dronesId} isn't charging.");
            int baseStationIndex = DataSource.Stations.FindIndex(item => item.Id == DataSource.DroneCharges[droneChargesIndex].StationId);
            if (baseStationIndex == -1)
                throw new DoesNotExistException($"Base station id: {baseStationIndex} does not exists.");
            BaseStation updateAStation = DataSource.Stations[baseStationIndex];
            updateAStation.EmptyCharges++;
            DataSource.Stations[baseStationIndex] = updateAStation;
            DataSource.DroneCharges.Remove(DataSource.DroneCharges[droneChargesIndex]);
        }
        public IEnumerable<Drone> GetDrones()
        {
            List<Drone> Drones = new List<Drone>();
            //to copy all the drones from list
            foreach (var itD in DataSource.Drones)
                Drones.Add(itD);
            return Drones;
        }
        public void UpdateDrone(int id, string newModel)
        {   
            int droneIndex = DataSource.Drones.FindIndex(item => item.Id == id);
            //checks if the drone was found
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {id} does not exist.");
            Drone drone = DataSource.Drones[droneIndex];
            drone.Model = newModel;
            DataSource.Drones[droneIndex] = drone;//to change the drone in the drone list
        }
    }
}
