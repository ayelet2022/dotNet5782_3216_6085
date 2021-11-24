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
        static Random Rand = new Random();
        double chargingRate;
        double[] power;
        /// <summary>
        /// the contractor of BL
        /// </summary>
        public BL()
        {
            power = dal.AskForBattery();
            chargingRate = power[4]; 
            InitializeDroneList(Drones);
        }
        /// <summary>
        /// the builder if list
        /// </summary>
        /// <param name="drones">the list we whant to build</param>
        void InitializeDroneList(List<DroneList> drones)
        {
            DroneList drone = new();
            //to copy all the drones frome the list
            foreach (var itD in dal.GetDrones())
            {
                itD.CopyPropertiesTo(drone);
                try
                {
                    IDAL.DO.Parcel parcel = dal.GetParcels().First(item => item.DroneId == itD.Id && item.Delivered == DateTime.MinValue); //a parcel was scheduled
                    drone.Status = DroneStatus.delivery;//the drone in deliver
                    double customerSLatitude = dal.GetCustomer(parcel.SenderId).Latitude;
                    double customerSLongitude = dal.GetCustomer(parcel.SenderId).Longitude;
                    double customerRLatitude = dal.GetCustomer(parcel.TargetId).Latitude;
                    double customerRLongitude = dal.GetCustomer(parcel.TargetId).Longitude;
                    double minBattery = 0;
                    //the distance from the sender to the resever
                    double disStoR = Distance.Haversine(customerSLongitude, customerSLatitude, customerRLongitude, customerRLatitude);
                    //the distatnce from the resever of the parcel to the closest base station to him
                    double disRtoBS = Distance.Haversine
                        (customerRLongitude, customerRLatitude, FindMinDistanceOfCToBS(customerRLatitude,customerRLongitude).Longitude, FindMinDistanceOfCToBS(customerRLatitude, customerRLongitude).Latitude);
                    drone.DroneLocation = new();
                    if (parcel.PickedUp == DateTime.MinValue)//meens the parcel wasnt picked up by the drone
                    {
                        //fineds the closest base station to the customer that sends the paecel and copys the location
                        drone.DroneLocation.Latitude = FindMinDistanceOfCToBS(customerSLatitude, customerSLongitude).Latitude;
                        drone.DroneLocation.Longitude = FindMinDistanceOfCToBS(customerSLatitude, customerSLongitude).Longitude;
                        //the ditance in km frome the dron lication to the customer that suppos to riseve the parcel
                        double disDtoS = Distance.Haversine
                            (drone.DroneLocation.Longitude, drone.DroneLocation.Latitude, customerSLongitude, customerSLatitude);
                        //culclate how much battery does the drone needs in order to do the delivery
                        //culcilate the km that the drone does without a parcel*how much battery it takes+
                        //culcilate the km that the drone does with a parcel*how much battery it takes
                        minBattery = (disDtoS + disRtoBS) * dal.AskForBattery()[0] + disStoR * dal.AskForBattery()[(int)parcel.Weight + 1];
                    }
                    else//meens the parcel was piched up
                    {
                        //copys the location of  the customer that sends the paecel
                        drone.DroneLocation.Latitude = customerSLatitude;
                        drone.DroneLocation.Longitude = customerSLatitude;
                        //culclate how much battery does the drone needs in order to do the delivery
                        //culcilate the km that the drone does without a parcel*how much battery it takes+
                        //culcilate the km that the drone does with a parcel*how much battery it takes
                        minBattery = disRtoBS * dal.AskForBattery()[0] + disStoR * dal.AskForBattery()[(int)parcel.Weight + 1];
                    }
                    drone.Battery = Rand.Next((int)minBattery, 101);
                    drones.Add(drone);
                    drone = new();

                }
                //meens thar are no parcels schedulde to the drone
                catch (InvalidOperationException)
                {
                    drone.Status = (DroneStatus)Rand.Next(0, 2);
                    if (drone.Status == DroneStatus.inFix)
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
                        int customerI = Rand.Next(0, customerId.Count);
                        drone.DroneLocation = new();
                        drone.DroneLocation.Longitude = dal.GetCustomer(customerId[customerI]).Longitude;
                        drone.DroneLocation.Latitude = dal.GetCustomer(customerId[customerI]).Latitude;
                        double disDtoS = Distance.Haversine
                            (drone.DroneLocation.Longitude, drone.DroneLocation.Latitude, FindMinDistanceOfDToBS(drone.DroneLocation.Longitude, drone.DroneLocation.Latitude).Longitude, FindMinDistanceOfDToBS(drone.DroneLocation.Longitude, drone.DroneLocation.Latitude).Latitude);
                        drone.Battery = Rand.Next((int)(disDtoS * dal.AskForBattery()[0]), 101);
                    }
                    drones.Add(drone);
                    drone = new();
                }
            }
        }
    }
}
