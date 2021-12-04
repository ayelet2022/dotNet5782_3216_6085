using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for WindowDrone.xaml
    /// </summary>
    public partial class WindowDrone : Window
    {
        Drone mainDrone = new();
        IBL.IBL ibl;
        private WindowDrones windowDrones;
         
        public WindowDrone(IBL.IBL bl, WindowDrones _windowDrones)
        {
            InitializeComponent();
            ibl = bl;
            windowDrones = _windowDrones;
            add.Visibility = Visibility.Visible;
            DataContext = mainDrone;
            weightA.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            IdStation.ItemsSource = bl.GetBaseStations().Select(s => s.Id);
        }
        public WindowDrone(Drone drone, IBL.IBL bl, WindowDrones _windowDrones)
        {
            ibl = bl;
            InitializeComponent();
            windowDrones = _windowDrones;
            statusAC.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            weightAC.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            Actions.Visibility=Visibility.Visible;
            DataContext = drone;
            mainDrone = drone;
            if (mainDrone.Status == DroneStatus.available)
            {
                ChargeDrone.Visibility = Visibility.Visible;
                ChangeStatusDrone.Visibility = Visibility.Visible;
                ChargeDrone.Content = "Send drone to charging";
                ChangeStatusDrone.Content = "Send drone to delievery";
            }
            if (mainDrone.Status == DroneStatus.inFix)
            {
                ChargeDrone.Content = "Release drone from charging";
                FDL.Visibility = Visibility.Visible;
                IdStationCharge.Visibility = Visibility.Visible;
                ChangeStatusDrone.Visibility = Visibility.Hidden;
            }
            else
            {
                if (mainDrone.Status == DroneStatus.delivery && mainDrone.ParcelInTransfer.StatusParcel == false)
                {
                    ChargeDrone.Visibility = Visibility.Hidden;
                    ChangeStatusDrone.Content = "Pick up parcel";
                }
                if (mainDrone.Status == DroneStatus.delivery && mainDrone.ParcelInTransfer.StatusParcel == true)
                {
                    ChargeDrone.Visibility = Visibility.Hidden;
                    ChangeStatusDrone.Content = "Supply parcel";
                }
            }
        }
        private void addButten_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ibl.AddDrone(mainDrone, (int)IdStation.SelectedItem);
                windowDrones.Drones.Add(ibl.GetDrones().First(i => i.Id == mainDrone.Id));
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been added successfully \n" + mainDrone.ToString());
                Close();
            }
            catch(FailedToAddException ex)
            {
                MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
            catch(IBL.BO.InvalidInputException ex)
            {
                MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ibl.UpdateDrone(ibl.GetDrone(mainDrone.Id), ModelAc.Text);
                int index = windowDrones.Drones.ToList().FindIndex(item => item.Id == mainDrone.Id);
                DroneList droneList = new();
                mainDrone.CopyPropertiesTo(droneList);
                droneList.DroneLocation= mainDrone.DroneLocation;
                if (mainDrone.ParcelInTransfer == null)
                    droneList.NumOfParcelOnTheWay = 0;
                else
                    droneList.NumOfParcelOnTheWay = mainDrone.ParcelInTransfer.Id;
                windowDrones.Drones.Remove(droneList);
                windowDrones.Drones.Add(droneList);
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been updateded successfully \n" + mainDrone.ToString());
            }
            catch (FailToUpdateException ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }

        }
        private void ChangeStatusDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainDrone.Status == DroneStatus.available)
                    ibl.ScheduledAParcelToADrone(mainDrone.Id);
                if (mainDrone.Status == DroneStatus.delivery && mainDrone.ParcelInTransfer.StatusParcel == false)
                    ibl.PickUpParcel(mainDrone.Id);
                if (mainDrone.Status == DroneStatus.delivery && mainDrone.ParcelInTransfer.StatusParcel == true)
                    ibl.DeliverParcel(mainDrone.Id);
                int index = windowDrones.Drones.ToList().FindIndex(item => item.Id == mainDrone.Id);
                DroneList droneList = new();
                mainDrone.CopyPropertiesTo(droneList);
                droneList.DroneLocation = mainDrone.DroneLocation;
                if (mainDrone.ParcelInTransfer == null)
                    droneList.NumOfParcelOnTheWay = 0;
                else
                    droneList.NumOfParcelOnTheWay = mainDrone.ParcelInTransfer.Id;
                windowDrones.Drones.ToList()[index] = droneList;
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been updateded successfully \n" + mainDrone.ToString());
            }
            catch (FailToUpdateException ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        private void ChargeDrone_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                if ((string)ChargeDrone.Content == "Send drone to charging")
                    ibl.SendDroneToCharging(mainDrone.Id);
                if ((string)ChargeDrone.Content == "Release drone from charging")
                    ibl.FreeDroneFromeCharger(mainDrone.Id, 90);
                int index = windowDrones.Drones.ToList().FindIndex(item => item.Id == mainDrone.Id);
                DroneList droneList = new();
                mainDrone.CopyPropertiesTo(droneList);
                droneList.DroneLocation = mainDrone.DroneLocation;
                if (mainDrone.ParcelInTransfer == null)
                    droneList.NumOfParcelOnTheWay = 0;
                else
                    droneList.NumOfParcelOnTheWay = mainDrone.ParcelInTransfer.Id;
                windowDrones.Drones.ToList()[index] = droneList;
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been updateded successfully \n" + mainDrone.ToString());
            }
            catch (FailToUpdateException ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }
        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
