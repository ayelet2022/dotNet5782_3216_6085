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
        public WindowDrone(IBL.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            add.Visibility = Visibility.Visible;
            weightA.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            mainDrone.Id = (int)IdBoxA.DataContext;
            mainDrone.Model = (string)ModelBoxA.DataContext;
        }
        public WindowDrone(Drone drone)
        {
            Actions.Visibility = Visibility.Visible;
            IdBoxAc.DataContext = drone.Id;
            BatteryBoxAc.DataContext = drone.Battery;
            status2.SelectedItem = drone.Status;
            LatitudeBoxAc.DataContext = drone.DroneLocation.Latitude;
            LongtitudeBoxAc.DataContext = drone.DroneLocation.Longitude;
            weight2.SelectedItem = drone.MaxWeight;
            ParcelBoxAc.DataContext = drone.ParcelInTransfer.Id;
            mainDrone = drone;
        }

        private void DronesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void weight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mainDrone.MaxWeight = (WeightCategories)statusA.SelectedItem;
            add.Visibility = Visibility.Hidden;
        }

        private void addButten_Click(object sender, RoutedEventArgs e)
        {
            ibl.AddDrone(mainDrone, (int)IdStation.DataContext);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ibl.UpdateDrone(mainDrone.Id, (string)ModelAc.DataContext);
            Actions.Visibility = Visibility.Hidden;
        }
    }
}
