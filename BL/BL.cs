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
            double[] power=dal.AskForBattery();
            double chargingRate=power[4];
            dal.GetDrones();
            InitializeDroneList(Drones);
        }

       
        
        void InitializeDroneList(List<DroneList>drones)
        {
            Random Rand = new Random(DateTime.Now.Millisecond);
            int i = 0;
            foreach (var itD in dal.GetDrones())
            {
                drones[i].Id = itD.Id;
                drones[i].Model = itD.Model;
                drones[i].MaxWeight = (WeightCategories)(int)itD.MaxWeight;
                try
                {
                    IDAL.DO.Parcel parcel = dal.GetParcels().First(item => item.DroneId == itD.Id && item.Delivered == DateTime.MinValue); //a parcel was scheduled
                }
                    //int parcelIndex = dal.GetParcels().FindIndex(item => item.DroneId == itD.Id);
                    //drones[i].ParcelInTransfer.Id = dal.GetParcels()[parcelIndex].Id;
                    //if (dal.GetParcels()[parcelIndex].Delivered == DateTime.MinValue)//a drone was scheduled but the parcel wasnt deliverd
                    //{
                    //    drones[i].Status = (DroneStatus)2;//the drone in deliver
                    //    if (dal.GetParcels()[parcelIndex].PickedUp == DateTime.MinValue)
                    //    {
                    //        //drones[i].DroneLocation.Latitude = 0;
                    //    }
                    //    else
                    //    {
                    //        int customerIndex= dal.GetCustomers().FindIndex(item => item.Id == dal.GetParcels()[parcelIndex].SenderId);
                    //        drones[i].DroneLocation.Latitude = dal.GetCustomers()[customerIndex].Latitude;
                    //        drones[i].DroneLocation.Longitude = dal.GetCustomers()[customerIndex].Longitude;
                    //    }
                    //    //drones[i].Battery =Rand.Next(0, 101);
                    //}
                catch (InvalidOperationException ex)
                {
                    // not found
                }
                //if the drone isnt scheduled
                //if (!dal.GetParcels().Exists(item => item.DroneId == itD.Id))
                //{
                //    drones[i].ParcelInTransfer.Id = 0;
                //    drones[i].Status = (DroneStatus)Rand.Next(0, 2);
                //}
                ////if the drone is in fix
                //if(dal.GetDroneCharge().Exists(item => item.DroneId == itD.Id))
                //{
                //    drones[i].Battery = Rand.Next(0, 21);
                //}
                ////if the drones isnt in fix and the drone wasnt sceduled
                //if(!dal.GetDroneCharge().Exists(item => item.DroneId == itD.Id)&& !dal.GetParcels().Exists(item => item.DroneId == itD.Id))
                //{
                    
                //}
               
            }
        }
        
    }
}
