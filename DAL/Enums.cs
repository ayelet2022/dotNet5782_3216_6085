using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    namespace IDAL
    {
        namespace DO
        {
            public enum WeightCategories { light, inbetween, heavy };
            public enum DroneStatuses { available, inFix, delivery };
            public enum Priorities { regular, fast, urgent };
            public enum MainQuastions { AddAnObject=1, UpdateAnObject, PrintAnObjectAccordingToTheId, PrintTheWholeListOfAnObject, EndTheProgram };
            public enum AddAnObject { AddABaseStation=1, AddADrone, AddACustomer, AddAParcel };
            public enum UpdateAnObject 
            { DronToAParcel=1, PickupAPercelFromTheDron, DeliverAPercelToACustomer, SendADronToAChargeStation, FreeADronFromACharge };
            public enum PrintAnObjectAccordingToTheId
            { PresentAStationAccordingToHisId=1, PresentADroneAccordingToHisId,PesentACustomerAccordingToHisId, PresentAParcelAccordingToHisId };
            public enum PrintTheWholeListOfAnObject
            { PrintAllTheBaseStation=1, PrintAllTheDrone, PrintAllTheCustomer,PrintAllTheParcel, printAllTheParcelsThatWerentPaired,PrintAllTheBaseStationWithAvailableCharges }


        }

    }
}
