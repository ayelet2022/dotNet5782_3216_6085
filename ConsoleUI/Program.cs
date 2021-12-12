//using System;
//using DO;
//using System.Collections.Generic;
//using DalApi;

//namespace ConsoleUI
//{
//    public enum MainQuastions { AddAnObject = 1, UpdateAnObject, PrintAnObjectAccordingToTheId, PrintTheWholeListOfAnObject, EndTheProgram };
//    public enum AddAnObject { AddABaseStation = 1, AddADrone, AddACustomer, AddAParcel };
//    public enum UpdateAnObject
//    { DronToAParcel = 1, PickupAPercelFromTheDron, DeliverAPercelToACustomer, SendADronToAChargeStation, FreeADronFromACharge };
//    public enum PrintAnObjectAccordingToTheId
//    { PresentAStationAccordingToHisId = 1, PresentADroneAccordingToHisId, PesentACustomerAccordingToHisId, PresentAParcelAccordingToHisId };
//    public enum PrintTheWholeListOfAnObject
//    { PrintAllTheBaseStation = 1, PrintAllTheDrone, PrintAllTheCustomer, PrintAllTheParcel, printAllTheParcelsThatWerentPaired, PrintAllTheBaseStationWithAvailableCharges }
//    class Program 
//    {
//        //IDal dalObj = DalApi.DalFactory.GetDL();
//        IDal dalObj = DalApi.DalFactory.GetDL();
//        /// <summary>
//        /// It runs the whole program to the user.
//        /// </summary>
//        /// <param name="args"></param>
//        public static void Main(string[] args)
//        {
//            //It prints the options to the user.
//            string print = "";
//            print += $"Enter 1 to add an object.\n";
//            print += $"Enter 2 to update an object.\n";
//            print += $"Enter 3 to print an object according to the id.\n";
//            print += $"Enter 4 to print the whole list of an object.\n";
//            print += $"Enter 5 to end the program.\n";
//            Console.WriteLine(print);
//            MainQuastions choice;
//            int input = 0;
//            double inputDouble = 0;
//            MainQuastions.TryParse(Console.ReadLine(), out choice);//cin the useres choice
//            try
//            {
//                while (choice != MainQuastions.EndTheProgram)//goes till the user enters 5-end the program
//                {
//                    switch (choice)
//                    {
//                        //case the user entered 1
//                        case MainQuastions.AddAnObject:
//                            //It prints the add options to the user.
//                            string print1 = "";
//                            print1 += $"Enter what would you like to add:\n";
//                            print1 += $"Enter 1 to add a base station.\n";
//                            print1 += $"Enter 2 to add a drone.\n";
//                            print1 += $"Enter 3 to add a customer.\n";
//                            print1 += $"Enter 4 to add a parcel.\n";
//                            Console.WriteLine(print1);
//                            AddAnObject ChoiceAdd;
//                            AddAnObject.TryParse(Console.ReadLine(), out ChoiceAdd);//cin the useres choice
//                            switch (ChoiceAdd)
//                            {
                                
//                                //a case to add a baseStation
//                                case AddAnObject.AddABaseStation:
                                    
//                                    BaseStation newStation = new();
//                                    Console.Write("Enter Id (4 digits): ");
//                                    int.TryParse(Console.ReadLine(), out input);
//                                    newStation.Id = input;
//                                    Console.Write("Enter name: ");
//                                    newStation.Name = Console.ReadLine();
//                                    Console.Write("Enter longitude: ");
//                                    double.TryParse(Console.ReadLine(), out inputDouble);
//                                    newStation.Longitude = inputDouble;
//                                    Console.Write("Enter Latitude: ");
//                                    double.TryParse(Console.ReadLine(), out inputDouble);
//                                    newStation.Latitude = inputDouble;
//                                    Console.Write("Enter amount of charge slots in new station : ");
//                                    int.TryParse(Console.ReadLine(), out input);
//                                    newStation.EmptyCharges = input;
//                                    dalObj.AddBaseStation(newStation);
//                                    break;
//                                //a case to add a drone
//                                case AddAnObject.AddADrone:
//                                    Drone newDrone = new();
//                                    Console.Write("Enter Id (4 digits): ");
//                                    int.TryParse(Console.ReadLine(), out input);
//                                    newDrone.Id = input;
//                                    Console.Write("Enter model: ");
//                                    newDrone.Model = Console.ReadLine();
//                                    Console.WriteLine("Enter weight (light=0/inbetween=1/heavy=2)");
//                                    int.TryParse(Console.ReadLine(), out input);
//                                    newDrone.MaxWeight = (WeightCategories)input;
//                                    Console.WriteLine("Enter status (available=0/inFix=1/delivery=2)");
//                                    int.TryParse(Console.ReadLine(), out input);
//                                    // newDrone.Status = (DroneStatuses)input;
//                                    Console.Write("Enter battery: ");
//                                    int.TryParse(Console.ReadLine(), out input);
//                                    // newDrone.Battery = input;
//                                    dalObj.AddDrone(newDrone);
//                                    break;
//                                //a case to add a customer and get the details from the user.
//                                case AddAnObject.AddACustomer:
//                                    Customer newCustomer = new Customer();//creats a new customer.
//                                    Console.WriteLine("Enter Id (4 digits): ");
//                                    int idCustomer;
//                                    int.TryParse(Console.ReadLine(), out idCustomer);//cin the id of the new customer
//                                    newCustomer.Id = idCustomer;//updates the new customers id
//                                    Console.WriteLine("Enter Name: ");
//                                    string name = Console.ReadLine();//cin the name of the new customer
//                                    newCustomer.Name = name;//updates the new customers name
//                                    Console.WriteLine("Enter Phone: ");
//                                    string phone = Console.ReadLine();//cin the phone of the new customer
//                                    newCustomer.Phone = phone;//updates the new customers phone
//                                    Console.WriteLine("Enter Latitude: ");
//                                    double latitude;
//                                    double.TryParse(Console.ReadLine(), out latitude);//cin the latitude of the new customer
//                                    newCustomer.Latitude = latitude;//updates the new customers latitude
//                                    Console.WriteLine("Enter Longitude: ");
//                                    double longitude;
//                                    double.TryParse(Console.ReadLine(), out longitude);//cin the longitude of the new customer
//                                    newCustomer.Longitude = longitude;//updates the new customers longitude
//                                    dalObj.AddCustomer(newCustomer);
//                                    break;
//                                //a case to add a parcel and get the details from the user.
//                                case AddAnObject.AddAParcel:
//                                    Parcel newParcel = new Parcel();
//                                    newParcel.Id = 0;//updates the new parcels id(it will change in the function)
//                                    Console.WriteLine("Enter Sender id (4 digits): ");
//                                    int senderId;
//                                    int.TryParse(Console.ReadLine(), out senderId);//cin the sender id of the new parcel
//                                    newParcel.SenderId = senderId;//updates the new parcels sender id
//                                    Console.WriteLine("Enter Target id (4 digits): ");
//                                    int targetId;
//                                    int.TryParse(Console.ReadLine(), out targetId);//cin the target id of the new parcel
//                                    newParcel.TargetId = targetId;//updates the new parcels target id
//                                    Console.WriteLine("Enter Weight (light=0/inbetween=1/heavy=2): ");
//                                    int weight;
//                                    int.TryParse(Console.ReadLine(), out weight);//cin the weight of the new parcel
//                                    newParcel.Weight = (WeightCategories)weight;//updates the new parcels weight
//                                    Console.WriteLine("Enter Priority (regular=0/fast=1/urgent=2): ");
//                                    int proiority;
//                                    int.TryParse(Console.ReadLine(), out proiority);//cin the priority of the new parcel
//                                    newParcel.Priority = (Priorities)proiority;//updates the new parcels proiority
//                                    dalObj.AddParcel(newParcel);
//                                    break;
//                            }
//                            break;
//                        //case the user entered 2
//                        case MainQuastions.UpdateAnObject:
//                            //It prints the update options to the user.
//                            String result = " ";
//                            result += "What would you want to update? \n";
//                            result += "Enter 1 to conect a dron to a parcel\n";
//                            result += "Enter 2 to pickup a percel from the dron\n";
//                            result += "Enter 3 to deliver a percel to a customer\n";
//                            result += "Enter 4 to send a dron to a charge station\n";
//                            result += "Enter 5 to free a dron from a charge station\n";
//                            Console.WriteLine(result);
//                            UpdateAnObject updateChoice;
//                            UpdateAnObject.TryParse(Console.ReadLine(), out updateChoice);//cin the useres choice
//                            switch (updateChoice)
//                            {
//                                //a case to assign a parcel to a drone
//                                case UpdateAnObject.DronToAParcel:
//                                    Console.WriteLine("Enter the  percel Id (4 digits): ");
//                                    int percelId1;
//                                    int.TryParse(Console.ReadLine(), out percelId1);//cin the parcels id
//                                    Console.WriteLine("Enter the  drone Id (4 digits): ");
//                                    int droneId1;
//                                    int.TryParse(Console.ReadLine(), out droneId1);//cin the drones id
//                                    dalObj.DronToAParcel(droneId1, percelId1);//sends to a function that assigns a parcel to a drone
//                                    break;
//                                //a case to pick up a parcel fron a drone
//                                case UpdateAnObject.PickupAPercelFromTheDron:
//                                    Console.WriteLine("Enter the  percel Id (4 digits): ");
//                                    int PercelId2;
//                                    int.TryParse(Console.ReadLine(), out PercelId2);//cin the parcels id
//                                    dalObj.PickUpParcel(PercelId2);//sends to a function that picks up a parcel fron a drone
//                                    break;
//                                //a case to deliver a parcel to the customer
//                                case UpdateAnObject.DeliverAPercelToACustomer:
//                                    Console.WriteLine("Enter the  percel Id (4 digits): ");
//                                    int PercelId3;
//                                    int.TryParse(Console.ReadLine(), out PercelId3);//cin the parcels id
//                                    dalObj.ParcelToCustomer(PercelId3);// sends to a function that delivers a parcel to the customer
//                                    break;
//                                //a case to send a drone to a charge station
//                                case UpdateAnObject.SendADronToAChargeStation:
//                                    Console.WriteLine("Enter the  drone Id (4 digits): ");
//                                    int DronesId1;
//                                    int.TryParse(Console.ReadLine(), out DronesId1);//cin the drones id
//                                    Console.WriteLine("Enter the id of the basestation you whant to charge the drone in (from the list): ");
//                                    IEnumerable<BaseStation> Stations = dalObj.GetBaseStations();
//                                    //prints for the user the baseStations that have free charge stations
//                                    foreach (var itBS in Stations)
//                                        Console.WriteLine(itBS.ToString());
//                                    int IdOfBaseStation;
//                                    int.TryParse(Console.ReadLine(), out IdOfBaseStation);//cin the baseStations id
//                                    dalObj.DronToCharger(DronesId1, IdOfBaseStation);// sends to a function that sends a drone to a charge station
//                                    break;
//                                //a case to free a drone from a charge station
//                                case UpdateAnObject.FreeADronFromACharge:
//                                    Console.WriteLine("Enter the  drone Id (4 digits): ");
//                                    int DronesId2;
//                                    int.TryParse(Console.ReadLine(), out DronesId2);//cin the drones id
//                                    dalObj.FreeDroneFromBaseStation(DronesId2);// sends to a function that frees a drone from a charge station
//                                    break;
//                            }
//                            break;
//                        //case the user entered 3
//                        case MainQuastions.PrintAnObjectAccordingToTheId:
//                            //It prints the prints options to the user.
//                            String print3 = "";
//                            print3 += $"Enter what would you like to print\n";
//                            print3 += $"1 to present a station according to his id.\n";
//                            print3 += $"2 to present a drone according to his id.\n";
//                            print3 += $"3 to present a customer according to his id.\n";
//                            print3 += $"4 to present a parcel according to his id.\n";
//                            Console.WriteLine(print3);
//                            PrintAnObjectAccordingToTheId choiceAccorIdPrint;
//                            PrintAnObjectAccordingToTheId.TryParse(Console.ReadLine(), out choiceAccorIdPrint);//cin the useres choice
//                            switch (choiceAccorIdPrint)
//                            {
//                                //in case you want to print a baseStation
//                                case PrintAnObjectAccordingToTheId.PresentAStationAccordingToHisId:
//                                    Console.WriteLine("Enter the base station id (4 digits)\n");
//                                    int id;
//                                    int.TryParse(Console.ReadLine(), out id);//cin the baseStations id from the user
//                                                                             //sends to a function that finds the baseStation and prints the info
//                                    Console.WriteLine(dalObj.GetBaseStation(id).ToString());
//                                    break;
//                                //in case you want to print a drone
//                                case PrintAnObjectAccordingToTheId.PresentADroneAccordingToHisId:
//                                    Console.WriteLine("Enter the drone id (4 digits)\n");
//                                    int idDrone;
//                                    int.TryParse(Console.ReadLine(), out idDrone);//cin the drones id from the user
//                                                                                  //sends to a function that finds the drone and prints the info
//                                    Console.WriteLine(dalObj.GetDrone(idDrone).ToString());
//                                    break;
//                                //in case you want to print a customer
//                                case PrintAnObjectAccordingToTheId.PesentACustomerAccordingToHisId:
//                                    Console.WriteLine("Enter the customer id (4 digits)\n");
//                                    int idCustomer;
//                                    int.TryParse(Console.ReadLine(), out idCustomer);//cin the customers id from the user
//                                                                                     //sends to a function that finds the customer and prints the info
//                                    Console.WriteLine(dalObj.GetCustomer(idCustomer).ToString());
//                                    break;
//                                //in case you want to print a parcel
//                                case PrintAnObjectAccordingToTheId.PresentAParcelAccordingToHisId:
//                                    Console.WriteLine("Enter the parcel id (4 digits)\n");
//                                    int idParcel;
//                                    int.TryParse(Console.ReadLine(), out idParcel);//cin the parcels id from the user
//                                                                                   //sends to a function that finds the parcel and prints the info
//                                    Console.WriteLine(dalObj.GetParcel(idParcel).ToString());
//                                    break;
//                            }
//                            break;
//                        //case the user entered 4
//                        case MainQuastions.PrintTheWholeListOfAnObject:
//                            //It prints the prints options to the user.
//                            string print4 = "";
//                            print4 += $"What list would you like to print ?\n";
//                            print4 += $"Enter 1 to print all the base stations\n";
//                            print4 += $"Enter 2 to print all the drone\n";
//                            print4 += $"Enter 3 to print all the customer\n";
//                            print4 += $"Enter 4 to print all the parcel\n";
//                            print4 += $"Enter 5 to print all the parcels that weren't paired\n";
//                            print4 += $"Enter 6 to print all the base station with available charges\n";
//                            Console.WriteLine(print4);
//                            PrintTheWholeListOfAnObject PrintChoice;
//                            PrintTheWholeListOfAnObject.TryParse(Console.ReadLine(), out PrintChoice);//cin the useres choice
//                            switch (PrintChoice)
//                            {
//                                //in case you want to print the whole baseStations
//                                case PrintTheWholeListOfAnObject.PrintAllTheBaseStation:
//                                    //creats a new list of baseStations and sends to a function that returns the list of the baseStations
//                                    IEnumerable<BaseStation> ActiveStations = dalObj.GetBaseStations();
//                                    //goes through the list and prints the info of each one.
//                                    foreach (var itBS in ActiveStations)
//                                        Console.WriteLine(itBS.ToString());
//                                    break;
//                                //in case you want to print the whole drones
//                                case PrintTheWholeListOfAnObject.PrintAllTheDrone:
//                                    //creats a new list of drones and sends to a function that returns the list of the drones
//                                    IEnumerable<Drone> ActiveDrones = dalObj.GetDrones();
//                                    //goes through the list and prints the info of each one.
//                                    foreach (var itD in ActiveDrones)
//                                        Console.WriteLine(itD.ToString());
//                                    break;
//                                //in case you want to print the whole customers
//                                case PrintTheWholeListOfAnObject.PrintAllTheCustomer:
//                                    //creats a new list of customers and sends to a function that returns the list of the customers
//                                    IEnumerable<Customer> ActiveCustomers = dalObj.GetCustomers();
//                                    //goes through the list and prints the info of each one.
//                                    foreach (var itC in ActiveCustomers)
//                                        Console.WriteLine(itC.ToString());
//                                    break;
//                                //in case you want to print the whole parcels
//                                case PrintTheWholeListOfAnObject.PrintAllTheParcel:
//                                    //creats a new list of parcels and sends to a function that returns the list of the parcels
//                                    IEnumerable<Parcel> ActiveParcels = dalObj.GetParcels();
//                                    //goes through the list and prints the info of each one.
//                                    foreach (var itP in ActiveParcels)
//                                        Console.WriteLine(itP.ToString());
//                                    break;
//                                //in case you want to print the parcels that were not paired to a drone.
//                                case PrintTheWholeListOfAnObject.printAllTheParcelsThatWerentPaired:
//                                    //creats a new list of parcels and sends to a function that returns the list of the parcels that were not paired to a drone.
//                                    IEnumerable<Parcel> Parcels = dalObj.GetParcels(item => item.Scheduled == null);
//                                    //goes through the list and prints the info of each one.
//                                    foreach (var itP in Parcels)
//                                        Console.WriteLine(itP.ToString());
//                                    break;
//                                //in case you want to print the baseStations with available chargers
//                                case PrintTheWholeListOfAnObject.PrintAllTheBaseStationWithAvailableCharges:
//                                    //creats a new list of parcels and sends to a function that returns the list of the baseStations with available chargers
//                                    IEnumerable<BaseStation> Stations = dalObj.GetBaseStations(item => item.EmptyCharges != 0);
//                                    //goes through the list and prints the info of each one.
//                                    foreach (var itS in Stations)
//                                        Console.WriteLine(itS.ToString());
//                                    break;
//                            }
//                            break;
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            //It prints the options to the user.
//            Console.WriteLine("Enter 1 to add an object,");
//            Console.WriteLine("Enter 2 to update an object, ");
//            Console.WriteLine("Enter 3 to print an object according to the id, ");
//            Console.WriteLine("Enter 4 to print the whole list of an object, ");
//            Console.WriteLine("Enter 5 to end the program, ");
//            MainQuastions.TryParse(Console.ReadLine(), out choice);//cin the users choice.
//        }
//    }
//}
