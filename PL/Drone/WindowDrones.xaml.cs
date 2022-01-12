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
    public struct StatusAndWeight
    {
        public BO.WeightCategories weight { get; set; }
        public BO.DroneStatus status { get; set; }
    }
    public enum WeightCategories { Light, MediumWeight, Heavy, All };
    public enum DroneStatus { Available, InFix, Delivery, All };
    /// <summary>
    /// Interaction logic for WindowDrones.xaml
    /// </summary>
    public partial class WindowDrones : Window
    {
        BlApi.IBL ibl;
        public Dictionary<StatusAndWeight, List<DroneList>> Drones;
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
            Drones = new Dictionary<StatusAndWeight, List<DroneList>>();
            Drones = (from item in bl.GetDrones()
                      group item by
                      new StatusAndWeight()
                      {
                          status = item.Status,
                          weight = item.MaxWeight
                      }).ToDictionary(item => item.Key, item => item.ToList());
            DronesListView.ItemsSource = Drones.SelectMany(item => item.Value);//to show all the drones
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
            StatusSelector.ItemsSource = System.Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = System.Enum.GetValues(typeof(WeightCategories));
            StatusSelector.SelectedIndex = 3;//no filter
            //Drones.CollectionChanged += Drones_CollectionChanged;//if the a drone in the drone list was changed
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
        public void Selector()
        {
            DroneStatus dStatus = (DroneStatus)StatusSelector.SelectedItem;
            if (WeightSelector.SelectedIndex == -1)//meens no filter was chosen
                WeightSelector.SelectedIndex = 3;//no filter-shows all the drones
            WeightCategories dWeight = (WeightCategories)WeightSelector.SelectedItem;
            //if no filter was chosen-show the all list
            if (dStatus == DroneStatus.All && dWeight == WeightCategories.All)
                DronesListView.ItemsSource = from item in Drones.Values.SelectMany(x => x)
                                             orderby item.MaxWeight, item.Status
                                             select item;//to show the all list
                                                         //if only he wants to filter the weight category
            if (dStatus == DroneStatus.All && dWeight != WeightCategories.All)
                DronesListView.ItemsSource = Drones.Where(item => item.Key.weight == (BO.WeightCategories)WeightSelector.SelectedItem).SelectMany(item => item.Value);
            //if only he wants to filter the statuse category
            if (dStatus != DroneStatus.All && dWeight == WeightCategories.All)
                DronesListView.ItemsSource = Drones.Where(item => item.Key.status == (BO.DroneStatus)StatusSelector.SelectedItem).SelectMany(item => item.Value);
            //if  he wants to filter both the weight category and the status category
            if (dStatus != DroneStatus.All && dWeight != WeightCategories.All)
                DronesListView.ItemsSource = Drones.Where(item => item.Key.weight == (BO.WeightCategories)WeightSelector.SelectedItem && item.Key.status == (BO.DroneStatus)StatusSelector.SelectedItem)
                    .SelectMany(item => item.Value);
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
        public void MyRefresh()
        {
            Drones = (from item in ibl.GetDrones()
                      group item by
                      new StatusAndWeight()
                      {
                          status = item.Status,
                          weight = item.MaxWeight
                      }).ToDictionary(item => item.Key, item => item.ToList());
            DronesListView.ItemsSource = Drones.SelectMany(item => item.Value);//to show all the drones
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DronesListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
            Selector();
        }
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            MyRefresh();
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement framework = sender as FrameworkElement;
            selectedDrone = framework.DataContext as DroneList;
            try
            {
                ibl.DeleteDrone(selectedDrone.Id);
                StatusAndWeight statusAndWeight = new() { status = selectedDrone.Status, weight = selectedDrone.MaxWeight };
                Drones[statusAndWeight].RemoveAll(i => i.Id == selectedDrone.Id);
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been deleted successfully \n" + selectedDrone.ToString());
                Selector();
            }
            catch (BO.ItemIsDeletedException ex)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone was not deleted \n" + selectedDrone.ToString());
            }

        }
    }
}
