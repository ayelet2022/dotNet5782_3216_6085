using System;
using BL;
using IBL.BO;
namespace ConsoleUI_BL
{
    public enum MainQuastions { AddAnObject = 1, UpdateAnObject, PrintAnObjectAccordingToTheId, PrintTheWholeListOfAnObject, EndTheProgram };
    public enum AddAnObject { AddABaseStation = 1, AddADrone, AddACustomer, AddAParcel };
    public enum UpdateAnObject
    { DronToAParcel = 1, PickupAPercelFromTheDron, DeliverAPercelToACustomer, SendADronToAChargeStation, FreeADronFromACharge };
    public enum PrintAnObjectAccordingToTheId
    { PresentAStationAccordingToHisId = 1, PresentADroneAccordingToHisId, PesentACustomerAccordingToHisId, PresentAParcelAccordingToHisId };
    public enum PrintTheWholeListOfAnObject
    { PrintAllTheBaseStation = 1, PrintAllTheDrone, PrintAllTheCustomer, PrintAllTheParcel, printAllTheParcelsThatWerentPaired, PrintAllTheBaseStationWithAvailableCharges }
    class Program
    {
        //static IBL obgIBL = new BL();
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
            MainQuastions.TryParse(Console.ReadLine(), out choice);//cin the useres choice
            try
            {
                if ((int)choice < 1 || (int)choice > 5)
                    throw new MyException("The input is incurrect");
                while (choice != MainQuastions.EndTheProgram)//goes till the user enters 5-end the program
                {
                    switch (choice)
                    {
                        case MainQuastions.AddAnObject:
                            break;
                        case MainQuastions.UpdateAnObject:
                            break;
                        case MainQuastions.PrintAnObjectAccordingToTheId:
                            break;
                        case MainQuastions.PrintTheWholeListOfAnObject:
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