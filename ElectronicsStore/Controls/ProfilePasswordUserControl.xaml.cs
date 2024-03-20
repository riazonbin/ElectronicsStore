using ElectronicsStore.ADO;
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

namespace ElectronicsStore.Controls
{
    /// <summary>
    /// Interaction logic for ProfilePasswordUserControl.xaml
    /// </summary>
    public partial class ProfilePasswordUserControl : UserControl
    {
        Grid _gridMain;

        public ProfilePasswordUserControl(Grid gridMain)
        {
            _gridMain = gridMain;

            InitializeComponent();
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            if (PbOldPassword.Password == "" || PbNewPassword.Password == "")
            {
                SnackbarOne.MessageQueue?.Enqueue($"Вы не заполнили все данные",
                    null, null, null, false,
                    true, TimeSpan.FromSeconds(2));
                return;
            }

            var foundUser = App.Connection.User.FirstOrDefault(x => x.Id == App.CurrentUser.Id && x.Password == PbOldPassword.Password);

            if (foundUser == null)
            {
                SnackbarOne.MessageQueue?.Enqueue($"Старый пароль не корректен!",
                null, null, null, false,
                true, TimeSpan.FromSeconds(2));
                return;
            }

            foundUser.Password = PbNewPassword.Password;

            App.Connection.SaveChanges();

            SnackbarSuccess.MessageQueue?.Enqueue($"Данные успешно сохранены!",
            null, null, null, false,
            true, TimeSpan.FromSeconds(2));

            PbOldPassword.Password = "";
            PbNewPassword.Password = "";
        }

        private void GoToProfileHyperLink(object sender, RoutedEventArgs e)
        {
            _gridMain.Children.Clear();

            UserControl usc = new PersonalCabinetUserControl(_gridMain);
            _gridMain.Children.Add(element: usc);
        }
    }
}
