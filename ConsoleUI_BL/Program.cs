using System;
using System.Collections.Generic;
using BL;
using IBL.BO;

namespace ConsoleUI_BL
{
    public enum MainQuastions { AddAnObject = 1, UpdateAnObject, PrintAnObjectAccordingToTheId, PrintTheWholeListOfAnObject, EndTheProgram };
    public enum AddAnObject { AddABaseStation = 1, AddADrone, AddACustomer, AddAParcel };
    public enum UpdateAnObject
    { updataADrone = 1, updataABaseStation, updataACustomer, SendADronToACharger, FreeADronFromACharger,ScheduledAParcelToADrone,PickupAParcelByADrone, };
    public enum PrintAnObjectAccordingToTheId
    { PresentAStationAccordingToHisId = 1, PresentADroneAccordingToHisId, PesentACustomerAccordingToHisId, PresentAParcelAccordingToHisId };
    public enum PrintTheWholeListOfAnObject
    { PrintAllTheBaseStation = 1, PrintAllTheDrone, PrintAllTheCustomer, PrintAllTheParcel, printAllTheParcelsThatWerentPaired, PrintAllTheBaseStationWithAvailableCharges }
    class Program
    {
        static BL.BL obj = new BL.BL();
        static void Main(string[] args)
        {
            //It prints the options to the user.
            string print = "";
            print += $"Enter 1 to add an object.\n";
            print += $"Enter 2 to update an object.\n";
            print += $"Enter 3 to print an object according to the id.\n";
            print += $"Enter 4 to print the whole list of an object.\n";
            print += $"Enter 5 to end the program.\n";
            Console.WriteLine(print);
            MainQuastions choice;
            int input = 0;
            double inputDouble = 0;
            Location location = new();
            MainQuastions.TryParse(Console.ReadLine(), out choice);//cin the useres choice
            try
            {
                if ((int)choice < 1 || (int)choice > 5)
                    throw new InvalidInputException("The input is incurrect");
                while (choice != MainQuastions.EndTheProgram)//goes till the user enters 5-end the program
                {
                    switch (choice)
                    {
                        case MainQuastions.AddAnObject: 
                            //It prints the add options to the user.
                            string print1 = "";
                            print1 += $"Enter what would you like to add:\n";
                            print1 += $"Enter 1 to add a base station.\n";
                            print1 += $"Enter 2 to add a drone.\n";
                            print1 += $"Enter 3 to add a customer.\n";
                            print1 += $"Enter 4 to add a parcel.\n";
                            Console.WriteLine(print1);
                            AddAnObject ChoiceAdd;
                            AddAnObject.TryParse(Console.ReadLine(), out ChoiceAdd);//cin the useres choice
                                switch (ChoiceAdd)
                                {
                                    //a case to add a baseStation
                                    case AddAnObject.AddABaseStation:
                                        BaseStation newStation = new();
                                        Console.Write("Enter Id (4 digits): ");
                                        int.TryParse(Console.ReadLine(), out input);
                                        newStation.Id = input;
                                        Console.Write("Enter name: ");
                                        newStation.Name = Console.ReadLine();
                                        newStation.Name = Console.ReadLine();
                                        Console.Write("Enter longitude: ");
                                        double.TryParse(Console.ReadLine(), out inputDouble);
                                        newStation.BaseStationLocation.Longitude = inputDouble;
                                        Console.Write("Enter Latitude: ");
                                        double.TryParse(Console.ReadLine(), out inputDouble);
                                        newStation.BaseStationLocation.Latitude = inputDouble;
                                        Console.Write("Enter amount of charge slots in new station : ");
                                        int.TryParse(Console.ReadLine(), out input);
                                        newStation.EmptyCharges = input;
                                        obj.AddBaseStation(newStation);
                                        break;
                                    //a case to add a drone
                                    case AddAnObject.AddADrone:
                                        Drone newDrone = new();
                                        Console.Write("Enter Id (6 digits): ");
                                        int.TryParse(Console.ReadLine(), out input);
                                        newDrone.Id = input;
                                        Console.WriteLine("Enter weight (light=0/inbetween=1/heavy=2)");
                                        int.TryParse(Console.ReadLine(), out input);
                                        newDrone.MaxWeight = (WeightCategories)input;
                                        Console.Write("Enter the drones model: ");
                                        newDrone.Model = Console.ReadLine();
                                        Console.Write("Enter the station to put the drone in for first charging: ");
                                        int.TryParse(Console.ReadLine(), out input);
                                        obj.AddDrone(newDrone, input);
                                        break;
                                    //a case to add a customer and get the details from the user.
                                    case AddAnObject.AddACustomer:
                                        Customer newCustomer = new Customer();//creats a new customer.
                                        Console.WriteLine("Enter Id (9 digits): ");
                                        int idCustomer;
                                        int.TryParse(Console.ReadLine(), out idCustomer);//cin the id of the new customer
                                        newCustomer.Id = idCustomer;//updates the new customers id
                                        Console.WriteLine("Enter Name: ");
                                        string name = Console.ReadLine();//cin the name of the new customer
                                        newCustomer.Name = name;//updates the new customers name
                                        Console.WriteLine("Enter Phone: ");
                                        string phone = Console.ReadLine();//cin the phone of the new customer
                                        newCustomer.Phone = phone;//updates the new customers phone
                                        Console.WriteLine("Enter Longitude: ");
                                        double.TryParse(Console.ReadLine(), out inputDouble);
                                        newCustomer.CustomerLocation.Longitude = inputDouble;
                                        Console.WriteLine("Enter Latitude: ");
                                        double.TryParse(Console.ReadLine(), out inputDouble);
                                        newCustomer.CustomerLocation.Latitude=inputDouble;
                                        obj.AddCustomer(newCustomer);
                                        break;
                                    //a case to add a parcel and get the details from the user.
                                    case AddAnObject.AddAParcel:
                                        Parcel newParcel = new Parcel();
                                        newParcel.Id = 0;//updates the new parcels id(it will change in the function)
                                        Console.WriteLine("Enter Sender id (4 digits): ");
                                        int senderId;
                                        int.TryParse(Console.ReadLine(), out senderId);//cin the sender id of the new parcel
                                        newParcel.Sender.Id = senderId;//updates the new parcels sender id
                                        Console.WriteLine("Enter Target id (4 digits): ");
                                        int targetId;
                                        int.TryParse(Console.ReadLine(), out targetId);//cin the target id of the new parcel
                                        newParcel.Recepter.Id = targetId;//updates the new parcels target id
                                        Console.WriteLine("Enter Weight (light=0/inbetween=1/heavy=2): ");
                                        int weight;
                                        int.TryParse(Console.ReadLine(), out weight);//cin the weight of the new parcel
                                        newParcel.Weight = (WeightCategories)weight;//updates the new parcels weight
                                        Console.WriteLine("Enter Priority (regular=0/fast=1/urgent=2): ");
                                        int proiority;
                                        int.TryParse(Console.ReadLine(), out proiority);//cin the priority of the new parcel
                                        newParcel.Priority = (Priorities)proiority;//updates the new parcels proiority
                                        obj.AddParcel(newParcel);
                                        break;
                                } 
                            break;
                        case MainQuastions.UpdateAnObject:
                            print1 = "";
                            print1 += $"Enter what would you like to add:\n";
                            print1 += $"Enter 1 to update a drone.\n";
                            print1 += $"Enter 2 to update a base station .\n";
                            print1 += $"Enter 3 to update a customer.\n";
                            print1 += $"Enter 4 to send a drone to a charger.\n";
                            print1 += $"Enter 5 to release a drone from a charger.\n";
                            print1 += $"Enter 6 to scheduled a drone to a parcel.\n";
                            print1 += $"Enter 7 to pickup a parcel by a drone.\n";
                            print1 += $"Enter 8 to deliver a parcel by a drone.\n";
                            Console.WriteLine(print1);
                            UpdateAnObject Choiceupdate;
                            UpdateAnObject.TryParse(Console.ReadLine(), out Choiceupdate);//cin the useres choice
                            switch (Choiceupdate)
                            {
                                case UpdateAnObject.updataADrone:
                                    break;
                                case UpdateAnObject.updataABaseStation:
                                    break;
                                case UpdateAnObject.updataACustomer:
                                    break;
                                case UpdateAnObject.SendADronToACharger:
                                    break;
                                case UpdateAnObject.FreeADronFromACharger:
                                    break;
                                case UpdateAnObject.ScheduledAParcelToADrone:
                                    break;
                                case UpdateAnObject.PickupAParcelByADrone:
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case MainQuastions.PrintAnObjectAccordingToTheId:
                            //It prints the prints options to the user.
                            String print3 = "";
                            print3 += $"Enter what would you like to print\n";
                            print3 += $"1 to present a station according to his id.\n";
                            print3 += $"2 to present a drone according to his id.\n";
                            print3 += $"3 to present a customer according to his id.\n";
                            print3 += $"4 to present a parcel according to his id.\n";
                            Console.WriteLine(print3);
                            PrintAnObjectAccordingToTheId choiceAccorIdPrint;
                            PrintAnObjectAccordingToTheId.TryParse(Console.ReadLine(), out choiceAccorIdPrint);//cin the useres choice
                            switch (choiceAccorIdPrint)
                            {
                                //in case you want to print a baseStation
                                case PrintAnObjectAccordingToTheId.PresentAStationAccordingToHisId:
                                    Console.WriteLine("Enter the base station id (4 digits)\n");
                                    int id;
                                    int.TryParse(Console.ReadLine(), out id);//cin the baseStations id from the user
                                    //sends to a function that finds the baseStation and prints the info
                                    Console.WriteLine(obj.GetBaseStation(id).ToString());
                                    break;
                                //in case you want to print a drone
                                case PrintAnObjectAccordingToTheId.PresentADroneAccordingToHisId:
                                    Console.WriteLine("Enter the drone id (6 digits)\n");
                                    int idDrone;
                                    int.TryParse(Console.ReadLine(), out idDrone);//cin the drones id from the user
                                    //sends to a function that finds the drone and prints the info
                                    Console.WriteLine(obj.GetDrone(idDrone).ToString());
                                    break;
                                //in case you want to print a customer
                                case PrintAnObjectAccordingToTheId.PesentACustomerAccordingToHisId:
                                    Console.WriteLine("Enter the customer id (9 digits)\n");
                                    int idCustomer;
                                    int.TryParse(Console.ReadLine(), out idCustomer);//cin the customers id from the user
                                    //sends to a function that finds the customer and prints the info
                                    Console.WriteLine(obj.GetCustomer(idCustomer).ToString());
                                    break;
                                //in case you want to print a parcel
                                case PrintAnObjectAccordingToTheId.PresentAParcelAccordingToHisId:
                                    Console.WriteLine("Enter the parcel id (4 digits)\n");
                                    int idParcel;
                                    int.TryParse(Console.ReadLine(), out idParcel);//cin the parcels id from the user
                                    //sends to a function that finds the parcel and prints the info
                                    Console.WriteLine(obj.GetParcel(idParcel).ToString());
                                    break;
                            }
                            break;
                        case MainQuastions.PrintTheWholeListOfAnObject:
                            //It prints the prints options to the user.
                            string print4 = "";
                            print4 += $"What list would you like to print ?\n";
                            print4 += $"Enter 1 to print all the base stations\n";
                            print4 += $"Enter 2 to print all the drone\n";
                            print4 += $"Enter 3 to print all the customer\n";
                            print4 += $"Enter 4 to print all the parcel\n";
                            print4 += $"Enter 5 to print all the parcels that weren't paired\n";
                            print4 += $"Enter 6 to print all the base station with available charges\n";
                            Console.WriteLine(print4);
                            PrintTheWholeListOfAnObject PrintChoice;
                            PrintTheWholeListOfAnObject.TryParse(Console.ReadLine(), out PrintChoice);//cin the useres choice
                            switch (PrintChoice)
                            {
                                //in case you want to print the whole baseStations
                                case PrintTheWholeListOfAnObject.PrintAllTheBaseStation:
                                    //creats a new list of baseStations and sends to a function that returns the list of the baseStations
                                    IEnumerable<BaseStationList> ActiveStations = obj.GetBaseStations();
                                    //goes through the list and prints the info of each one.
                                    foreach (var itBS in ActiveStations)
                                        Console.WriteLine(itBS.ToString());
                                    break;
                                //in case you want to print the whole drones
                                case PrintTheWholeListOfAnObject.PrintAllTheDrone:
                                    //creats a new list of drones and sends to a function that returns the list of the drones
                                    IEnumerable<DroneList> ActiveDrones = obj.GetDrones();
                                    //goes through the list and prints the info of each one.
                                    foreach (var itD in ActiveDrones)
                                        Console.WriteLine(itD.ToString());
                                    break;
                                //in case you want to print the whole customers
                                case PrintTheWholeListOfAnObject.PrintAllTheCustomer:
                                    //creats a new list of customers and sends to a function that returns the list of the customers
                                    IEnumerable<CustomerList> ActiveCustomers = obj.GetCustomers();
                                    //goes through the list and prints the info of each one.
                                    foreach (var itC in ActiveCustomers)
                                        Console.WriteLine(itC.ToString());
                                    break;
                                //in case you want to print the whole parcels
                                case PrintTheWholeListOfAnObject.PrintAllTheParcel:
                                    //creats a new list of parcels and sends to a function that returns the list of the parcels
                                    IEnumerable<ParcelList> ActiveParcels = obj.GetParcels();
                                    //goes through the list and prints the info of each one.
                                    foreach (var itP in ActiveParcels)
                                        Console.WriteLine(itP.ToString());
                                    break;
                                //in case you want to print the parcels that were not paired to a drone.
                                case PrintTheWholeListOfAnObject.printAllTheParcelsThatWerentPaired:
                                    //creats a new list of parcels and sends to a function that returns the list of the parcels that were not paired to a drone.
                                    IEnumerable<ParcelList> Parcels = obj.GetParcelThatWerenNotPaired();
                                    //goes through the list and prints the info of each one.
                                    foreach (var itP in Parcels)
                                        Console.WriteLine(itP.ToString());
                                    break;
                                //in case you want to print the baseStations with available chargers
                                case PrintTheWholeListOfAnObject.PrintAllTheBaseStationWithAvailableCharges:
                                    //creats a new list of parcels and sends to a function that returns the list of the baseStations with available chargers
                                    IEnumerable<BaseStationList> Stations = obj.GetBaseStationWithAvailableCharges();
                                    //goes through the list and prints the info of each one.
                                    foreach (var itS in Stations)
                                        Console.WriteLine(itS.ToString());
                                    break;
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}