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
    public enum WeightCategoriesP { light, mediumWeight, heavy,All };
    public enum Priorities { regular, fast, urgent,All };
    public enum ParcelStatus { creat, schedul, pickup, delivery,All }
    /// <summary>
    /// Interaction logic for WindowDrones.xaml
    /// </summary>
    public partial class WindowParcels : Window
    {
        private bool _close { get; set; } = false;
        BL.BL ibl;
        public ObservableCollection<ParcelList> Parcels;
        public ParcelList selectedParcel = new();

        /// <summary>
        /// the constractor of the window
        /// </summary>
        /// <param name="bl"> the accses to the fileds in IBL</param>
        public WindowParcels(BL.BL bl)
        {
            InitializeComponent();
            ibl = bl;
            Parcels = new ObservableCollection<ParcelList>();
            List<ParcelList> parcels = ibl.GetParcels().ToList();
            foreach (var item in parcels)//to fet and shoe all the drones
                Parcels.Add(item);
            ParcelsListView.ItemsSource = Parcels;//to show all the drones 
            StatusSelector.ItemsSource = System.Enum.GetValues(typeof(DroneStatus));
            WeightSelector.ItemsSource = System.Enum.GetValues(typeof(WeightCategories));
            StatusSelector.SelectedIndex = 3;//no filter
            Parcels.CollectionChanged += Parcels_CollectionChanged;//if the a drone in the drone list was changed
        }

        /// <summary>
        /// when a parcel in the drones was changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Parcels_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
            if ((ParcelStatus)StatusSelector.SelectedItem == ParcelStatus.All && (WeightCategories)WeightSelector.SelectedItem == WeightCategories.All)
                ParcelsListView.ItemsSource = Parcels;//to show the all list
            //if only he wants to filter the weight category
            if ((ParcelStatus)StatusSelector.SelectedItem == ParcelStatus.All && (WeightCategories)WeightSelector.SelectedItem != WeightCategories.All)
                ParcelsListView.ItemsSource = Parcels.ToList().FindAll(item => item.Weight == (BO.WeightCategories)WeightSelector.SelectedItem);
            //if only he wants to filter the statuse category
            if ((ParcelStatus)StatusSelector.SelectedItem != ParcelStatus.All && (WeightCategories)WeightSelector.SelectedItem == WeightCategories.All)
                ParcelsListView.ItemsSource = Parcels.ToList().FindAll(item => item.ParcelStatus == (BO.ParcelStatus)StatusSelector.SelectedItem);
            //if  he wants to filter both the weight category and the status category
            if ((ParcelStatus)StatusSelector.SelectedItem != ParcelStatus.All && (WeightCategories)WeightSelector.SelectedItem != WeightCategories.All)
                ParcelsListView.ItemsSource = Parcels.ToList().FindAll(item => item.ParcelStatus == (BO.ParcelStatus)StatusSelector.SelectedItem && item.Weight == (BO.WeightCategories)WeightSelector.SelectedItem);
            ParcelsListView.Items.Refresh();
        }

        /// <summary>
        /// to add a new drone to the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new WindowParcel(ibl, this).Show();//t oadd a new drone to the parcel
        }

        /// <summary>
        /// t opresent the drone that the mous double clicked  on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedParcel = (ParcelList)ParcelsListView.SelectedItem;//the drone that the mous double clicked on
            new WindowParcel(ibl, this, ParcelsListView.SelectedIndex).Show();//to show the all the details of the drone and to be able to updae him
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
