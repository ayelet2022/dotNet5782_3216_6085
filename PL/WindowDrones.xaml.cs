using System;
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
using IBL.BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for WindowDrones.xaml
    /// </summary>
    public partial class WindowDrones : Window
    {
        IBL.IBL ibl;
        public ObservableCollection<DroneList> Drones;
        public WindowDrones(IBL.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            Drones = new ObservableCollection<DroneList>();
            List<DroneList> drones = ibl.GetDrones().ToList();
            foreach (var item in drones)
                Drones.Add(item);
            DronesListView.ItemsSource = Drones;
            ComboBoxItem newItem = new ComboBoxItem();
            string a = "default(no filter)";
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            Drones.CollectionChanged += Drones_CollectionChanged;
        }

        private void Drones_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StatusSelector.SelectedIndex = StatusSelector.SelectedIndex;
            WeightSelector.SelectedIndex = WeightSelector.SelectedIndex;
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector();
        }
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector();
        }
        private void Selector()
        {
            if (StatusSelector.SelectedItem == null && WeightSelector.SelectedItem != null)
                DronesListView.ItemsSource = Drones.ToList().FindAll(item => item.MaxWeight == (WeightCategories)WeightSelector.SelectedItem);
            if (StatusSelector.SelectedItem != null && WeightSelector.SelectedItem == null)
                DronesListView.ItemsSource = Drones.ToList().FindAll(item => item.Status == (DroneStatus)StatusSelector.SelectedItem);
            if (StatusSelector.SelectedItem != null && WeightSelector.SelectedItem != null)
                DronesListView.ItemsSource = Drones.ToList().FindAll(item => item.Status == (DroneStatus)StatusSelector.SelectedItem && item.MaxWeight == (WeightCategories)WeightSelector.SelectedItem);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new WindowDrone(ibl, this).Show();
        }

        private void DronesListView_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            DroneList drone = (DroneList)DronesListView.SelectedItem;
            Drone mainDrone = ibl.GetDrone(drone.Id);
            new WindowDrone(mainDrone, ibl, this).Show();
        }

        private void CloseWDS_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
