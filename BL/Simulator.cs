using System;
using BO;
using System.Threading;
using static BL.BL;
using System.Linq;
using static System.Math;
namespace BL
{
    internal class Simulator
    {
        enum Charging { Starting, Going, Charging }
        private const int Timer = 500;
        private const double Speed = 1;
        private const double TimeStep = Timer / 1000.00;
        private const double Step = Speed / TimeStep;
        public Simulator(BL bl, int droneId, Action action, Func<bool> stop)
        {
            Drone drone = bl.GetDrone(droneId);
            int parcelId = 0;
            int stationId = 0;
            BaseStation station = null;
            Customer customer = null;
            double distance = 0.0;
            double batteryUse = 0;
            DO.Parcel parcel = default;
            bool pickUp = false;
            Charging charging = Charging.Starting;

            void Delivery(int id)
            {
                parcel = bl.dal.GetParcel(id);
                int parcelWeight = (int)parcel.Weight + 1;
                batteryUse = bl.dal.AskForBattery()[parcelWeight];
                pickUp = parcel.PickedUp is not null;
                customer = bl.GetCustomer(pickUp ? parcel.TargetId : parcel.SenderId);
            }

            while (!stop())
            {
                switch (drone.Status)
                {
                    case DroneStatus.available:
                        if (!Sleep())
                            break;
                        lock (bl)
                        {
                            parcelId = bl.dal.GetParcels(p => p.Scheduled == null
                             && (WeightCategories)p.Weight <= drone.MaxWeight
                             && bl.UseOfBattery(parcel, drone.Id) < drone.Battery)
                                .OrderByDescending(x => x.Priority)
                                .ThenByDescending(x => x.Weight)
                                .FirstOrDefault().Id;
                            if (parcelId == default && drone.Battery != 100)
                            {
                                stationId = bl.FindMinDistanceOfDToBS(drone.DroneLocation.Longitude, drone.DroneLocation.Latitude).Id;
                                if (stationId != default)
                                {
                                    drone.Status = DroneStatus.inFix;
                                    charging = Charging.Starting;
                                    bl.dal.DronToCharger(droneId, stationId);
                                    //bl.dal.DroneInStation(stationId);
                                    //DO.DroneCharge droneCharge = new() { DroneId = drone.Id,StationId=stationId };
                                    //bl.dal.AddDroneCharge(droneCharge);
                                }
                            }
                            if (parcelId != default && drone.Battery != 100)
                            {

                                bl.dal.DronToAParcel(droneId, parcelId);
                                drone.ParcelInTransfer.Id = parcelId;
                                Delivery(parcelId);
                                drone.Status = DroneStatus.delivery;
                            }
                        }
                        break;
                    case DroneStatus.inFix:
                        switch (charging)
                        {
                            #region CASE CHARGING
                            case Charging.Starting:
                                lock (bl)
                                {
                                    try
                                    {
                                        if (stationId != default)
                                            station = bl.GetBaseStation(stationId);
                                        else
                                            station = bl.GetBaseStation(bl.dal.GetDroneCharge(droneId).StationId);
                                    }
                                    catch (DO.DoesNotExistException ex)
                                    {
                                        throw new DataCorruptionException("Internal data curruption", ex);

                                    }
                                    catch (NotFoundInputException ex)
                                    {
                                        throw new DataCorruptionException("Internal data curruption", ex);
                                    }
                                    distance = Distance.Haversine
                                        (drone.DroneLocation.Longitude, drone.DroneLocation.Latitude,
                                        station.BaseStationLocation.Longitude, station.BaseStationLocation.Latitude);
                                    charging = Charging.Going;
                                }
                                break;
                            case Charging.Going:
                                if (distance < 0.01)
                                    lock (bl)
                                    {
                                        drone.DroneLocation = station.BaseStationLocation;
                                        charging = Charging.Charging;
                                    }
                                else
                                {
                                    if (!Sleep())
                                        break;
                                    lock (bl)
                                    {
                                        double del = distance < Step ? distance : Step;
                                        distance -= del;
                                        if (drone.Battery - del * bl.dal.AskForBattery()[0] < 0)
                                            drone.Battery = 0;
                                        else
                                            drone.Battery = (int)(drone.Battery - del * bl.dal.AskForBattery()[0]);
                                    }
                                }
                                break;
                            case Charging.Charging:
                                if (drone.Battery == 100)
                                    lock (bl)
                                    {
                                        drone.Status = DroneStatus.available;
                                        bl.dal.FreeDroneFromBaseStation(drone.Id);
                                    }
                                else
                                {
                                    if (!Sleep())
                                        break;
                                    lock (bl)
                                    {
                                        if (drone.Battery + bl.dal.AskForBattery()[4] > 0)
                                            drone.Battery = (int)(drone.Battery + bl.dal.AskForBattery()[4] * TimeStep);
                                        else
                                            drone.Battery = 0;
                                    }
                                }
                                break;
                            default:
                                throw new DataCorruptionException("Internal data curruption");
                                #endregion
                        }
                        if (drone.Battery == 100)
                            bl.FreeDroneFromeCharger(drone.Id);
                        break;
                    case DroneStatus.delivery:
                        lock (bl)
                        {
                            if (parcelId == 0)
                                Delivery(parcel.Id);
                            distance = Distance.Haversine(drone.DroneLocation.Longitude, drone.DroneLocation.Latitude,
                                customer.CustomerLocation.Longitude, customer.CustomerLocation.Latitude);
                        }
                        if (distance < 0.01 || drone.Battery == 0)
                            lock (bl)
                            {
                                drone.DroneLocation = customer.CustomerLocation;
                                if (pickUp)
                                {
                                    bl.dal.ParcelToCustomer(parcel.Id);
                                    drone.Status = DroneStatus.available;
                                }
                                else
                                {
                                    bl.dal.PickUpParcel(droneId);
                                }
                            }
                        else
                        {
                            if (!Sleep())
                                break;
                            lock (bl)
                            {
                                double del = distance < Step ? distance : Step;
                                double propor = del / distance;
                                drone.Battery = Max(0, (int)(drone.Battery + bl.power[(int)(pickUp ? batteryUse : 0)]));
                                double lat = drone.DroneLocation.Latitude + (customer.CustomerLocation.Latitude - drone.DroneLocation.Latitude) * propor;
                                double lon = drone.DroneLocation.Longitude + (customer.CustomerLocation.Longitude - drone.DroneLocation.Longitude) * propor;
                                drone.DroneLocation = new() { Latitude = lat, Longitude = lon };
                            }
                        }
                        break;
                    default:
                        throw new DataCorruptionException("Internal data curruption");
                }
                action();
            }
        }
        private static bool Sleep()
        {
            try
            {
                Thread.Sleep(Timer);
            }
            catch(ThreadInterruptedException)
            {
                return false;
            }
            return true;
        }
    }
}
