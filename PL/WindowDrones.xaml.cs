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
    public enum WeightCategories { Light, MediumWeight, Heavy, All};
    public enum DroneStatus { Available, InFix, Delivery, All};
    /// <summary>
    /// Interaction logic for WindowDrones.xaml
    /// </summary>
    public partial class WindowDrones : Window
    {
        IBL.IBL ibl;
        public ObservableCollection<DroneList> Drones;
        public DroneList selectedDrone = new();

        /// <summary>
        /// the constractor of the window
        /// </summary>
        /// <param name="bl"> the accses to the fileds in IBL</param>
        public WindowDrones(IBL.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            Drones = new ObservableCollection<DroneList>();
            List<DroneList> drones = ibl.GetDrones().ToList();
            foreach (var item in drones)//to fet and shoe all the drones
                Drones.Add(item);
            DronesListView.ItemsSource = Drones;//to show all the drones 
            StatusSelector.ItemsSource = System.Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = System.Enum.GetValues(typeof(WeightCategories));
            StatusSelector.SelectedIndex = 3;//no filter
            Drones.CollectionChanged += Drones_CollectionChanged;//if the a drone in the drone list was changed
        }

        /// <summary>
        /// when a drone in the drones was changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drones_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Selector();//to update  the ist that was printed 
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
        ///  filters the list of drones that was enterd according to what was filtterd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector();// to show the list according to the filter that was enterd
        }
        private void Selector()
        {
            if (WeightSelector.SelectedIndex == -1)
                WeightSelector.SelectedIndex = 3;
            if ((DroneStatus)StatusSelector.SelectedItem == DroneStatus.All && (WeightCategories)WeightSelector.SelectedItem == WeightCategories.All)
                DronesListView.ItemsSource = Drones;
            if ((DroneStatus)StatusSelector.SelectedItem == DroneStatus.All && (WeightCategories)WeightSelector.SelectedItem != WeightCategories.All)
                DronesListView.ItemsSource = Drones.ToList().FindAll(item => item.MaxWeight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem);
            if ((DroneStatus)StatusSelector.SelectedItem != DroneStatus.All && (WeightCategories)WeightSelector.SelectedItem == WeightCategories.All)
                DronesListView.ItemsSource = Drones.ToList().FindAll(item => item.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem);
            if ((DroneStatus)StatusSelector.SelectedItem != DroneStatus.All && (WeightCategories)WeightSelector.SelectedItem != WeightCategories.All)
                DronesListView.ItemsSource = Drones.ToList().FindAll(item => item.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem && item.MaxWeight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem);
            DronesListView.Items.Refresh();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new WindowDrone(ibl, this).Show();
        }
        private void DronesListView_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            selectedDrone=(DroneList)DronesListView.SelectedItem;
            new WindowDrone(ibl, this, DronesListView.SelectedIndex).Show();
        }
        private void CloseWDS_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
