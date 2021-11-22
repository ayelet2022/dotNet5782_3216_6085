using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;

namespace BL
{
    public partial class BL : IBL.IBL
    {
        IDal dal = new DalObject.DalObject();
        List<DroneList> Drones = new List<DroneList>();
        public BL()
        {
            double[] power = dal.AskForBattery();
            double chargingRate = power[4];
            dal.GetDrones();
            InitializeDroneList(Drones);
        }
        public IDAL.DO.BaseStation FindMinDistanceCtoS(Customer customer)
        {
            IDAL.DO.BaseStation baseStation = new();
            double minDistance = 0;
            double distance = 0;
            foreach (var item in dal.GetBaseStations())
            {
                distance = Distance.Haversine(item.Latitude, item.Longitude, customer.CustomerLocation.Latitude, customer.CustomerLocation.Longitude);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    baseStation = item;
                }
            }
            return baseStation;
        }
        void InitializeDroneList(List<DroneList> drones)
        {
            Random Rand = new Random(DateTime.Now.Millisecond);
            DroneList drone = new();
            foreach (var itD in dal.GetDrones())
            {
                itD.CopyPropertiesTo(drone);
                try
                {
                    IDAL.DO.Parcel parcel = dal.GetParcels().First(item => item.DroneId == itD.Id && item.Delivered == DateTime.MinValue); //a parcel was scheduled
                    drone.Status = (DroneStatus)2;//the drone in deliver
                    double customerSLatitude = dal.GetCustomer(parcel.SenderId).Latitude;
                    double customerSLongitude = dal.GetCustomer(parcel.SenderId).Longitude;
                    double customerRLatitude = dal.GetCustomer(parcel.TargetId).Latitude;
                    double customerRLongitude = dal.GetCustomer(parcel.TargetId).Longitude;
                    double minBattery = 0;
                    double disStoR = Distance.Haversine(customerSLongitude, customerSLatitude, customerRLongitude, customerRLatitude);
                    double disRtoBS = Distance.Haversine(customerRLongitude, customerRLatitude, FindMinDistanceCtoS(customerR).Longitude, FindMinDistanceCtoS(customerR).Latitude);
                    if (parcel.PickedUp == DateTime.MinValue)
                    {
                        drone.DroneLocation.Latitude = FindMinDistanceOfCToBS(customerS).Latitude;
                        drone.DroneLocation.Longitude = FindMinDistanceCtoS(customerS).Longitude;
                        double disDtoS = Distance.Haversine(drone.DroneLocation.Longitude, drone.DroneLocation.Latitude, customerSLongitude, customerSLatitude);
                        minBattery = (disDtoS + disRtoBS) * dal.AskForBattery()[0] + disStoR * dal.AskForBattery()[(int)(parcel.Weight) + 1];
                    }
                    else
                    {
                        drone.DroneLocation.Latitude = customerSLatitude;
                        drone.DroneLocation.Longitude = customerSLatitude;
                        minBattery = disRtoBS * dal.AskForBattery()[0] + disStoR * dal.AskForBattery()[(int)(parcel.Weight) + 1];
                    }
                    drone.Battery = Rand.Next((int)minBattery, 101);
                    drones.Add(drone);
                }
                catch (InvalidOperationException)
                {
                    drone.Status = (DroneStatus)Rand.Next(0, 2);
                    if (drone.Status == (DroneStatus)1)
                    {
                        List<IDAL.DO.BaseStation> baseStationL = dal.GetBaseStations().ToList();
                        int stationI = Rand.Next(0, baseStationL.Count);
                        drone.DroneLocation = new();
                        drone.DroneLocation.Longitude = baseStationL[stationI].Longitude;
                        drone.DroneLocation.Latitude = baseStationL[stationI].Latitude;
                        drone.Battery = Rand.Next(0, 21);
                    }
                    else
                    {
                        List<int> customerId = dal.GetCustomersRe().ToList();
                        int customerI = Rand.Next(0,customerId.Count);
                        drone.DroneLocation = new();
                        drone.DroneLocation.Longitude = dal.GetCustomer(customerId[customerI]).Longitude;
                        drone.DroneLocation.Latitude = dal.GetCustomer(customerId[customerI]).Latitude;
                        Drone blDrone = GetDrone(itD.Id);
                        double disDtoS = Distance.Haversine(drone.DroneLocation.Longitude, drone.DroneLocation.Latitude, FindMinDistanceOfDToBS(blDrone).Longitude, FindMinDistanceOfDToBS(blDrone).Latitude);
                        drone.Battery = Rand.Next((int)(disDtoS * dal.AskForBattery()[0]), 101);
                    }
                    drones.Add(drone);
                }
            }
        }
    }
}
