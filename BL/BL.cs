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
            Random Rand = new Random(DateTime.Now.Millisecond);
            int i = 0;
            foreach (var itD in dalObj.PrintDrones())
            {
                drones[i].Id = itD.Id;
                drones[i].Model = itD.Model;
                drones[i].MaxWeight = (WeightCategories)(int)itD.MaxWeight;
                List<IDAL.DO.Parcel> parcelList = dalObj.GetParcels();
                if (parcelList.Exists(item => item.DroneId == itD.Id))//a parcel was scheduled
                {
                    int parcelIndex = dalObj.GetParcels().FindIndex(item => item.DroneId == itD.Id);
                    drones[i].ParcelInTransfer.Id = dalObj.GetParcels()[parcelIndex].Id;
                    if (dalObj.GetParcels()[parcelIndex].Delivered == DateTime.MinValue)//a drone was scheduled but the parcel wasnt deliverd
                    {
                        drones[i].Status = (DroneStatus)2;//the drone in deliver
                        if (dalObj.GetParcels()[parcelIndex].PickedUp == DateTime.MinValue)
                        {
                            //drones[i].DroneLocation.Latitude = 0;
                        }
                        else
                        {
                            int customerIndex= dalObj.GetCustomers().FindIndex(item => item.Id == dalObj.GetParcels()[parcelIndex].SenderId);
                            drones[i].DroneLocation.Latitude = dalObj.GetCustomers()[customerIndex].Latitude;
                            drones[i].DroneLocation.Longitude = dalObj.GetCustomers()[customerIndex].Longitude;
                        }
                        //drones[i].Battery =Rand.Next(0, 101);
                    }
                }
                //if the drone isnt scheduled
                if (!dalObj.GetParcels().Exists(item => item.DroneId == itD.Id))
                {
                    drones[i].ParcelInTransfer.Id = 0;
                    drones[i].Status = (DroneStatus)Rand.Next(0, 2);
                }
                //if the drone is in fix
                if(dalObj.GetDroneCharge().Exists(item => item.DroneId == itD.Id))
                {
                    drones[i].Battery = Rand.Next(0, 21);
                }
                //if the drones isnt in fix and the drone wasnt sceduled
                if(!dalObj.GetDroneCharge().Exists(item => item.DroneId == itD.Id)&& !dalObj.GetParcels().Exists(item => item.DroneId == itD.Id))
                {
                    
                }
               
            }
        }
        
    }
}
