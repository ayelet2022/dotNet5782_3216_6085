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
    public struct StatusWeightAndPriorities
    {
        public BO.WeightCategories weight { get; set; }
        public BO.ParcelStatus status { get; set; }
        public BO.Priorities priorities { get; set; }
    }
    public enum WeightCategoriesP { light, mediumWeight, heavy,All };
    public enum Priorities { regular, fast, urgent,All };
    public enum ParcelStatus { creat, schedul, pickup, delivery,All }
    /// <summary>
    /// Interaction logic for WindowDrones.xaml
    /// </summary>
    public partial class WindowParcels : Window
    {
        private bool _close { get; set; } = false;
        BlApi.IBL ibl;
        public Dictionary<StatusWeightAndPriorities, List<ParcelList>> Parcels;
        public ParcelList selectedParcel = new();

        /// <summary>
        /// the constractor of the window
        /// </summary>
        /// <param name="bl"> the accses to the fileds in IBL</param>
        public WindowParcels(BlApi.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            Parcels = new Dictionary<StatusWeightAndPriorities, List<ParcelList>>();
            Parcels = (from item in bl.GetParcels()
                       group item by
                       new StatusWeightAndPriorities()
                       {
                           status = item.ParcelStatus,
                           weight = item.Weight,
                           priorities = item.Priority,
                       }).ToDictionary(item => item.Key, item => item.ToList());
            ParcelsListView.ItemsSource = Parcels.SelectMany(item => item.Value);//to show all the drones 
            StatusSelector.ItemsSource = System.Enum.GetValues(typeof(ParcelStatus));
            WeightSelector.ItemsSource = System.Enum.GetValues(typeof(WeightCategories));
            PrioritiesSelector.ItemsSource= System.Enum.GetValues(typeof(Priorities)); 
            StatusSelector.SelectedIndex = 4;//no filter
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
        public void Selector()
        {
            ParcelStatus pStatus = (ParcelStatus)StatusSelector.SelectedItem;
            if (WeightSelector.SelectedIndex == -1)//meens no filter was chosen
                WeightSelector.SelectedIndex = 3;//no filter-shows all the drones
            if (PrioritiesSelector.SelectedIndex == -1)
                PrioritiesSelector.SelectedIndex = 3;
            Priorities pPriorities=(Priorities)PrioritiesSelector.SelectedItem; 
            WeightCategories dWeight = (WeightCategories)WeightSelector.SelectedItem;
            //if no filter was chosen-show the all list
            if (pStatus == ParcelStatus.All && dWeight == WeightCategories.All&&pPriorities==Priorities.All)
                ParcelsListView.ItemsSource = from item in Parcels.Values.SelectMany(x => x)
                                             orderby item.Weight, item.ParcelStatus,item.Priority
                                             select item;//to show the all list//to show the all list
            //if only he wants to filter the weight category
            if (pStatus == ParcelStatus.All && dWeight == WeightCategories.All&&pPriorities!=Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.priorities == (BO.Priorities)PrioritiesSelector.SelectedItem).SelectMany(item => item.Value);
            //if only he wants to filter the statuse category
            if (pStatus == ParcelStatus.All && dWeight != WeightCategories.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.weight == (BO.WeightCategories)WeightSelector.SelectedItem).SelectMany(item => item.Value);
            //if  he wants to filter both the weight category and the status category
            if (pStatus != ParcelStatus.All && dWeight == WeightCategories.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.status == (BO.ParcelStatus)StatusSelector.SelectedItem).SelectMany(item => item.Value);
            if (pStatus == ParcelStatus.All && dWeight != WeightCategories.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.priorities == (BO.Priorities)PrioritiesSelector.SelectedItem&& item.Key.weight == (BO.WeightCategories)WeightSelector.SelectedItem).SelectMany(item => item.Value);
            //if only he wants to filter the statuse category
            if (pStatus != ParcelStatus.All && dWeight == WeightCategories.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.priorities == (BO.Priorities)PrioritiesSelector.SelectedItem && item.Key.status == (BO.ParcelStatus)StatusSelector.SelectedItem).SelectMany(item => item.Value);
            if (pStatus != ParcelStatus.All && dWeight != WeightCategories.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.weight == (BO.WeightCategories)WeightSelector.SelectedItem && item.Key.status == (BO.ParcelStatus)StatusSelector.SelectedItem).SelectMany(item => item.Value);
            if (pStatus != ParcelStatus.All && dWeight != WeightCategories.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Key.weight == (BO.WeightCategories)WeightSelector.SelectedItem && item.Key.priorities == (BO.Priorities)PrioritiesSelector.SelectedItem && item.Key.status == (BO.ParcelStatus)StatusSelector.SelectedItem).SelectMany(item => item.Value);
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
            new WindowParcel(ibl, this, 0).Show();//to show the all the details of the drone and to be able to updae him
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

        private void PrioritiesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Parcels = (from item in ibl.GetParcels()
                       group item by
                       new StatusWeightAndPriorities()
                       {
                           status = item.ParcelStatus,
                           weight = item.Weight,
                           priorities = item.Priority,
                       }).ToDictionary(item => item.Key, item => item.ToList());
            Selector();
        }
    }
}
