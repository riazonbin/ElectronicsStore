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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicsStore.Controls
{
    /// <summary>
    /// Interaction logic for HistoryOfOrdersUserControl.xaml
    /// </summary>
    public partial class HistoryOfOrdersUserControl : UserControl
    {
        public HistoryOfOrdersUserControl()
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
            if (App.CurrentUser.Id == 1)
            {
                DgHistoryOrders.ItemsSource = App.Connection.Order.Where(x => x.OrderStatus_Id == 5).ToList();
            }
            else
            {
                DgHistoryOrders.ItemsSource = App.Connection.Order.Where(x => x.OrderStatus_Id == 5
                && x.User_Id == App.CurrentUser.Id).ToList();
            }
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
