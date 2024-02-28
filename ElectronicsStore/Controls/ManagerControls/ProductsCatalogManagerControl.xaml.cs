using MaterialDesignThemes.Wpf;
using ElectronicsStore.ADO;
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

namespace ElectronicsStore.Controls.ManagerControls
{
    /// <summary>
    /// Interaction logic for UserControlHome.xaml
    /// </summary>
    public partial class ProductsCatalogManagerControl : UserControl
    {
        Grid _gridMain;
        private Predicate<Product> _discountFilter = x => true;
        private Func<Product, object> _sortingQuery = x => x.Id;
        List<Product> MenuItemsList { get; set; }
        public ProductsCatalogManagerControl(Grid GridMain)
        {
            InitializeComponent();
            _gridMain = GridMain;
        }

        private void EditDishButtonClick(object sender, RoutedEventArgs e)
        {
            _gridMain.Children.Clear();

            var menuItemId = (int)((Button)sender).Tag;

            var menuItem = App.Connection.Product.FirstOrDefault(x => x.Id == menuItemId);

            UserControl usc = new CreatingProductManagerControl(menuItem, _gridMain);
            _gridMain.Children.Add(element: usc);
        }

        private void CreateDishButtonClick(object sender, RoutedEventArgs e)
        {
            _gridMain.Children.Clear();

            UserControl usc = new CreatingProductManagerControl(null, _gridMain);
            _gridMain.Children.Add(element: usc);
        }

        private void CbDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CbDiscount.SelectedIndex)
            {
                case 0:
                    _discountFilter = x => true;
                    break;

                case 1:
                    _discountFilter = x => x.Discount.Value == 0;
                    break;

                case 2:
                    _discountFilter = x => x.Discount.Value == 5;
                    break;

                case 3:
                    _discountFilter = x => x.Discount.Value == 10;
                    break;

                case 4:
                    _discountFilter = x => x.Discount.Value == 15;
                    break;

                case 5:
                    _discountFilter = x => x.Discount.Value == 20;
                    break;

                case 6:
                    _discountFilter = x => x.Discount.Value == 35;
                    break;
            }

            UpdateMenuItems();
        }

        private void UpdateMenuItems()
        {
            if(!IsInitialized)
            {
                return;
            }

            MenuItemsList = App.Connection.Product.ToList()
                .Where(x => _discountFilter(x) && x.IsMarkedForDeletion != true)
                .OrderBy(x => _sortingQuery(x))
                .ToList();

            SearchMenuItems();
        }

        private void SearchMenuItems()
        {
            LvMenuItems.ItemsSource = MenuItemsList
                .Where(x => string.Join(" ", x.Name)
                .ToLower()
                .Contains(TbSearch.Text.ToLower()));
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchMenuItems();
        }

        private void CbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CbSort.SelectedIndex)
            {
                case 0:
                    _sortingQuery = x => x.Name;
                    break;

                case 1:
                    _sortingQuery = x => x.Price;
                    break;

                default:
                    _sortingQuery = x => -x.Price;
                    break;
            }

            UpdateMenuItems();
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            UpdateMenuItems();
        }
    }
}
