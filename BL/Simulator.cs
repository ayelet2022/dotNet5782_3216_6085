using System;
using BO;
using System.Threading;
using static BL.BL;
using System.Linq;

namespace BL
{
    internal class Simulator
    {
        int Timer = 1000;
        int Speed = 1;
        public Simulator(BL bl, int droneId, Action action, Func<bool> stop)
        {
            Drone drone = bl.GetDrone(droneId);
            while(stop())
            {
                try
                {
                    switch (drone.Status)
                    {
                        case DroneStatus.available:
                            bl.ScheduledAParcelToADrone(drone.Id);
                            break;
                        case DroneStatus.inFix:
                            if (drone.Battery == 100)
                                bl.FreeDroneFromeCharger(drone.Id);
                            break;
                        case DroneStatus.delivery:
                            if (drone.ParcelInTransfer.StatusParcel == false)
                                bl.PickUpParcel(drone.Id);
                            else
                                bl.DeliverParcel(drone.Id);
                            break;
                    }
                }
                catch (FailToUpdateException ex)
                {
                    if (drone.Battery != 100)
                        bl.SendDroneToCharging(drone.Id);
                    throw;
                }
            }
        }
    }
}
