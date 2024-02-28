using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicsStore.ADO
{
    public partial class Product
    {
        public Visibility IsAdminMode => App.CurrentUser.Role.Id == 2 ? Visibility.Visible : Visibility.Collapsed;
        public Visibility IsUserMode => App.CurrentUser.Role.Id == 1 ? Visibility.Visible : Visibility.Collapsed;
    }
}
