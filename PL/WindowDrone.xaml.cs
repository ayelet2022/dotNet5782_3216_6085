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

        }
        public WindowDrone(DroneList droneList)
        {

            InitializeComponent();
            Actions.Visibility=Visibility.Visible;
            mainDroneList = droneList;
            IdBoxAc.DataContext = droneList.Id;
            BatteryBoxAc.DataContext = droneList.Battery;
            LatitudeBoxAc.DataContext = droneList.DroneLocation.Latitude;
            LongtitudeBoxAc.DataContext = droneList.DroneLocation.Longitude;
            weightAC.SelectedItem = droneList.MaxWeight;
            ParcelBoxAc.DataContext = droneList.NumOfParcelOnTheWay;
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
        private void weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainDrone.MaxWeight = (WeightCategories)weightA.SelectedItem;
        }

        private void addButten_Click(object sender, RoutedEventArgs e)
        {
            ibl.AddDrone(mainDrone, (int)IdStation.DataContext);
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
            mainDrone.Id = (int)IdBoxA.DataContext;

        }

        private void ModelBoxA_TextChanged(object sender, TextChangedEventArgs e)
        {
            mainDrone.Model = (string)ModelBoxA.DataContext;
        }

        private void ChargeDrone_Click(object sender, RoutedEventArgs e)
        {
            if ((string)ChargeDrone.DataContext == "Send drone to charging")
                if (mainDroneList.Status == DroneStatus.available)
                    ibl.SendDroneToCharging(mainDroneList.Id);
            if ((string)ChargeDrone.DataContext == "Release drone from charging")
            {
                FDL.Visibility = Visibility.Visible;
                FDB.Visibility = Visibility.Visible;
                ibl.FreeDroneFromeCharger(mainDroneList.Id, (int)FDB.DataContext);
            }
        }
    }
}
