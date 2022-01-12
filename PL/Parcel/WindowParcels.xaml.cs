using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            IEnumerable<ParcelList> parcels = bl.GetParcels();
            parcels.OrderBy(item => item.Id);
            foreach (var item in parcels)Parcels.Add(item);
            ParcelsListView.ItemsSource = Parcels;
        }

        /// <summary>
        /// filters the list of parcels that was enterd according to what was filtterd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector();//to show the list according to the filter that was enterd
        }

        /// <summary>
        /// filters the list of parcels that was enterd according to what was filtterd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrioritiesSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector();
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
            WeightCategoriesP dWeight = (WeightCategoriesP)WeightSelector.SelectedItem;
            //if no filter was chosen-show the all list
            if (pStatus == ParcelStatus.All && dWeight == WeightCategoriesP.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels;
            //if only he wants to filter the weight category
            if (pStatus == ParcelStatus.All && dWeight == WeightCategoriesP.All&&pPriorities!=Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Priority == (BO.Priorities)PrioritiesSelector.SelectedItem).Select(item => item);
            //if only he wants to filter the statuse category
            if (pStatus == ParcelStatus.All && dWeight != WeightCategoriesP.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Weight == (BO.WeightCategories)WeightSelector.SelectedItem).Select(item => item);
            //if  he wants to filter both the weight category and the status category
            if (pStatus != ParcelStatus.All && dWeight == WeightCategoriesP.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.ParcelStatus == (BO.ParcelStatus)StatusSelector.SelectedItem).Select(item => item);
            if (pStatus == ParcelStatus.All && dWeight != WeightCategoriesP.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Priority == (BO.Priorities)PrioritiesSelector.SelectedItem&& item.Weight == (BO.WeightCategories)WeightSelector.SelectedItem).Select(item => item);
            //if only he wants to filter the statuse category
            if (pStatus != ParcelStatus.All && dWeight == WeightCategoriesP.All && pPriorities != Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Priority == (BO.Priorities)PrioritiesSelector.SelectedItem && item.ParcelStatus == (BO.ParcelStatus)StatusSelector.SelectedItem).Select(item => item);
            if (pStatus != ParcelStatus.All && dWeight != WeightCategoriesP.All && pPriorities == Priorities.All)
                ParcelsListView.ItemsSource = Parcels.Where(item => item.Weight == (BO.WeightCategories)WeightSelector.SelectedItem && item.ParcelStatus == (BO.ParcelStatus)StatusSelector.SelectedItem).Select(item => item);
            if (pStatus != ParcelStatus.All && dWeight != WeightCategoriesP.All && pPriorities != Priorities.All)
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

        

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            MyRefresh();
        }

        public void MyRefresh()
        {
            IEnumerable<ParcelList> parcels = ibl.GetParcels();
            parcels.OrderBy(item => item.Id);
            Parcels = new ObservableCollection<ParcelList>();
            foreach (var item in parcels) Parcels.Add(item);
            ParcelsListView.ItemsSource = Parcels;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement framework = sender as FrameworkElement;
            selectedParcel = framework.DataContext as ParcelList;
            try
            {
                ibl.DeleteParcel(selectedParcel.Id);
                Parcels.Remove(selectedParcel);
                MessageBoxResult messageBoxResult = MessageBox.Show("The parcel has been deleted successfully \n" + selectedParcel.ToString());
            }
            catch (BO.ItemIsDeletedException ex)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("The parcel was not deleted \n" + selectedParcel.ToString());
            }

        }
    }
}
