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
            foreach (var itD in dalObject.PrintDrones())
            {
                if(itD.)
            }
            dalObject.PrintDrones();
        }
    }
}
