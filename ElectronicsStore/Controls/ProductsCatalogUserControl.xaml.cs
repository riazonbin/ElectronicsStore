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
using System.Data.Entity.Migrations;
using ElectronicsStore.Controls.ManagerControls;

namespace ElectronicsStore.Controls
{
    /// <summary>
    /// Interaction logic for UserControlHome.xaml
    /// </summary>
    public partial class ProductsCatalogUserControl : UserControl
    {
        Grid _gridMain;
        private Predicate<Product> _filterQuery = x => true;
        private Func<Product, object> _sortingQuery = x => x.Id;
        List<Product> MenuItemsList { get; set; }
        public ProductsCatalogUserControl(Grid GridMain)
        {
            InitializeComponent();
            _gridMain = GridMain;
        }

        private void CreateDishButtonClick(object sender, RoutedEventArgs e)
        {
            _gridMain.Children.Clear();

            UserControl usc = new CreatingProductManagerControl(null, _gridMain);
            _gridMain.Children.Add(element: usc);
        }

        private void EditProduct(object sender, RoutedEventArgs e)
        {
            _gridMain.Children.Clear();

            var productId = (int)((Button)sender).Tag;

            var product = App.Connection.Product.FirstOrDefault(x => x.Id == productId);


            UserControl usc = new CreatingProductManagerControl(product, _gridMain);
            _gridMain.Children.Add(element: usc);
        }

        private void AddToCartBtnClick(object sender, RoutedEventArgs e)
        {
            var productId = (int)((Button)sender).Tag;

            var product = App.Connection.Product.FirstOrDefault(x => x.Id == productId);

            var basket = App.Connection.Basket.FirstOrDefault(x => x.User_Id == App.CurrentUser.Id && x.Product_Id == productId);

            if (basket is null)
            {
                basket = new Basket
                {
                    Product = product,
                    User = App.CurrentUser,
                    Count = 1
                };
            }
            else
            {
                basket.Count++;
            }

            App.Connection.Basket.AddOrUpdate(basket);
            App.Connection.SaveChanges();

            SnackbarOne.MessageQueue?.Enqueue($"Товар \"{product.Name}\" добавлен в корзину!",
                null, null, null, false, true, TimeSpan.FromSeconds(2));
        }

        private void UpdateMenuItems()
        {
            if(!IsInitialized)
            {
                return;
            }

            MenuItemsList = App.Connection.Product.ToList()
                .Where(x => _filterQuery(x) && x.IsMarkedForDeletion != true)
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
                    _sortingQuery = x => x.Id;
                    break;

                case 1:
                    _sortingQuery = x => x.Name;
                    break;

                case 2:
                    _sortingQuery = x => x.Price;
                    break;

                case 3:
                    _sortingQuery = x => -x.Price;
                    break;
            }

            UpdateMenuItems();
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            List<ProductType> productTypes = new List<ProductType>();
            productTypes.Add(new ProductType { Name = "Все" });

            productTypes.AddRange(App.Connection.ProductType.ToList());
            CbCategory.ItemsSource = productTypes;
            CbCategory.SelectedIndex = 0;

            UpdateMenuItems();
        }

        private void CbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CbCategory.SelectedIndex == 0) 
            {
                _filterQuery = x => true;
                UpdateMenuItems();
                return;
            }

            ProductType selectedProductType = CbCategory.SelectedItem as ProductType;

            if(selectedProductType != null)
            {
                _filterQuery = x => x.ProductType_Id == selectedProductType.Id;
                UpdateMenuItems();
            }

        }
    }
}
