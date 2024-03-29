﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using BO;

namespace PL 
{
    public enum DroneStatus { Available, InFix, Delivery, All };
    /// <summary>
    /// Interaction logic for WindowDrones.xaml
    /// </summary>
    public partial class WindowDrones : Window
    {
        BlApi.IBL ibl;
        public ObservableCollection<DroneList> Drones;
        private bool _close { get; set; } = false;
        public DroneList selectedDrone { get; set; } = new();

        /// <summary>
        /// the constractor of the window
        /// </summary>
        /// <param name="bl"> the accses to the fileds in IBL</param>
        public WindowDrones(BlApi.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            Drones = new ObservableCollection<DroneList>();
            IEnumerable<DroneList> drones = ibl.GetDrones();
            drones.OrderBy(item => item.Id);
            foreach (var item in drones)Drones.Add(item);
            DronesListView.ItemsSource = Drones;//to show all the customers
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            StatusSelector.SelectedIndex = 3;//no filter
        }

        /// <summary>
        /// filters the list of drones that was enterd according to what was filtterd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector();//to show the list according to the filter that was enterd
        }
        /// <summary>
        /// shows the list according to the filter that the user disided
        /// </summary>
        public void Selector()
        {
            DroneStatus dStatus = (DroneStatus)StatusSelector.SelectedItem;
            DronesListView.ItemsSource = null;
            //if no filter was chosen-show the all list
            if (dStatus == DroneStatus.All)
                DronesListView.ItemsSource = Drones;
            //if only he wants to filter the statuse category
            if (dStatus != DroneStatus.All)
                DronesListView.ItemsSource = Drones.Where(item => item.Status == (BO.DroneStatus)StatusSelector.SelectedItem);
            DronesListView.Items.Refresh();
        }

        /// <summary>
        /// to add a new drone to the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new WindowDrone(ibl, this).Show();//t oadd a new drone to the parcel
        }

        public void MyRefresh()
        {
            IEnumerable<DroneList> drones = ibl.GetDrones();
            drones.OrderBy(item => item.Id);
            Drones = new ObservableCollection<DroneList>();
            foreach (var item in drones) Drones.Add(item);
            DronesListView.ItemsSource = Drones;//to show all the customers
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
            Selector();
        }

        /// <summary>
        /// t opresent the drone that the mous double clicked  on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DronesListView_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            selectedDrone = (DroneList)DronesListView.SelectedItem;//the drone that the mous double clicked on
            new WindowDrone(ibl, this, 0).Show();//to show the all the details of the drone and to be able to updae him
        }

        /// <summary>
        /// to close the window of the drones list 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseWDS_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
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
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Selector();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement framework = sender as FrameworkElement;
            selectedDrone = framework.DataContext as DroneList;
            try
            {
                ibl.DeleteDrone(selectedDrone.Id);
                Drones.Remove(selectedDrone);
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been deleted successfully \n" + selectedDrone.ToString());
            }
            catch (BO.ItemIsDeletedException ex)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone was not deleted \n" + selectedDrone.ToString());
            }

        }
    }
}
