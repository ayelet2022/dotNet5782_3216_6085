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
        DroneList mainDroneList = new();
        IBL.IBL ibl;
        public WindowDrone(IBL.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            add.Visibility=Visibility.Visible;
            weightA.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            IdStation.ItemsSource = bl.GetBaseStations().Select(s => s.Id);


        }
        public WindowDrone(DroneList droneList)
        {
            InitializeComponent();
            statusAC.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            weightAC.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            Actions.Visibility=Visibility.Visible;
            DataContext = droneList;
            mainDroneList = droneList;
            if (mainDroneList.Status == DroneStatus.available)
            {
                ChargeDrone.Visibility = Visibility.Visible;
                ChangeStatusDrone.Visibility = Visibility.Visible;
                ChargeDrone.Content = "Send drone to charging";
                ChangeStatusDrone.Content = "Send drone to delievery";
            }
            if (mainDroneList.Status == DroneStatus.inFix)
            {
                ChargeDrone.Content = "Release drone from charging";
                ChangeStatusDrone.Visibility = Visibility.Hidden;
            }
            else
            {
                if (mainDroneList.Status == DroneStatus.delivery && ibl.GetParcel(mainDroneList.NumOfParcelOnTheWay).PickedUp == null)
                {
                    ChargeDrone.Visibility = Visibility.Hidden;
                    ChangeStatusDrone.Content = "Pick up parcel";
                }
                if (mainDroneList.Status == DroneStatus.delivery && ibl.GetParcel(mainDroneList.NumOfParcelOnTheWay).Delivered == null)
                {
                    ChargeDrone.Visibility = Visibility.Hidden;
                    ChangeStatusDrone.Content = "Supply parcel";
                }
            }
        }
        private void weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainDrone = (Drone)DataContext;

        }

        private void addButten_Click(object sender, RoutedEventArgs e)
        {
            ibl.AddDrone(mainDrone, (int)IdStation.SelectedItem);
            add.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ibl.UpdateDrone(mainDroneList.Id, (string)ModelAc.DataContext);
            Actions.Visibility = Visibility.Hidden;
        }

        private void statusA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            statusA.SelectedItem = mainDrone.Status;
        }

        private void statusAC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainDroneList.Status = (DroneStatus)statusAC.SelectedItem;
        }
        private void ChangeStatusDrone_Click(object sender, RoutedEventArgs e)
        {
            if (mainDroneList.Status == DroneStatus.available)
                ibl.ScheduledAParcelToADrone(mainDroneList.Id);
            if (mainDroneList.Status == DroneStatus.delivery && ibl.GetParcel(mainDroneList.NumOfParcelOnTheWay).PickedUp == null)
                ibl.PickUpParcel(mainDroneList.Id);
            if (mainDroneList.Status == DroneStatus.delivery && ibl.GetParcel(mainDroneList.NumOfParcelOnTheWay).Delivered == null)
                ibl.DeliverParcel(mainDroneList.Id);
        }

        private void IdBoxA_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainDrone = (Drone)DataContext;

        }

        private void ModelBoxA_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainDrone = (Drone)DataContext;

        }

        private void ChargeDrone_Click(object sender, RoutedEventArgs e)
        {
            if ((string)ChargeDrone.Content == "Send drone to charging")
                if (mainDroneList.Status == DroneStatus.available)
                    ibl.SendDroneToCharging(mainDroneList.Id);
            if ((string)ChargeDrone.Content == "Release drone from charging")
            {
                FDL.Visibility = Visibility.Visible;
                IdStationCharge.Visibility = Visibility.Visible;
                IdStationCharge.ItemsSource = ibl.GetBaseStations().Select(s => s.Id);
                ibl.FreeDroneFromeCharger(mainDroneList.Id, (int)IdStationCharge.SelectedItem);
            }
        }

        private void IdStation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void IdStationCharge_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
