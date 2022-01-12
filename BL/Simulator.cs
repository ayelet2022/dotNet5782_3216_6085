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

        private const double kmh = 3600;//כל קילומטר זה שנייה כי בשעה יש 3600 שניות

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
                        TimeSpan timeCharge = (TimeSpan)(DateTime.Now - bl.dal.GetDroneCharge(droneID).StartCharging);
                        double hoursnInCahrge = timeCharge.Hours + (((double)timeCharge.Minutes) / 60) + (((double)timeCharge.Seconds) / 3600);
                        double batrryCharge = (int)(timeCharge.TotalHours * bl.power[4]) + drone.Battery; //DroneLoadingRate == 10000

                        while (drone.Battery < 100)
                        {
                            if (drone.Battery + 10 > 100)
                            {
                                bl.GetDrones().First(x => x.Id == droneID).Battery = 100;
                            }
                            else
                            {
                                bl.GetDrones().First(x => x.Id == droneID).Battery += 10;
                            }
                            ReportProgressInSimultor();
                            Thread.Sleep(1500);
                        }
                        bl.FreeDroneFromeCharger(droneID);
                        ReportProgressInSimultor();
                        break;
                    case DroneStatus.delivery:
                        Drone _drone = bl.GetDrone(droneID);
                        if (bl.GetParcel(_drone.ParcelInTransfer.Id).PickedUp == null)
                        {
                            battery = drone.Battery;
                            Location location = new Location { Longitude = drone.DroneLocation.Longitude, Latitude = drone.DroneLocation.Latitude };
                            distance = Distance.Haversine
                                (_drone.DroneLocation.Longitude, _drone.DroneLocation.Latitude, _drone.ParcelInTransfer.PickUpLocation.Longitude, _drone.ParcelInTransfer.PickUpLocation.Latitude);
                            double latitude = Math.Abs((bl.GetCustomer(_drone.ParcelInTransfer.Sender.Id).CustomerLocation.Latitude - drone.DroneLocation.Latitude) / distance);
                            double longitude = Math.Abs((bl.GetCustomer(_drone.ParcelInTransfer.Sender.Id).CustomerLocation.Longitude - drone.DroneLocation.Longitude) / distance);
                            while (distance > 1)
                            {
                                drone.Battery -= (int)bl.power[0];//the drone is available
                                distance -= 1;
                                UpdateLocationDrone(_drone.DroneLocation, bl.GetCustomer(_drone.ParcelInTransfer.Sender.Id).CustomerLocation,_drone,longitude,latitude);
                                drone.DroneLocation = _drone.DroneLocation;
                                bl.GetDrones().First(item => item.Id == drone.Id).DroneLocation = drone.DroneLocation;
                                ReportProgressInSimultor();
                                Thread.Sleep(500);
                            }
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
                            double latitude = Math.Abs((bl.GetCustomer(_drone.ParcelInTransfer.Sender.Id).CustomerLocation.Latitude - drone.DroneLocation.Latitude) / distance);
                            double longitude = Math.Abs((bl.GetCustomer(_drone.ParcelInTransfer.Sender.Id).CustomerLocation.Longitude - drone.DroneLocation.Longitude) / distance);
                            while (distance > 1)
                            {
                                switch (_drone.ParcelInTransfer.Weight)
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
                                UpdateLocationDrone(_drone.DroneLocation, bl.GetCustomer(_drone.ParcelInTransfer.Sender.Id).CustomerLocation, _drone, longitude, latitude);
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
        ///updates the location of the drone 
        /// </summary>
        /// <param name="targetLocation">where the drone is heading to</param>
        /// <param name="drone">the drone we want to update</param>
        private void UpdateLocationDrone(Location locationOfDrone, Location locationOfNextStep, Drone myDrone, double lon, double lat)
        {
            double droneLatitude = locationOfDrone.Latitude;
            double droneLongitude = locationOfDrone.Longitude;

            double nextStepLatitude = locationOfNextStep.Latitude;
            double nextStepLongitude = locationOfNextStep.Longitude;

            //Calculate the latitude of the new location.
            if (droneLatitude < nextStepLatitude)// ++++++
            {
                //double step = (nextStepLatitude - droneLatitude) / myDrone.Delivery.TransportDistance;
                //myDrone.CurrentLocation.latitude += (nextStepLatitude - droneLatitude) / myDrone.Delivery.TransportDistance;
                myDrone.DroneLocation.Latitude += lat;
            }
            else
            {
                //double step = (  droneLatitude - nextStepLatitude) / myDrone.Delivery.TransportDistance;
                //myDrone.CurrentLocation.latitude -= (droneLatitude - nextStepLatitude) / myDrone.Delivery.TransportDistance;
                myDrone.DroneLocation.Latitude -= lat;
            }

            //Calculate the Longitude of the new location.
            if (droneLongitude < nextStepLongitude)//+++++++
            {
                // double step = (nextStepLongitude - droneLongitude) / myDrone.Delivery.TransportDistance;
                //myDrone.CurrentLocation.longitude += (nextStepLongitude - droneLongitude) / myDrone.Delivery.TransportDistance;
                myDrone.DroneLocation.Longitude += lon;
            }
            else
            {
                //double step = (droneLongitude - nextStepLongitude) / myDrone.Delivery.TransportDistance;
                //myDrone.CurrentLocation.longitude -= (droneLongitude - nextStepLongitude) / myDrone.Delivery.TransportDistance;
                myDrone.DroneLocation.Longitude -= lon;
            }
        }
    }
}
