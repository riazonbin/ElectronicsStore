using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicsStore.Controls
{
    /// <summary>
    /// Interaction logic for CurrentOrdersUserControl.xaml
    /// </summary>
    public partial class CurrentOrdersUserControl : UserControl
    {

        public CurrentOrdersUserControl()
        {
            InitializeComponent();
            App.dispatcherTimer.Tick += DispatcherTimerTick;
        }

        private void DispatcherTimerTick(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            try
            {
                if (App.CurrentUser.Id == 1)
                {
                    DgCurrentOrders.ItemsSource = App.Connection.Order.Where(x => x.OrderStatus_Id != 5).ToList();
                }
                else
                {
                    DgCurrentOrders.ItemsSource = App.Connection.Order.Where(x => x.OrderStatus_Id != 5
                    && x.User_Id == App.CurrentUser.Id).ToList();
                }
            }
            catch { }
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            DgClientColumn.Visibility = App.CurrentUser.Role_Id == 1 ? Visibility.Visible : Visibility.Collapsed;
            RefreshData();
        }

        private void UserControlUnloaded(object sender, RoutedEventArgs e)
        {
            App.dispatcherTimer.Tick -= DispatcherTimerTick;
        }
    }
}
