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
        /// <summary>
        /// adds a new drone to the arrey
        /// </summary>
        public void AddDrone(Drone addDrone)
        {
            if (DataSource.Drones.Exists(item => item.Id == addDrone.Id))
                throw new ExistsException("Drone already exists.");
            DataSource.Drones.Add(addDrone);
        }

        /// <summary>
        ///  search for the drone in the arrey thet has the same id as the user enterd and returnes it
        /// </summary>
        /// <param name="idDrone">the id(that was enterd by the user in main) of the dron that the user wants to print</param>
        /// <returns>resturn the drone that needs to be printed</returns>
        public Drone GetDrone(int idDrone)
        {
            if (DataSource.Drones.Exists(item => item.Id != idDrone))
                throw new DoesNotExistException("Drone does not exist.");
            int droneIndex = 0;
            while (DataSource.Drones[droneIndex].Id != idDrone)//search for the drone that has the same id has the id that the user enterd
                droneIndex++;
            return DataSource.Drones[droneIndex];
        }

        /// <summary>
        /// update the drone that has the same id as the user enterd to charge in the base station that has the id that the user enterd
        /// </summary>
        /// <param name="dronesId">the drones id that the user enterd</param>
        /// <param name="idOfBaseStation">the base station id that the user enterd</param>
        public void DronToCharger(int dronesId, int idOfBaseStation)
        {
            if (DataSource.Drones.Exists(item => item.Id != dronesId))
                throw new DoesNotExistException("Drone does not exists.");
            if (DataSource.Stations.Exists(item => item.Id != idOfBaseStation))
                throw new DoesNotExistException("Base Station does not exists.");
            int droneIndex = 0;
            while (DataSource.Drones[droneIndex].Id != dronesId)//search for the drone that has the same id has the id that the user enterd
                droneIndex++;
            int stationsIndex = 0;
            while (DataSource.Stations[stationsIndex].Id != idOfBaseStation)//search for the base station that has the same id has the id that the user enterd
                stationsIndex++;                
            DroneCharge updateADrone = new();
            updateADrone.DroneId = dronesId;
            updateADrone.StationId = idOfBaseStation;
            DataSource.DroneCharges.Add(updateADrone);
            BaseStation updateAStation = DataSource.Stations[stationsIndex];
            updateAStation.EmptyCharges--;
            DataSource.Stations[stationsIndex] = updateAStation;
        }

        /// <summary>
        /// free the drone-that has the same id that the user enterd from the charger that he was cherging from in the base station
        /// </summary>
        /// <param name="dronesId">the drones id that the user enterd</param>
        public void FreeDroneFromBaseStation(int dronesId)
        {
            if (DataSource.Drones.Exists(item => item.Id != dronesId))
                throw new DoesNotExistException("Drone does not exists.");
            int droneLocation = 0;
            while (DataSource.Drones[droneLocation].Id != dronesId)//search for the drone that has the same id has the id that the user enterd
                droneLocation++;
            //DataSource.Drones[droneLocation].Battery = 100;
            //DataSource.Drones[droneLocation].Status = (DroneStatuses)0;//availabel
            int droneChargesIndex = 0;
            while (DataSource.DroneCharges[droneChargesIndex].DroneId != dronesId)//search for the drone charge that is chargine the drone that has the same id as the user enterd
                droneChargesIndex++;
            DataSource.DroneCharges.Remove(DataSource.DroneCharges[droneChargesIndex]);
            int stationLocation = 0;
            while (DataSource.Stations[stationLocation].Id != DataSource.DroneCharges[droneChargesIndex].StationId)//search for the base station that has the same id as the one in the chrger
                stationLocation++;
            BaseStation updateAStation = DataSource.Stations[stationLocation];
            updateAStation.EmptyCharges--;
            DataSource.Stations[stationLocation] = updateAStation;
        }

        /// <summary>
        /// copyes the values of all the drones in order to print them
        /// </summary>
        /// <returns>the new arrey that has the the drones</returns>
        public IEnumerable<Drone> GetDrones()
        {
            List<Drone> Drones = new List<Drone>();
            foreach (var itD in DataSource.Drones)
            {
                Drones.Add(itD);
            }
            return Drones;
        }

        public void UpdateDrone(int id, string newModel)
        {   
            int i = 0;
            Drone drone = new();
            foreach (var item in DataSource.Drones)
            {
                if (item.Id == id)
                {
                    drone = item;
                    drone.Model = newModel;
                    break;
                }
                i++;
            }
            DataSource.Drones[i] = drone;
        }

        //public void UpdateDronToInDelevery(int droneId)
        //{
        //    int i = 0;
        //    Drone drone = new();
        //    foreach (var item in DataSource.Drones)
        //    {
        //        if (item.Id == droneId)
        //        {
        //            drone = item;
        //            drone.Model = item.Model;
        //            break;
        //        }
        //        i++;
        //    }
        //    DataSource.Drones[i] = drone;
        //}
    }
}
