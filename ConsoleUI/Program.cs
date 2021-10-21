using System;
using DAL;
namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            string Choice= Console.ReadLine();
            switch (Choice)
            {
                case "add":
                    
                    break;
                case "Update":
                    
                    break;
                case "present_according_to_the_id":
                    
                    break;
                case "present_the_whole_list":
                    Choice = Console.ReadLine();
                    switch (Choice)
                    {
                        case "BaseStation":

                            break;
                        case "Drone":

                            break;
                        case "Customer":

                            break;
                        case "Parcel":

                            break;
                        case "Parcel_that_weren't_paired":
                            break;
                        case "BaseStation_with_available_charges":
                            break;
                    }
                    break;
                case "Exit":
                    break;
            }
        }
    }
}
