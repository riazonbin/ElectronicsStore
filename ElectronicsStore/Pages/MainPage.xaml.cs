using ElectronicsStore.Controls;
using ElectronicsStore.Controls.ClientControls;
using ElectronicsStore.Controls.ManagerControls;
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

namespace ElectronicsStore.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public Visibility IsManagerRole;

        public MainPage()
        {
            InitializeComponent();
            SelectDefaultControl();

            IsManagerRole = App.CurrentUser.Role_Id == 2 ? Visibility.Visible : Visibility.Collapsed;
            (ListViewMenu.FindName("ItemBasket") as ListViewItem).Visibility = IsManagerRole;
            (ListViewMenu.FindName("ItemAccount") as ListViewItem).Visibility = IsManagerRole;
        }

        void SelectDefaultControl()
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            if (App.CurrentUser.Role.Id == 1)
            {
                usc = new ProductsCatalogUserControl(GridMain);
            }
            else
            {
                usc = new ProductsCatalogUserControl(GridMain);
            }

            GridMain.Children.Add(usc);
            ListViewMenu.SelectedIndex = 1;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemProducts":
                    usc = new ProductsCatalogUserControl(GridMain);
                    GridMain.Children.Add(usc);
                    break;

                case "ItemHistory":
                    usc = new HistoryOfOrdersUserControl();
                    GridMain.Children.Add(element: usc);
                    break;

                case "ItemBasket":
                    usc = new BasketClientControl();
                    GridMain.Children.Add(element: usc);
                    break;

                case "ItemAccount":
                    usc = new PersonalCabinetUserControl(GridMain);
                    GridMain.Children.Add(element: usc);
                    break;

                case "ItemCurrentOrders":
                    usc = new CurrentOrdersUserControl();
                    GridMain.Children.Add(element: usc);
                    break;
                default:
                    break;
            }
        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Вы уверены, что хотите выйти?";
            string sCaption = "Подтверждение";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Question;

            MessageBoxResult rsltMessageBox = (MessageBoxResult)ModernWpf.MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    NavigationService.Navigate(new AuthorizationPage());
                    break;

                case MessageBoxResult.No:
                    /* ... */
                    break;
            }
        }
    }
}
