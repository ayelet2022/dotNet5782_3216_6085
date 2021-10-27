using System;
using DAL.DalObject;
using DAL;
using DAL.IDAL.DO;
namespace ConsoleUI
{
    class Program 
    {
        static void Main(string[] args)
        {
            DalObject obj = new DalObject();
            string print="";
            print+=$"Enter 1 to add an object.\n";
            print+=$"Enter 2 to update an object.\n";
            print+=$"Enter 3 to print an object according to the id.\n";
            print+= $"Enter 4 to print the whole list of an object.\n";
            print+= $"Enter 5 to end the program.\n";
            Console.WriteLine(print);
            MainQuastions choice;
            MainQuastions.TryParse(Console.ReadLine(), out choice);
            while (choice!=MainQuastions.EndTheProgram)
            {
                switch (choice)
                {
                    case MainQuastions.AddAnObject:
                        string print1 = "";
                        print1+=$"Enter what would you like to add:\n";
                        print1+=$"Enter 1 to add a base station.\n";
                        print1+=$"Enter 2 to add a drone.\n";
                        print1+=$"Enter 3 to add a customer.\n";
                        print1+=$"Enter 4 to add a parcel.\n";
                        Console.WriteLine(print1);
                        AddAnObject ChoiceAdd;
                        AddAnObject.TryParse(Console.ReadLine(), out ChoiceAdd);
                        switch (ChoiceAdd)
                        {
                            case AddAnObject.AddABaseStation:
                                DalObject.AddBaseStation();
                                break;
                            case AddAnObject.AddADrone:
                                DalObject.AddDrone();
                                break;
                            case AddAnObject.AddACustomer:
                                Customer newCustomer = new Customer();
                                Console.WriteLine("Enter Id (4 digits): ");
                                int idCustomer;
                                int.TryParse(Console.ReadLine(), out idCustomer);
                                newCustomer.Id = idCustomer;
                                Console.WriteLine("Enter Name: ");
                                string name = Console.ReadLine();
                                newCustomer.Name = name;
                                Console.WriteLine("Enter Phone: ");
                                string phone = Console.ReadLine();
                                newCustomer.Phone = phone;
                                Console.WriteLine("Enter Latitude: ");
                                double latitude;
                                double.TryParse(Console.ReadLine(), out latitude);
                                newCustomer.Latitude = latitude;
                                Console.WriteLine("Enter Longitude: ");
                                double longitude;
                                double.TryParse(Console.ReadLine(), out longitude);
                                newCustomer.Longitude = longitude;
                                DalObject.AddCustomer(newCustomer);
                                break;
                            case AddAnObject.AddAParcel:
                                Parcel newParcel = new Parcel();
                                newParcel.Id = 0;
                                Console.WriteLine("Enter SenderId (4 digits): ");
                                int senderId;
                                int.TryParse(Console.ReadLine(), out senderId);
                                newParcel.SenderId = senderId;
                                Console.WriteLine("Enter TargetId (4 digits): ");
                                int targetId;
                                int.TryParse(Console.ReadLine(), out targetId);
                                newParcel.TargetId = targetId;
                                Console.WriteLine("Enter Weight (light=0/inbetween=1/heavy=2): ");
                                int weight;
                                int.TryParse(Console.ReadLine(), out weight);
                                newParcel.Weight = (WeightCategories)weight;
                                Console.WriteLine("Enter Priority (regular=0/fast=1/urgent=2): ");
                                int proiority;
                                int.TryParse(Console.ReadLine(), out proiority);
                                newParcel.Priority = (Priorities)proiority;
                                DalObject.AddParcel(newParcel);
                                break;
                        }
                        break;
                    case MainQuastions.UpdateAnObject:
                        String result = " ";
                        result += "What would you want to update? \n";
                        result += "Enter 1 to conect a dron to a parcel\n";
                        result += "Enter 2 to pickup a percel from the dron\n";
                        result += "Enter 3 to deliver a percel to a customer\n";
                        result += "Enter 4 to send a dron to a charge station\n";
                        result += "Enter 5 to free a dron from a charge station\n";
                        Console.WriteLine(result);
                        UpdateAnObject updateChoice;
                        UpdateAnObject.TryParse(Console.ReadLine(), out updateChoice);
                        switch (updateChoice)
                        {
                            case UpdateAnObject.DronToAParcel:
                                Console.WriteLine("Enter the  percel Id (4 digits): ");
                                int PercelId1;
                                int.TryParse(Console.ReadLine(), out PercelId1);
                                DAL.DalObject.DalObject.DronToAParcel(PercelId1);
                                break;
                            case UpdateAnObject.PickupAPercelFromTheDron:
                                Console.WriteLine("Enter the  percel Id (4 digits): ");
                                int PercelId2;
                                int.TryParse(Console.ReadLine(), out PercelId2);
                                DAL.DalObject.DalObject.PickUpParcel(PercelId2);
                                break;
                            case UpdateAnObject.DeliverAPercelToACustomer:
                                Console.WriteLine("Enter the  percel Id (4 digits): ");
                                int PercelId3;
                                int.TryParse(Console.ReadLine(), out PercelId3);
                                DAL.DalObject.DalObject.ParcelToCustomer(PercelId3);
                                break;
                            case UpdateAnObject.SendADronToAChargeStation:
                                Console.WriteLine("Enter the  drone Id (4 digits): ");
                                int DronesId1;
                                int.TryParse(Console.ReadLine(), out DronesId1);
                                Console.WriteLine("Enter the id of the basestation you whant to charge the drone in (from the list): ");
                                BaseStation[] Stations = DAL.DalObject.DalObject.BaseStationWithAvailableCharges();
                                for (int baseStationIndex = 0; baseStationIndex < Stations.Length; baseStationIndex++)
                                    Console.WriteLine(Stations[baseStationIndex].ToString());
                                int IdOfBaseStation;
                                int.TryParse(Console.ReadLine(), out IdOfBaseStation);
                                DAL.DalObject.DalObject.DronToCharger(DronesId1, IdOfBaseStation);
                                break;
                            case UpdateAnObject.FreeADronFromACharge:
                                Console.WriteLine("Enter the  drone Id (4 digits): ");
                                int DronesId2;
                                int.TryParse(Console.ReadLine(), out DronesId2);
                                DAL.DalObject.DalObject.FreeDroneFromBaseStation(DronesId2);
                                break;
                        }
                        break;
                    case MainQuastions.PrintAnObjectAccordingToTheId:
                        String print3 = "";
                        print3 += $"Enter what would you like to do\n";
                        print3 += $"1 to present a station according to his id.\n";
                        print3 += $"2 to present a drone according to his id.\n";
                        print3 += $"3 to present a customer according to his id.\n";
                        print3 += $"4 to present a parcel according to his id.\n";
                        Console.WriteLine(print3);
                        PrintAnObjectAccordingToTheId choiceAccorIdPrint;
                        PrintAnObjectAccordingToTheId.TryParse(Console.ReadLine(), out choiceAccorIdPrint);
                        switch (choiceAccorIdPrint)
                        {
                            case PrintAnObjectAccordingToTheId.PresentAStationAccordingToHisId:
                                Console.WriteLine("Enter the base station id (4 digits)\n");
                                int id;
                                int.TryParse(Console.ReadLine(), out id);
                                Console.WriteLine(DalObject.PrintBaseStation(id).ToString());
                                break;
                            case PrintAnObjectAccordingToTheId.PresentADroneAccordingToHisId:
                                Console.WriteLine("Enter the drone id (4 digits)\n");
                                int idDrone;
                                int.TryParse(Console.ReadLine(), out idDrone);
                                Console.WriteLine(DalObject.PrintDrone(idDrone).ToString());
                                break;
                            case PrintAnObjectAccordingToTheId.PesentACustomerAccordingToHisId:
                                Console.WriteLine("Enter the customer id (4 digits)\n");
                                int idCustomer;
                                int.TryParse(Console.ReadLine(), out idCustomer);
                                Console.WriteLine(DalObject.PrintCustomer(idCustomer).ToString());
                                break;
                            case PrintAnObjectAccordingToTheId.PresentAParcelAccordingToHisId:
                                Console.WriteLine("Enter the parcel id (4 digits)\n");
                                int idParcel;
                                int.TryParse(Console.ReadLine(), out idParcel);
                                Console.WriteLine(DalObject.PrintParcel(idParcel).ToString());
                                break;
                        }
                        break;
                    case MainQuastions.PrintTheWholeListOfAnObject:
                        string print4="";
                        print4+= $"What list would you like to print ?\n";
                        print4+= $"Enter 1 to print all the base stations\n";
                        print4+= $"Enter 2 to print all the drone\n";
                        print4+= $"Enter 3 to print all the customer\n";
                        print4+= $"Enter 4 to print all the parcel\n";
                        print4+= $"Enter 5 to print all the parcels that weren't paired\n";
                        print4+= $"Enter 6 to print all the base station with available charges\n";
                        Console.WriteLine("What list would you like to print?");
                        PrintTheWholeListOfAnObject PrintChoice;
                        PrintTheWholeListOfAnObject.TryParse(Console.ReadLine(), out PrintChoice);
                        switch (PrintChoice)
                        {
                            case PrintTheWholeListOfAnObject.PrintAllTheBaseStations:
                                BaseStation[] ActiveStations = DAL.DalObject.DalObject.PrintBaseStations();
                                for (int ActiveStationIndex = 0; ActiveStationIndex < ActiveStations.Length; ActiveStationIndex++)
                                    Console.WriteLine(ActiveStations[ActiveStationIndex].ToString());
                                break;
                            case PrintTheWholeListOfAnObject.PrintAllTheDrone:
                                Drone[] ActiveDrones = DAL.DalObject.DalObject.PrintDrones();
                                for (int activeDronesIndex = 0; activeDronesIndex < ActiveDrones.Length; activeDronesIndex++)
                                    Console.WriteLine(ActiveDrones[activeDronesIndex].ToString());
                                break;
                            case PrintTheWholeListOfAnObject.PrintAllTheCustomer:
                                Customer[] ActiveCustomers = DAL.DalObject.DalObject.PrintCustomers();
                                for (int activeCustomersIndex = 0; activeCustomersIndex < ActiveCustomers.Length; activeCustomersIndex++)
                                    Console.WriteLine(ActiveCustomers[activeCustomersIndex].ToString());
                                break;
                            case PrintTheWholeListOfAnObject.PrintAllTheParcel:
                                Parcel[] ActiveParcels = DAL.DalObject.DalObject.PrintPercels();
                                for (int activeParcelsIndex = 0; activeParcelsIndex < ActiveParcels.Length; activeParcelsIndex++)
                                    Console.WriteLine(ActiveParcels[activeParcelsIndex].ToString());
                                break;
                            case PrintTheWholeListOfAnObject.printAllTheParcelsThatWerentPaired:
                                Parcel[] Parcels = DAL.DalObject.DalObject.ParcelThatWerenNotPaired();
                                for (int parcelsIndex = 0; parcelsIndex < Parcels.Length; parcelsIndex++)
                                    Console.WriteLine(Parcels[parcelsIndex].ToString());
                                break;
                            case PrintTheWholeListOfAnObject.PrintAllTheBaseStationWithAvailableCharges:
                                BaseStation[] Stations = DAL.DalObject.DalObject.BaseStationWithAvailableCharges();
                                for (int stationsIndex = 0; stationsIndex < Stations.Length; stationsIndex++)
                                    Console.WriteLine(Stations[stationsIndex].ToString());
                                break;
                        }
                        break;
                }
                Console.WriteLine("Enter 1 to add an object, ");
                Console.WriteLine("Enter 2 to update an object, ");
                Console.WriteLine("Enter 3 to print an object according to the id, ");
                Console.WriteLine("Enter 4 to print the whole list of an object, ");
                Console.WriteLine("Enter 5 to end the program, ");
                MainQuastions.TryParse(Console.ReadLine(), out choice);
            }
        }
    }
}
