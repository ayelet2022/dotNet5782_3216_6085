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
            public enum MainQuastions { AddAnObject, UpdateAnObject, PrintAnObjectAccordingToTheId, PrintTheWholeListOfAnObject, EndTheProgram };
            public enum AddAnObject { AddABaseStation, AddADrone, AddACustomer, AddAParcel };
            public enum UpdateAnObject 
            { DronToAParcel, PickupAPercelFromTheDron, DeliverAPercelToACustomer, SendADronToAChargeStation, FreeADronFromACharge };
            public enum PrintAnObjectAccordingToTheId
            { PresentAStationAccordingToHisId, PresentADroneAccordingToHisId, DeliverAPercelToACustomer, PesentACustomerAccordingToHisId, PresentAParcelAccordingToHisId };
            public enum PrintTheWholeListOfAnObject
            { PrintAllTheBaseStations, PrintAllTheDrone, PrintAllTheCustomer,PrintAllTheParcel, printAllTheParcelsThatWerentPaired,PrintAllTheBaseStationWithAvailableCharges }


        }

    }
}
