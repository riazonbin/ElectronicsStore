using ElectronicsStore.ADO;
using ElectronicsStore.Controls.ManagerControls;
using ElectronicsStore.Pages;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
using System.Xml.Linq;

namespace ElectronicsStore.Controls
{
    /// <summary>
    /// Interaction logic for PersonalCabinetUserControl.xaml
    /// </summary>
    public partial class PersonalCabinetUserControl : UserControl
    {
        Grid _gridMain;
        bool IsAdminMode = App.CurrentUser.Role_Id == 1;

        public PersonalCabinetUserControl(Grid gridMain)
        {
            _gridMain = gridMain;
            InitializeComponent();

            DataContext = App.CurrentUser;

            //wProfileButton.Content = $"{App.CurrentUser.Lastname[0]}.{App.CurrentUser.Firstname[0]}";
        }

        private void EditSaveChangesButtonClick(object sender, RoutedEventArgs e)
        {
            if (TbEditSave.Text == "Редактировать данные")
            {
                TbLogin.IsEnabled = true;
                TbFirstname.IsEnabled = true;
                TbLastname.IsEnabled = true;

                TbEditSave.Text = "Сохранить изменения";
                return;
            }


            if (TbFirstname.Text == "" || TbLastname.Text == "" || TbLogin.Text == "")
            {
                ModernWpf.MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            App.Connection.User.AddOrUpdate(App.CurrentUser);
            App.Connection.SaveChanges();

            ProfileButton.Content = $"{App.CurrentUser.Lastname[0]}.{App.CurrentUser.Firstname[0]}";

            ModernWpf.MessageBox.Show("Пользователь успешно сохранен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            TbLogin.IsEnabled = false;
            TbFirstname.IsEnabled = false;
            TbLastname.IsEnabled = false;
            TbEditSave.Text = "Редактировать данные";

        }

        private void UserControlUnloaded(object sender, RoutedEventArgs e)
        {
            if(App.CurrentUser != null)
            {
                App.Connection.Entry(App.CurrentUser).Reload();
            }
        }

        private void ChangePasswordHyperLink(object sender, RoutedEventArgs e)
        {
            _gridMain.Children.Clear();

            UserControl usc = new ProfilePasswordUserControl(_gridMain);
            _gridMain.Children.Add(element: usc);
        }
    }
}
