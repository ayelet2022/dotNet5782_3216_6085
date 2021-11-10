using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;
using IDAL;
using DalObject;
namespace BL
{
    public partial class BL
    {
        public BL()
        { 
            IDal dalObject = new DalObject.DalObject();
            double[] power=dalObject.AskForBattery();
            double chargingRate=power[4];
            dalObject.PrintDrones();
            List<Drone> Drones = new List<Drone>();
            InitializeDroneList(Drones,dalObject);
        }
        void InitializeDroneList(List<Drone>drones,IDal dalObj)
        {
            int i = 0;
            foreach (var itD in dalObj.PrintDrones())
            {
                drones[i].Id = itD.Id;
                drones[i].Model = itD.Model;
                drones[i].MaxWeight = (WeightCategories)(int)itD.MaxWeight;
                drones[i].ParcelInTransfer.Id = dalObj.GetParcel()[dalObj.GetParcel().FindIndex(item => item.DroneId == itD.Id)].Id;

            }
        }
    }
}
