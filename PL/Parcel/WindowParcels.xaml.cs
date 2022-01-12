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
    public enum WeightCategoriesP { light, mediumWeight, heavy, All };
    public enum Priorities { regular, fast, urgent, All };
    public enum ParcelStatus { creat, schedul, pickup, delivery, All }

    /// <summary>
    /// Interaction logic for WindowDrones.xaml
    /// </summary>
    public partial class WindowParcels : Window
    {
        private bool _close { get; set; } = false;
        BlApi.IBL ibl;
        public ObservableCollection<ParcelList> Parcels;
        public ParcelList selectedParcel = new();

        /// <summary>
        /// the constractor of the window
        /// </summary>
        /// <param name="bl"> the accses to the fileds in IBL</param>
        public WindowParcels(BlApi.IBL bl)
        {
            InitializeComponent();
            ibl = bl;
            Parcels = new ObservableCollection<ParcelList>();
            List<ParcelList> parcels = bl.GetParcels().ToList();
            parcels.OrderBy(item => item.Id);
            Parcels= (ObservableCollection<ParcelList>)(from item in parcels 
                    select item);
            ParcelsListView.ItemsSource = Parcels;
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
            if (pStatus == ParcelStatus.All && dWeight == WeightCategories.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels;
            //if only he wants to filter the weight category
            if (pStatus == ParcelStatus.All && dWeight == WeightCategories.All&&pPriorities!=Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Priority == (BO.Priorities)PrioritiesSelector.SelectedItem).Select(item => item);
            //if only he wants to filter the statuse category
            if (pStatus == ParcelStatus.All && dWeight != WeightCategories.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Weight == (BO.WeightCategories)WeightSelector.SelectedItem).Select(item => item);
            //if  he wants to filter both the weight category and the status category
            if (pStatus != ParcelStatus.All && dWeight == WeightCategories.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.ParcelStatus == (BO.ParcelStatus)StatusSelector.SelectedItem).Select(item => item);
            if (pStatus == ParcelStatus.All && dWeight != WeightCategories.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Priority == (BO.Priorities)PrioritiesSelector.SelectedItem&& item.Weight == (BO.WeightCategories)WeightSelector.SelectedItem).Select(item => item);
            //if only he wants to filter the statuse category
            if (pStatus != ParcelStatus.All && dWeight == WeightCategories.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Priority == (BO.Priorities)PrioritiesSelector.SelectedItem && item.ParcelStatus == (BO.ParcelStatus)StatusSelector.SelectedItem).Select(item => item);
            if (pStatus != ParcelStatus.All && dWeight != WeightCategories.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Weight == (BO.WeightCategories)WeightSelector.SelectedItem && item.ParcelStatus == (BO.ParcelStatus)StatusSelector.SelectedItem).Select(item => item);
            if (pStatus != ParcelStatus.All && dWeight != WeightCategories.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Weight == (BO.WeightCategories)WeightSelector.SelectedItem && item.Priority == (BO.Priorities)PrioritiesSelector.SelectedItem && item.ParcelStatus == (BO.ParcelStatus)StatusSelector.SelectedItem).Select(item => item);
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
            Parcels = (ObservableCollection<ParcelList>)ibl.GetParcels();
            Selector();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement framework = sender as FrameworkElement;
            selectedParcel = framework.DataContext as ParcelList;
            try
            {
                ibl.DeleteCustomer(selectedParcel.Id);
                Parcels.Remove(selectedParcel);
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone has been deleted successfully \n" + selectedParcel.ToString());
            }
            catch (BO.ItemIsDeletedException ex)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("The drone was not deleted \n" + selectedParcel.ToString());
            }

        }
    }
}
