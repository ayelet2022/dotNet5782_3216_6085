using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using BO;

namespace PL
{
    public enum WeightCategories { Light, MediumWeight, Heavy, All};
    public enum DroneStatus { Available, InFix, Delivery, All};
    /// <summary>
    /// Interaction logic for WindowDrones.xaml
    /// </summary>
    public partial class WindowDrones : Window
    {
        private bool _close { get; set; } = false;
        BL.BL ibl;
        public ObservableCollection<DroneList> Drones;
        public DroneList selectedDrone = new();

        /// <summary>
        /// the constractor of the window
        /// </summary>
        /// <param name="bl"> the accses to the fileds in IBL</param>
        public WindowDrones(BL.BL bl)
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

        /// <summary>
        /// shows the list according to the filter that the user disided
        /// </summary>
        private void Selector()
        {
            if (WeightSelector.SelectedIndex == -1)//meens no filter was chosen
                WeightSelector.SelectedIndex = 3;//no filter-shows all the drones
            //if no filter was chosen-show the all list
            if ((DroneStatus)StatusSelector.SelectedItem == DroneStatus.All && (WeightCategories)WeightSelector.SelectedItem == WeightCategories.All)
                DronesListView.ItemsSource = Drones;//to show the all list
            //if only he wants to filter the weight category
            if ((DroneStatus)StatusSelector.SelectedItem == DroneStatus.All && (WeightCategories)WeightSelector.SelectedItem != WeightCategories.All)
                DronesListView.ItemsSource = Drones.ToList().FindAll(item => item.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedItem);
            //if only he wants to filter the statuse category
            if ((DroneStatus)StatusSelector.SelectedItem != DroneStatus.All && (WeightCategories)WeightSelector.SelectedItem == WeightCategories.All)
                DronesListView.ItemsSource = Drones.ToList().FindAll(item => item.Status == (BO.DroneStatus)StatusSelector.SelectedItem);
            //if  he wants to filter both the weight category and the status category
            if ((DroneStatus)StatusSelector.SelectedItem != DroneStatus.All && (WeightCategories)WeightSelector.SelectedItem != WeightCategories.All)
                DronesListView.ItemsSource = Drones.ToList().FindAll(item => item.Status == (BO.DroneStatus)StatusSelector.SelectedItem && item.MaxWeight == (BO.WeightCategories)WeightSelector.SelectedItem);
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

        /// <summary>
        /// t opresent the drone that the mous double clicked  on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DronesListView_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            selectedDrone=(DroneList)DronesListView.SelectedItem;//the drone that the mous double clicked on
            new WindowDrone(ibl, this, DronesListView.SelectedIndex).Show();//to show the all the details of the drone and to be able to updae him
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
    }
}
