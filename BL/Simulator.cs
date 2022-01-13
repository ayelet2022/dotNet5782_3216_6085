using System;
using BO;
using System.Threading;
using static BL.BL;
using System.Linq;
using static System.Math;
using System.Collections.Generic;

namespace BL
{
    internal class Simulator
    {
        BL bl;

        public Simulator(BL _bl, int droneID, Action ReportProgressInSimultor, Func<bool> IsTimeRun)
        {
            bl = _bl;
            double distance;
            int battery;
            DroneList drone = bl.GetDrones().First(x => x.Id == droneID);
            while (!IsTimeRun())
            {
                switch (drone.Status)
                {
                    case DroneStatus.available:
                        try
                        {
                            bl.ScheduledAParcelToADrone(droneID);
                            ReportProgressInSimultor();
                        }
                        catch
                        {
                            if (drone.Battery < 100)
                            {
                                battery = drone.Battery;
                                DO.BaseStation baseStation = bl.FindMinDistanceOfDToBSWithEempChar(drone);
                                distance = Distance.Haversine(drone.DroneLocation.Longitude, drone.DroneLocation.Latitude, baseStation.Longitude, baseStation.Latitude);
                                while (distance > 0)
                                {
                                    drone.Battery -= (int)bl.power[0];//the drone is available
                                    ReportProgressInSimultor();
                                    distance -= 1;
                                    Thread.Sleep(1000);
                                }
                                drone.Battery = battery;//restarting the battery
                                bl.SendDroneToCharging(droneID);//here it will change it to the correct battery.
                                ReportProgressInSimultor();
                            }
                        }
                        break;
                    case DroneStatus.inFix:
                        while (drone.Battery < 100)//meens the battery isnt full
                        {
                            if (drone.Battery + (int)bl.power[4] > 100)//check if there aill be to much battery
                            {
                                bl.GetDrones().First(x => x.Id == droneID).Battery = 100;
                            }
                            else
                            {
                                bl.GetDrones().First(x => x.Id == droneID).Battery += (int)bl.power[4];
                            }
                            ReportProgressInSimultor();
                            Thread.Sleep(1000);
                        }
                        bl.FreeDroneFromeCharger(droneID);
                        ReportProgressInSimultor();
                        break;
                    case DroneStatus.delivery:
                        Drone _drone = bl.GetDrone(droneID);
                        if (bl.GetParcel(_drone.ParcelInTransfer.Id).PickedUp == null)//meens the drone needs to pick up the parcel
                        {
                            battery = drone.Battery;
                            Location location = new Location { Longitude = drone.DroneLocation.Longitude, Latitude = drone.DroneLocation.Latitude };
                            //the distance between the drone and the sender of the parcel
                            distance = Distance.Haversine
                                (_drone.DroneLocation.Longitude, _drone.DroneLocation.Latitude, _drone.ParcelInTransfer.PickUpLocation.Longitude, _drone.ParcelInTransfer.PickUpLocation.Latitude);
                            double latitude = Abs((bl.GetCustomer(_drone.ParcelInTransfer.Sender.Id).CustomerLocation.Latitude - drone.DroneLocation.Latitude) / distance);
                            double longitude = Abs((bl.GetCustomer(_drone.ParcelInTransfer.Sender.Id).CustomerLocation.Longitude - drone.DroneLocation.Longitude) / distance);
                            //meens the drone didnt get to sender yet
                            while (distance > 1)
                            {
                                drone.Battery -= (int)bl.power[0];//the drone is available
                                distance -= 1;
                                UpdateLocationDrone(_drone.DroneLocation, bl.GetCustomer(_drone.ParcelInTransfer.Sender.Id).CustomerLocation,_drone,longitude,latitude);
                                drone.DroneLocation = _drone.DroneLocation;
                                bl.GetDrones().First(item => item.Id == drone.Id).DroneLocation = drone.DroneLocation;
                                ReportProgressInSimultor();
                                Thread.Sleep(1000);
                            }
                            //meens the drone arraived to the sender
                            drone.DroneLocation = location;
                            drone.Battery = battery;
                            bl.PickUpParcel(_drone.Id);
                            ReportProgressInSimultor();
                        }
                        else // PickedUp != null
                        {
                            battery = drone.Battery;
                            distance = _drone.ParcelInTransfer.TransportDistance;//the distance betwwen the sender and the resever
                            Location d = new Location { Longitude = drone.DroneLocation.Longitude, Latitude = drone.DroneLocation.Latitude };
                            double latitude = Abs((bl.GetCustomer(_drone.ParcelInTransfer.Recepter.Id).CustomerLocation.Latitude - drone.DroneLocation.Latitude) / distance);
                            double longitude = Abs((bl.GetCustomer(_drone.ParcelInTransfer.Recepter.Id).CustomerLocation.Longitude - drone.DroneLocation.Longitude) / distance);
                            //meens the drone didnt get to recever yet
                            while (distance > 1)
                            {
                                switch (_drone.ParcelInTransfer.Weight)//according the parcels weight
                                {
                                    case WeightCategories.light:
                                        drone.Battery -= (int)bl.power[1];//light
                                        break;
                                    case WeightCategories.mediumWeight:
                                        drone.Battery -= (int)bl.power[2];//medium
                                        break;
                                    case WeightCategories.heavy:
                                        drone.Battery -= (int)bl.power[3];//heavy
                                        break;
                                    default:
                                        break;
                                }
                                UpdateLocationDrone(_drone.DroneLocation, bl.GetCustomer(_drone.ParcelInTransfer.Recepter.Id).CustomerLocation, _drone, longitude, latitude);
                                drone.DroneLocation = _drone.DroneLocation;
                                ReportProgressInSimultor();
                                distance -= 1;
                                Thread.Sleep(500);
                            }
                            drone.DroneLocation = d;
                            drone.Battery = battery;
                            bl.DeliverParcel(_drone.Id);
                            ReportProgressInSimultor();
                        }
                        break;
                    default:
                        break;
                }
                Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationOfDrone">the drones location</param>
        /// <param name="locationOfNextStep">the target location</param>
        /// <param name="myDrone">the drone that is in delivery</param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        private void UpdateLocationDrone(Location locationOfDrone, Location locationOfNextStep, Drone myDrone, double lon, double lat)
        {
            double droneLatitude = locationOfDrone.Latitude;
            double droneLongitude = locationOfDrone.Longitude;

            double targetLatitude = locationOfNextStep.Latitude;
            double targetLongitude = locationOfNextStep.Longitude;

            //Calculate the latitude of the new location.
            if (droneLatitude < targetLatitude)
                myDrone.DroneLocation.Latitude += lat;
            else
                myDrone.DroneLocation.Latitude -= lat;
            //Calculate the Longitude of the new location.
            if (droneLongitude < targetLongitude)
                myDrone.DroneLocation.Longitude += lon;
            else
                myDrone.DroneLocation.Longitude -= lon;
        }
    }
}
