using ElectronicsStore.Pages;
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

namespace ElectronicsStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartTimer();
            MainWindowFrame.Navigate(new AuthorizationPage());
        }

        void StartTimer()
        {
            App.dispatcherTimer.Tick += new EventHandler(dispatcherTimerTick);
            App.dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
            App.dispatcherTimer.Start();
        }

        private void dispatcherTimerTick(object sender, EventArgs e)
        {
            UpdateOrderStatuses();
        }

        private void UpdateOrderStatuses()
        {
            Random random = new Random();

            foreach (var order in App.Connection.Order.Where(x => x.OrderStatus_Id != 5).ToList())
            {

                if(random.Next(1, 5) == 1)
                {
                    order.OrderStatus_Id += 1;
                }

                if(order.OrderStatus_Id== 5)
                {
                    order.OrderEndDate = DateTime.Now;
                }

                random = new Random();
            }
            App.Connection.SaveChanges();
        }
    }
}
