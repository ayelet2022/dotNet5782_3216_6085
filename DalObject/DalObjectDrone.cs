using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DO;
using DalApi;

namespace Dal
{
    internal sealed partial class DalObject : IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone addDrone)
        {
            if (DataSource.Drones.Exists(item => item.Id == addDrone.Id && item.IsActive))
                throw new ExistsException($"Drone id: {addDrone.Id} already exists.");
            addDrone.IsActive = true;
            DataSource.Drones.Add(addDrone);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int idDrone)
        {
            try
            {
                //search for the drone that has the same id has the id that the user enterd
                return DataSource.Drones.First(item => item.Id == idDrone && item.IsActive);
            }
            catch (InvalidOperationException ex)
            {
                throw new DoesNotExistException($"Drone id: {idDrone} does not exist.");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DronToCharger(int dronesId, int idOfBaseStation)
        {
            //search for the drone that has the same id has the id that the user enterd
            int droneIndex = DataSource.Drones.FindIndex(item => item.Id == dronesId && item.IsActive);
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {dronesId} does not exist.");
            //search for the base station that has the same id has the id that the user enterd
            int baseStationIndex = DataSource.Stations.FindIndex(item => item.Id == idOfBaseStation);
            if (baseStationIndex == -1)
                throw new DoesNotExistException($"Base station id: {idOfBaseStation} does not exists.");
            DroneCharge droneCharge = new();
            droneCharge.StartCharging = DateTime.Now;
            droneCharge.DroneId = dronesId;
            droneCharge.StationId = idOfBaseStation;
            AddDroneCharge(droneCharge);
            BaseStation updateAStation = DataSource.Stations[baseStationIndex];
            updateAStation.EmptyCharges--;
            DataSource.Stations[baseStationIndex] = updateAStation;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void FreeDroneFromBaseStation(int dronesId)
        {
            //search for the drone that has the same id has the id that the user enterd
            int droneIndex = DataSource.Drones.FindIndex(item => item.Id == dronesId && item.IsActive);
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones(Predicate<Drone> predicate = null)
        {
            return from item in DataSource.Drones
                   where predicate == null ? true : predicate(item) && item.IsActive
                   select item;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int id, string newModel)
        {
            int droneIndex = DataSource.Drones.FindIndex(item => item.Id == id && item.IsActive);
            //checks if the drone was found
            if (droneIndex == -1)
                throw new DoesNotExistException($"Drone id: {id} does not exist.");
            Drone drone = DataSource.Drones[droneIndex];
            drone.Model = newModel;
            DataSource.Drones[droneIndex] = drone;//to change the drone in the drone list
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int id)
        {
            try
            {
                Drone drone = GetDrone(id);
                DataSource.Drones.Remove(drone);
                DeleteDroneCharge(id);
            }
            catch (DoesNotExistException ex)
            {
                throw new ItemIsDeletedException($"Drone: { id } is already deleted.");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void NotActiveDrone(int id)
        {
            try
            {
                Drone drone = GetDrone(id);
                drone.IsActive = false;
                int parcelIndex = DataSource.Drones.FindIndex(item => item.Id == id);
                DataSource.Drones[parcelIndex] = drone;
            }
            catch (DoesNotExistException ex)
            {
                throw new ItemIsDeletedException($"Drone: { id } is already deleted.");
            }
        }
    }
}
