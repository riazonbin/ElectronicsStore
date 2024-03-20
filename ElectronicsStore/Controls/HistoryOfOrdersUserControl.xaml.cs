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
        }
        private void ReloadData()
        {
            if (App.CurrentUser.Role.Id == 1)
            {
                LvOrdersHistory.ItemsSource = App.Connection.Order.Where(x => x.User_Id == App.CurrentUser.Id && x.OrderStatus.Id == 6).ToList();
            }
            else
            {
                LvOrdersHistory.ItemsSource = App.Connection.Order.Where(x => x.OrderStatus.Id == 6).ToList();
            }

            if (LvOrdersHistory.Items.Count == 0)
            {
                TbDullHistory.Visibility = Visibility.Visible;
            }
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            TbDullHistory.Visibility = Visibility.Collapsed;
            ReloadData();
        }
    }
}
