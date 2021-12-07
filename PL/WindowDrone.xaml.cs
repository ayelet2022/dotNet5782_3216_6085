﻿using System;
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
            AddGrid.Visibility = Visibility.Visible;
            DataContext = mainDrone;
            weightA.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            IdStation.ItemsSource = bl.GetBaseStations().Select(s => s.Id);
        }
        public WindowDrone(IBL.IBL bl, WindowDrones _windowDrones, int i=0)
        {
            ibl = bl;
            InitializeComponent();
            windowDrones = _windowDrones;
            ActionseGrid.Visibility=Visibility.Visible;
            Buttens.Visibility = Visibility.Visible;
            mainDrone = ibl.GetDrone(windowDrones.selectedDrone.Id);
            DataContext = mainDrone;
            if(mainDrone.ParcelInTransfer!=null)
            {
                ParcelIT.Visibility = Visibility.Visible;
            }
            if (mainDrone.Status == (IBL.BO.DroneStatus)DroneStatus.Available)
            {
                ChargeDrone.Visibility = Visibility.Visible;
                ChangeStatusDrone.Visibility = Visibility.Visible;
                ChargeDrone.Content = "Send drone to charging";
                ChangeStatusDrone.Content = "Send drone to delievery";
            }
            if (mainDrone.Status == (IBL.BO.DroneStatus)DroneStatus.InFix)
            {
                ChargeDrone.Content = "Release drone from charging";
                ChangeStatusDrone.Visibility = Visibility.Hidden;
            }
            else
            {
                if (mainDrone.Status == (IBL.BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == false)
                {
                    ChargeDrone.Visibility = Visibility.Hidden;
                    ChangeStatusDrone.Content = "Pick up parcel";
                }
                if (mainDrone.Status == (IBL.BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == true)
                {
                    ChargeDrone.Visibility = Visibility.Hidden;
                    ChangeStatusDrone.Content = "Supply parcel";
                }
            }
        }
        private void AddButten_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainDrone.Id < 100000 || mainDrone.Id > 999999)
                    throw new InvalidInputException($"The drones id: {mainDrone.Id} is incorrect, the drone was not added.\n");
                if (mainDrone.Model == null || mainDrone.Model == "")
                    throw new InvalidInputException($"The drones model: {mainDrone.Model} is incorrect, the drone was not added.\n");
                if (mainDrone.Id == default)
                    throw new MissingInfoException("No information entered for this drone");
                if (IdStation.SelectedItem == null)
                    throw new MissingInfoException("No station was entered for this drone");
                ibl.AddDrone(mainDrone, (int)IdStation.SelectedItem);
                windowDrones.Drones.Add(ibl.GetDrones().First(i => i.Id == mainDrone.Id));
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been added successfully \n" + mainDrone.ToString());
                Close();
            }
            catch(FailedToAddException ex)
            {
                var message = MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message+"\n"+"Woul'd you like to try agein?\n","Error",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                        IdBoxA.Text = "";
                        ModelBoxA.Text = "";
                        break;
                    case MessageBoxResult.No:
                        Close();
                        break;
                    default:
                        break;
                }   
            }
            catch (InvalidInputException ex)
            {
                var message = MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                        if (IdBoxA.Text.Length != 6)
                            IdBoxA.Text = "";
                        break;
                    case MessageBoxResult.No:
                        Close();
                        break;
                    default:
                        break;
                }
            }
            catch(MissingInfoException ex)
            {
                var message = MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (message)
                {
                    case MessageBoxResult.Yes:
                            break;
                    case MessageBoxResult.No:
                        Close();
                        break;
                    default:
                        break;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ModelBoxAc.Text == null)
                    throw new MissingInfoException($"The drones model: {mainDrone.Model} is incorrect, the drone was not added.\n");
                ibl.UpdateDrone(mainDrone, ModelBoxAc.Text);
                windowDrones.selectedDrone.Model = ModelBoxAc.Text;
                int index = windowDrones.Drones.ToList().FindIndex(item => item.Id == mainDrone.Id);
                windowDrones.Drones[index] = windowDrones.selectedDrone;
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been updateded successfully \n" + mainDrone.ToString());
            }
            catch (FailToUpdateException ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
            catch (MissingInfoException ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }
        private void ChangeStatusDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainDrone.Status == (IBL.BO.DroneStatus)DroneStatus.Available)
                {
                    ibl.ScheduledAParcelToADrone(mainDrone.Id);
                    ChargeDrone.Visibility = Visibility.Hidden;
                    ChangeStatusDrone.Content = "Pick up parcel";
                }
                if (mainDrone.Status == (IBL.BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == false)
                {
                    ibl.PickUpParcel(mainDrone.Id);
                    ChargeDrone.Visibility = Visibility.Hidden;
                    ChangeStatusDrone.Content = "Supply parcel";
                }
                if (mainDrone.Status == (IBL.BO.DroneStatus)DroneStatus.Delivery && mainDrone.ParcelInTransfer.StatusParcel == true)
                {
                    ibl.DeliverParcel(mainDrone.Id);
                    ChargeDrone.Visibility = Visibility.Visible;
                    ChargeDrone.Content = "Send drone to charging";
                    ChangeStatusDrone.Content = "Send drone to delievery";
                }
                int index = windowDrones.Drones.ToList().FindIndex(item => item.Id == mainDrone.Id);
                windowDrones.Drones[index] = ibl.GetDrones().First(item => item.Id == mainDrone.Id);
                mainDrone = ibl.GetDrone(mainDrone.Id);
                DataContext = mainDrone;
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
                {
                    ibl.SendDroneToCharging(mainDrone.Id);
                    ChangeStatusDrone.Visibility = Visibility.Hidden;
                    ChargeDrone.Content = "Release drone from charging";
                }
                else
                    if ((string)ChargeDrone.Content == "Release drone from charging")
                {
                    ibl.FreeDroneFromeCharger(mainDrone.Id);
                    ChangeStatusDrone.Visibility = Visibility.Visible;
                    ChargeDrone.Content = "Send drone to charging";
                    ChangeStatusDrone.Content = "Send drone to delievery";
                }
                int index = windowDrones.Drones.ToList().FindIndex(item => item.Id == mainDrone.Id);
                windowDrones.Drones[index] = ibl.GetDrones().First(item => item.Id == mainDrone.Id);
                mainDrone = ibl.GetDrone(mainDrone.Id);
                DataContext = mainDrone;
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been updateded successfully \n" + mainDrone.ToString());
            }
            catch (FailToUpdateException ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void IdBoxA_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IdBoxA.Text.Length < 6 && IdBoxA.Text.Length > 1)
                IdBoxA.BorderBrush = new SolidColorBrush(Colors.Red);
            else
                IdBoxA.BorderBrush = new SolidColorBrush(Colors.Black);
        }
    }
}
