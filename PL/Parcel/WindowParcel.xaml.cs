﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Text.RegularExpressions;
using BO;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for WindowParcel.xaml
    /// </summary>
    public partial class WindowParcel : Window
    {
        Parcel mainParcel = new();

        private bool _close { get; set; } = false;
        BL.BL ibl;
        private WindowParcels windowParcels;
        StatusWeightAndPriorities _StatusWeightAndPriorities;

        /// <summary>
        /// constructer-adds a new Parcel   
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowParcels">the window with all the Parcel</param>
        public WindowParcel(BL.BL bl, WindowParcels _windowParcels)
        {
            InitializeComponent();
            ibl = bl;
            windowParcels = _windowParcels;
            AddGrid.Visibility = Visibility.Visible;
            DataContext = mainParcel;
            mainParcel.Sender = new();
            mainParcel.Recepter = new();
            WeightComboBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            PriorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            addDeletDeliverPickUpButton.Content = "Add the Parcel";
            addDeletDeliverPickUpButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// constructer-updates the drone that the mouse clicked twice on
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowParcels">the window with all the drones</param>
        /// <param name="i">the diffrence between the constractor of add to the constractor of update</param>
        public WindowParcel(BL.BL bl, WindowParcels _windowParcels, int i = 0)
        {
            ibl = bl;
            InitializeComponent();
            windowParcels = _windowParcels;
            ActionseGrid.Visibility = Visibility.Visible;
            if (i == 0)
                mainParcel = ibl.GetParcel(windowParcels.selectedParcel.Id);//returnes the drone that the mouce clicked twise on
            else
                mainParcel = ibl.GetParcel(i);
            DataContext = mainParcel;//to connect between the text box and the data
            if (mainParcel.Scheduled == null)
            {
                addDeletDeliverPickUpButton.Content = "Delet the parcel";
                addDeletDeliverPickUpButton.Visibility = Visibility.Visible;
            }
            if (mainParcel.Scheduled != null)//if the parcel  has a drone 
            {
                if (mainParcel.Delivered == null && mainParcel.PickedUp == null)
                {
                    addDeletDeliverPickUpButton.Content = "Pick up";
                }
                if (mainParcel.Delivered == null && mainParcel.PickedUp != null)
                {
                    addDeletDeliverPickUpButton.Content = "Deliver";
                }

                DroneInParcel.Visibility = Visibility.Visible;//show the gride of the parcels drone
            }
        }

        /// <summary>
        /// to close the drones window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }
        /// <summary>
        /// to only allow to enter int in a text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void addDeletDeliverPickUpButton_Click(object sender, RoutedEventArgs e)
        {
            switch (addDeletDeliverPickUpButton.Content)
            {
                case "Add the Parcel":
                    try
                    {
                        if (mainParcel.Id == default || mainParcel.Sender == default || mainParcel.Recepter == default)
                            throw new MissingInfoException("No information entered for this drone");
                        ibl.AddParcel(mainParcel);
                        _StatusWeightAndPriorities.status = BO.ParcelStatus.creat;
                        _StatusWeightAndPriorities.priorities = mainParcel.Priority;
                        _StatusWeightAndPriorities.weight = mainParcel.Weight;
                        mainParcel = ibl.GetParcel(mainParcel.Id);
                        if (windowParcels.Parcels.ContainsKey(_StatusWeightAndPriorities))
                            windowParcels.Parcels[_StatusWeightAndPriorities].Add(ibl.GetParcels().First(i => i.Id == mainParcel.Id));
                        else
                            windowParcels.Parcels.Add(_StatusWeightAndPriorities, ibl.GetParcels().Where(i => i.Id == mainParcel.Id).ToList());
                        MessageBoxResult messageBoxResult = MessageBox.Show("The parcel has been added successfully \n" + mainParcel.ToString());
                        _close = true;
                        Close();
                    }
                    catch (FailedToAddException ex)
                    {
                        var message = MessageBox.Show("Failed to add the parcel: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                            MessageBoxButton.YesNo, MessageBoxImage.Error);
                        switch (message)
                        {
                            case MessageBoxResult.Yes:
                                IDBoxA.Text = "";
                                SenderBoxA.Text = "";
                                RecepterBoxA.Text = "";
                                break;
                            case MessageBoxResult.No:
                                _close = true;
                                Close();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (InvalidInputException ex)
                    {
                        var message = MessageBox.Show("Failed to add the parcel: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                            MessageBoxButton.YesNo, MessageBoxImage.Error);
                        switch (message)
                        {
                            case MessageBoxResult.Yes:
                                IDBoxA.Text = "";
                                break;
                            case MessageBoxResult.No:
                                _close = true;
                                Close();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (MissingInfoException ex)
                    {
                        var message = MessageBox.Show("Failed to add the parcel: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                            MessageBoxButton.YesNo, MessageBoxImage.Error);
                        switch (message)
                        {
                            case MessageBoxResult.Yes:
                                break;
                            case MessageBoxResult.No:
                                _close = true;
                                Close();
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "Delet the parcel":
                    ibl.DeletParcel(mainParcel);
                    break;
                case "Pick up":
                    try
                    {
                        ibl.PickUpParcel(mainParcel.ParecelDrone.Id);
                    }
                    catch (FailToUpdateException ex)
                    {
                        var message = MessageBox.Show("Failed to pick up the parcel: " + ex.GetType().Name + "\n" + ex.Message + "\n", "Error",
                            MessageBoxButton.YesNo, MessageBoxImage.Error);
                        switch (message)
                        {
                            case MessageBoxResult.Yes:
                                IDBoxA.Text = "";
                                SenderBoxA.Text = "";
                                RecepterBoxA.Text = "";
                                break;
                            case MessageBoxResult.No:
                                _close = true;
                                Close();
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case "Deliver":
                    try
                    {
                        ibl.DeliverParcel(mainParcel.ParecelDrone.Id);
                    }
                    catch (FailToUpdateException ex)
                    {
                        var message = MessageBox.Show("Failed to deliver the parcel: " + ex.GetType().Name + "\n" + ex.Message + "\n", "Error",
                            MessageBoxButton.YesNo, MessageBoxImage.Error);
                        switch (message)
                        {
                            case MessageBoxResult.Yes:
                                IDBoxA.Text = "";
                                SenderBoxA.Text = "";
                                RecepterBoxA.Text = "";
                                break;
                            case MessageBoxResult.No:
                                _close = true;
                                Close();
                                break;
                            default:
                                break;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// to not be able to close the window with the x on the top
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClose(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force the window to close");
            }
        }

        private void SenderButten_Click(object sender, RoutedEventArgs e)
        {
            WindowCustomers windowCustomers = new WindowCustomers(ibl);
            new WindowCustomer(ibl, windowCustomers, mainParcel.Sender.Id).Show();
        }

        private void RecepterButten_Click(object sender, RoutedEventArgs e)
        {
            WindowCustomers windowCustomers = new WindowCustomers(ibl);
            new WindowCustomer(ibl, windowCustomers, mainParcel.Recepter.Id).Show();
        }
        private void Parcelsdrone_Click(object sender, RoutedEventArgs e)
        {
            WindowDrones windowDrones = new WindowDrones(ibl);
            new WindowDrone(ibl, windowDrones, mainParcel.ParecelDrone.Id);
        }
    }
}
