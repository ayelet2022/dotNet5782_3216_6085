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
        BlApi.IBL ibl;
        private WindowParcels windowParcels;
        private WindowCustomer windowCustomer { get; set; }
        private int index { get; set; }
        private int customerId { get; set; }

        #region CUNSTRACTORS
        /// <summary>
        /// a constructer of parcel (when we come from a customer window)
        /// </summary>
        /// <param name="bl">the accses to bl</param>
        /// <param name="_windowCustomer">the window we came from</param>
        /// <param name="id">the id of the parcel</param>
        /// <param name="_index">the index wehre the parcel was</param>
        /// <param name="_customerId">the customer that has the parcel</param>
        public WindowParcel(BlApi.IBL bl, WindowCustomer _windowCustomer, int id, int _index, int _customerId) : this(bl, null, id)
        {
            windowCustomer = _windowCustomer;
            index = _index;
            customerId = _customerId;
        }
        /// <summary>
        /// constructer-adds a new Parcel   
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowParcels">the window with all the Parcel</param>
        public WindowParcel(BlApi.IBL bl, WindowParcels _windowParcels)
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
            addButton.Content = "Add a Parcel";
            addButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// constructer-updates the drone that the mouse clicked twice on
        /// </summary>
        /// <param name="bl">the accses to IBL</param>
        /// <param name="_windowParcels">the window with all the drones</param>
        /// <param name="i">the diffrence between the constractor of add to the constractor of update</param>
        public WindowParcel(BlApi.IBL bl, WindowParcels _windowParcels, int i = 0)
        {
            ibl = bl;
            InitializeComponent();
            windowParcels = _windowParcels;
            UpdateGride.Visibility = Visibility.Visible;
            if (i == 0)
                mainParcel = ibl.GetParcel(windowParcels.selectedParcel.Id);//returnes the drone that the mouce clicked twise on
            else
                mainParcel = ibl.GetParcel(i);
            DataContext = mainParcel;//to connect between the text box and the data
            if (mainParcel.Scheduled == null)
                addButton.Visibility = Visibility.Collapsed;
            if (mainParcel.Scheduled != null)//if the parcel  has a drone 
                DroneInParcel.Visibility = Visibility.Visible;//show the gride of the parcels drone
        }
        #endregion


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

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mainParcel.Sender.Id == default || mainParcel.Recepter.Id == default)
                    throw new MissingInfoException("No information entered for this drone");
                ibl.AddParcel(mainParcel);
                int id = ibl.GetParcels().Last().Id;
                mainParcel = ibl.GetParcel(id);
                if (windowParcels != null)
                    windowParcels.Parcels.Add(ibl.GetParcels().First(item => item.Id == mainParcel.Id));
                else
                {
                    ParcelInCustomer parcelInCustomer = new()
                    {
                        Id = mainParcel.Id,
                        Weight = mainParcel.Weight,
                        Priority = mainParcel.Priority,
                        Status =BO.ParcelStatus.creat,
                        SenderOrRecepter =new()
                        {
                            Id = mainParcel.Sender.Id,
                            Name = mainParcel.Sender.Name
                        }
                    };
                    windowCustomer.ParcelFromCusW.Add(parcelInCustomer);
                }
                MessageBoxResult messageBoxResult1 = MessageBox.Show("The parcel has been added successfully \n" + mainParcel.ToString());
                _close = true;
                Close();
            }
            #region CATCH
            catch (FailedToAddException ex)
            {
                var message = MessageBox.Show("Failed to add the parcel: " + ex.GetType().Name + "\n" + ex.Message + "\n" + "Woul'd you like to try agein?\n", "Error",
                    MessageBoxButton.YesNo, MessageBoxImage.Error);
                switch (message)
                {
                    case MessageBoxResult.Yes:
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
            #endregion
        }
        public void MyRefresh()
        {
            mainParcel = ibl.GetParcel(mainParcel.Id);
            DataContext = mainParcel;
            windowParcels.MyRefresh();
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
            MessageBoxResult messageBoxResult;
            Customer customer = ibl.GetCustomer(mainParcel.Sender.Id);
            if (ibl.IsActive(customer.Id))
                new WindowCustomer(ibl, this, mainParcel.Sender.Id, mainParcel).Show();
            else
                messageBoxResult = MessageBox.Show("The customer was deleted \n" + mainParcel.Sender.ToString());
        }

        private void RecepterButten_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult;
            Customer customer = ibl.GetCustomer(mainParcel.Recepter.Id);
            if (ibl.IsActive(customer.Id))
                new WindowCustomer(ibl, this, mainParcel.Recepter.Id, mainParcel).Show();
            else
                messageBoxResult = MessageBox.Show("The customer was deleted \n" + mainParcel.Recepter.ToString());
        }
        private void Parcelsdrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ibl.GetDrone(mainParcel.ParecelDrone.Id);
                WindowDrones windowDrones = new WindowDrones(ibl);
                new WindowDrone(ibl, windowDrones, mainParcel.ParecelDrone.Id).Show();
            }
            catch (NotFoundInputException ex)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone was deleted \n" + mainParcel.ParecelDrone.ToString());
            }
        }
    }
}
